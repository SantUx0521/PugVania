using UnityEngine;

public class RoyalHability : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Vector2 boxDimensions;
    [SerializeField] private Transform boxPosition;
    [SerializeField] private float time;
    [SerializeField] protected float knockbackForce = 25f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip fire;

    void Start()
    {
        Destroy(gameObject, time);
    }

    public void hit()
    {
        audioSource.PlayOneShot(fire);
        Collider2D[] objects = Physics2D.OverlapBoxAll(boxPosition.position, boxDimensions, 0f);
        foreach (Collider2D colision in objects)
        {
            if (colision.CompareTag("Player"))
            {
                Vector2 knockbackDir = (colision.transform.position - transform.position).normalized;
                colision.GetComponent<MovementPlayer>().takeDamage(damage, knockbackDir, knockbackForce);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxPosition.position, boxDimensions);
    }
}
