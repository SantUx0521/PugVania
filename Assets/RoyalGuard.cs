using System.Collections;
using UnityEngine;

public class RoyalGuard : MonoBehaviour, InterfaceEnemy
{
    private Animator anim;
    public Rigidbody2D rb;
    public Transform player;
    private bool lookingRight;
    [SerializeField] private GameObject defeatButton;
    [SerializeField] TextController textController;
    [SerializeField] Text texto;

    [Header("Combate")]
    [SerializeField] public float health;
    [SerializeField] protected float damage;
    [SerializeField] private int contactDamage;
    [SerializeField] protected float knockbackForce = 25f;
    [SerializeField] private Transform attack;
    [SerializeField] private float attackRadius;
    [SerializeField] private int attackDamage;
    [Header("Audio")]
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip sword;
    [SerializeField] public AudioClip steps;
    [SerializeField] GameObject SoundTrack;
    protected bool isDead;
    private DamageFlash damageFlash;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        damageFlash = GetComponent<DamageFlash>();
    }
    void Update()
    {
        float distancePlayer = Vector2.Distance(transform.position, player.transform.position);
        anim.SetFloat("DistancePlayer", distancePlayer);

        if (health <= 0 && !isDead)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            StartCoroutine(Death());
        }
    }
    public IEnumerator Death()
    {
        anim.SetBool("Defeat", true);
        isDead = true;
        yield return new WaitForSeconds(1.5f);
        SoundTrack.SetActive(false);
        textController.isDesitionBoxActive = true;
        textController.ActiveCartel(texto);
        defeatButton.SetActive(true);
    }

    public void lookingPlayer()
    {
        if ((player.position.x > transform.position.x && !lookingRight) || (player.position.x < transform.position.x && lookingRight))
        {
            lookingRight = !lookingRight;
            transform.eulerAngles = new Vector3(0, lookingRight ? 0 : 180, 0);
        }
    }
    public virtual void Damage(int _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        damageFlash.callDamageFlash();
    }
    protected void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") && !MovementPlayer.Instance.pState.invincible)
        {
            DamagePerTouch();
            MovementPlayer.Instance.HitStopTime(0, 5, 0.5f);
        }
    }
    public void DamagePerTouch()
    {
        Vector2 knockbackDir = (MovementPlayer.Instance.transform.position - transform.position).normalized;

        MovementPlayer.Instance.takeDamage(
            contactDamage,
            knockbackDir,
            knockbackForce
        );

        MovementPlayer.Instance.HitStopTime(0, 5, 0.5f);

    }

    public void Attack()
    {
        audioSource.PlayOneShot(sword);
        Collider2D[] objects = Physics2D.OverlapCircleAll(attack.position, attackRadius);
        foreach (Collider2D colision in objects)
        {
            if (colision.CompareTag("Player"))
            {
                Vector2 knockbackDir = (colision.transform.position - transform.position).normalized;
                colision.GetComponent<MovementPlayer>().takeDamage(attackDamage, knockbackDir, knockbackForce);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attack.position, attackRadius);
    }

}
