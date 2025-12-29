using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BugFunction : MonoBehaviour, InterfaceEnemy
{
    [Header("Referencias")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] EnemyStates state;
    [SerializeField] protected MovementPlayer player;
    private Animator anim;
    [Header("Movimiento")]
    [SerializeField] private float VelocidadMovBase;
    [SerializeField] private float VelocidadMovActual;
    [SerializeField] private Transform frontController;
    [SerializeField] private float distanceRay;
    [SerializeField] private LayerMask Gmask;
    [SerializeField] private LayerMask Emask;
    [SerializeField] private bool touchingFloor;
    [SerializeField] private bool touchingEnemy;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime = -999f;

    [Header("Estados")]
    [SerializeField] protected float health;
    protected bool isDead = false;
    [SerializeField] protected float damage;
    [SerializeField] private float knockbackForce = 25f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = MovementPlayer.Instance;
    }

    void Update()
    {
        touchingFloor = Physics2D.Raycast(frontController.position, transform.right * -1, distanceRay, Gmask);
        touchingEnemy = Physics2D.Raycast(frontController.position, transform.right * -1, distanceRay, Emask);
        if (health <= 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            StartCoroutine(Death());
        }
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            case EnemyStates.run:
                runBehavior();
                break;
        }
        RaycastHit2D information = Physics2D.Raycast(frontController.position, Vector2.down, distanceRay);
        if (information == false)
        {
            VelocidadMovActual *= -1;
            Flip();
        }

    }

    private void runBehavior()
    {
        rb.linearVelocity = new Vector2(VelocidadMovActual, rb.linearVelocity.y);
        if ((VelocidadMovActual > 0 && !LookingRight()) || (VelocidadMovActual < 0 && LookingRight()))
        {
            Flip();
        }
        if (touchingFloor || touchingEnemy)
        {
            VelocidadMovActual *= -1;
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.y = rotation.y == 0 ? 180 : 0;
        transform.eulerAngles = rotation;
    }

    private bool LookingRight()
    {
        return transform.eulerAngles.y == 180;
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
