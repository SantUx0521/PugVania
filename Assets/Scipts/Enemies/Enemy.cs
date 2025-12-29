using System.Collections;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour, InterfaceEnemy
{
    [Header("Estado")]
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLenght;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;
    protected bool isDead = false;

    [Header("Combate")]
    [SerializeField] protected MovementPlayer player;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] private float knockbackForce = 25f;
    [SerializeField] protected GameObject bloodSpurt;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime = -999f;
    private Animator anim;

    [Header("Patrullaje")]
    [SerializeField] protected float chaseRange = 10f;
    [SerializeField] protected float patrolSpeed = 2f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected bool lookingRight = true;
    private Vector2 lastPosition;
    private float minMoveDistance = 0.1f;

    [Header("Patrulla con espera")]
    [SerializeField] private float tiempoAEsperar = 1.5f;
    private float tiempoAEsperarActual = 0f;
    private bool esperandoPatrulla = false;

    protected float recoilTimer;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = MovementPlayer.Instance;
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (health <= 0)
        {
            StartCoroutine(Death());
        }

        HandleRecoil();

        if (player == null)
        {
            player = MovementPlayer.Instance;
            if (player == null) return;
        }

        if (!isRecoiling && isDead == false)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= chaseRange)
            {
                ChasePlayer();
            }
            else
            {
                if (esperandoPatrulla)
                {
                    ComportamientoEsperar();
                }
                else
                {
                    ComportamientoPatrullaje();
                }
            }
        }
    }

    public IEnumerator Death()
    {
        anim.SetTrigger("Death");
        isDead = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    protected void HandleRecoil()
    {
        if (isRecoiling)
        {
            if (recoilTimer < recoilLenght)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
    }

    protected virtual void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            new Vector2(player.transform.position.x, transform.position.y),
            speed * Time.deltaTime
        );

        FlipToPlayer();
    }

    protected virtual void ComportamientoPatrullaje()
    {
        transform.position += new Vector3((lookingRight ? 1 : -1) * patrolSpeed * Time.deltaTime, 0, 0);

        if (!Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer))
        {
            Flip();
            CambiarAEstadoEsperar();
        }

        if (Vector2.Distance(transform.position, lastPosition) <= minMoveDistance)
        {
            Flip();
            CambiarAEstadoEsperar();
        }

        lastPosition = transform.position;
    }

    protected void CambiarAEstadoEsperar()
    {
        esperandoPatrulla = true;
        tiempoAEsperarActual = tiempoAEsperar;
    }

    protected void ComportamientoEsperar()
    {
        tiempoAEsperarActual -= Time.deltaTime;
        if (tiempoAEsperarActual <= 0)
        {
            esperandoPatrulla = false;
        }
    }

    protected void FlipToPlayer()
    {
        float distanceX = player.transform.position.x - transform.position.x;

        if (Mathf.Abs(distanceX) > 0.1f)
        {
            if ((distanceX < 0 && lookingRight) || (distanceX > 0 && !lookingRight))
            {
                Flip();
            }
        }
    }

    protected void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.y = rotation.y == 0 ? 180 : 0;
        transform.eulerAngles = rotation;
        lookingRight = !lookingRight;
    }

    public virtual void Damage(int _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        anim.SetTrigger("Hit");
        if (!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
            GameObject _bloodSpurtParticle = Instantiate(bloodSpurt, transform.position, quaternion.identity);
            Destroy(_bloodSpurtParticle, 1.5f);
        }
    }

    protected void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") && Time.time > lastAttackTime + attackCooldown)
        {
            if (!MovementPlayer.Instance.pState.invincible)
            {
                attack();
                lastAttackTime = Time.time;
            }
        }
    }

    protected virtual void attack()
    {
        Vector2 knockbackDir = (MovementPlayer.Instance.transform.position - transform.position).normalized;

        MovementPlayer.Instance.takeDamage(
            (int)damage,
            knockbackDir,
            knockbackForce
        );

        MovementPlayer.Instance.HitStopTime(0, 5, 0.5f);
    }
}
