using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class FallinPlatform : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rock;
    private Rigidbody2D rb;
    [SerializeField] float respawnTime = 5f;
    [SerializeField] private float velocity;
    private Animator anim;
    private Vector3 initialPosition;
    private Collider2D platformCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        initialPosition = transform.position;
        platformCollider = GetComponent<Collider2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.CompareTag("Player"))
        {
            Rock();
            anim.SetTrigger("Playercito");
            StartCoroutine(Fallen(_other));
        }
    }

    private IEnumerator Fallen(Collision2D _other)
    {
        yield return new WaitForSeconds(time);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Physics2D.IgnoreCollision(platformCollider, _other.collider, true);
        platformCollider.enabled = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(new Vector2(0.1f, 0));

        yield return new WaitForSeconds(respawnTime);
        ResetPlatform();
        Physics2D.IgnoreCollision(platformCollider, _other.collider, false);
    }

    private void ResetPlatform()
    {
        anim.SetTrigger("Respawn");
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = initialPosition;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        platformCollider.enabled = true;
    }
    private void Rock()
    {
        audioSource.PlayOneShot(rock);
    }

}
