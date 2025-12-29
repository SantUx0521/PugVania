using System.Collections;
using UnityEngine;

public class FollowGroundPlayer : MonoBehaviour, InterfaceEnemy
{
    public float radius;
    public LayerMask playerMask;
    public Transform transformPlayer;
    public states actualState;
    public float velocity;
    public float maxDistance;
    public Vector3 startPoint;
    public bool lookingRight;
    public Rigidbody2D rb;
    public Animator anim;
    private bool coroutineStarted = false;

    [Header("Combate")]
    [SerializeField] protected MovementPlayer player;
    [SerializeField] protected float health;
    protected bool isDead = false;
    [SerializeField] protected float damage;
    [SerializeField] private float knockbackForce = 25f;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime = -999f;

    public enum states
    {
        waiting,
        following,
        goingBack,
        alert,

    }
    private void Awake()
    {
        player = MovementPlayer.Instance;
    }
    private void Start()
    {
        startPoint = transform.position;
    }

    void Update()
    {
        switch (actualState)
        {
            case states.waiting:
                waitin();
                break;
            case states.following:
                follow();
                break;
            case states.alert:
                alert();
                break;
            case states.goingBack:
                comeBack();
                break;

        }
        if (health <= 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            StartCoroutine(Death());
        }
    }

    private void waitin()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radius, playerMask);
        if (playerCollider)
        {
            transformPlayer = playerCollider.transform;

            actualState = states.alert;
        }
    }

    private void follow()
    {
        anim.SetBool("Run", true);
        if (transformPlayer == null)
        {
            actualState = states.goingBack;
            return;
        }

        if (transform.position.x < transformPlayer.position.x)
        {
            rb.linearVelocity = new Vector2(velocity, rb.linearVelocityY);
        }
        else
        {
            rb.linearVelocity = new Vector2(-velocity, rb.linearVelocityY);
        }

        FlipToObj(transformPlayer.position);

        if (Vector2.Distance(transform.position, startPoint) > maxDistance || Vector2.Distance(transform.position, transformPlayer.position) > maxDistance)
        {
            actualState = states.goingBack;
            transformPlayer = null;
        }

    }

    private void alert()
    {
        if (!coroutineStarted)
        {
            coroutineStarted = true;
            StartCoroutine(alertita());
        }
    }
    private IEnumerator alertita()
    {
        anim.SetTrigger("Found");
        yield return new WaitForSeconds(0.2f);
        actualState = states.following;
        coroutineStarted = false;
    }

    private void comeBack()
    {
        if (transform.position.x < startPoint.x)
        {
            rb.linearVelocity = new Vector2(velocity, rb.linearVelocityY);
        }
        else
        {
            rb.linearVelocity = new Vector2(-velocity, rb.linearVelocityY);
        }

        FlipToObj(startPoint);
        if (Vector2.Distance(transform.position, startPoint) < 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("Run", false);
            actualState = states.waiting;
        }
    }
    private void FlipToObj(Vector3 objective)
    {
        float dif = objective.x - transform.position.x;
        if (Mathf.Abs(dif) < 0.1f)
        {
            return; 
        }
        if (objective.x > transform.position.x && !lookingRight)
        {
            flip();
        }
        else if (objective.x < transform.position.x && lookingRight)
        {
            flip();
        }
    }

    private void flip()
    {
        lookingRight = !lookingRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(startPoint, maxDistance);
    }
    public IEnumerator Death()
    {
        anim.SetTrigger("Death");
        isDead = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    public virtual void Damage(int _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        anim.SetTrigger("Hit");
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
