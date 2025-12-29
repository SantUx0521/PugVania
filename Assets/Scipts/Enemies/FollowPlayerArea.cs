using UnityEngine;

public class FollowPlayerArea : MonoBehaviour
{
    public float radius;
    public LayerMask playerMask;
    public Transform transformPlayer;
    public states actualState;
    public float velocity;
    public float maxDistance;
    public Vector3 startPoint;
    public bool lookingRight;

    public enum states
    {
        waiting,
        following,
        goingBack,

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
            case states.goingBack:
                comeBack();
                break;
            
        }

    }

    private void waitin()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radius, playerMask);
        if (playerCollider)
        {
            transformPlayer = playerCollider.transform;

            actualState = states.following;
        }
    }

    private void follow()
    {
        if (transformPlayer == null)
        {
            actualState = states.goingBack;
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, transformPlayer.position, velocity * Time.deltaTime);

        FlipToObj(transformPlayer.position);

        if (Vector2.Distance(transform.position, startPoint) > maxDistance || Vector2.Distance(transform.position, transformPlayer.position) > maxDistance)
        {
            actualState = states.goingBack;
            transformPlayer = null;
        }

    }

    private void comeBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, startPoint, velocity * Time.deltaTime);
        FlipToObj(startPoint);
        if (Vector2.Distance(transform.position, startPoint) < 0.1f)
        {
            actualState = states.waiting;
        }
    }
    private void FlipToObj(Vector3 objective)
    {
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
}
