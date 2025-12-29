using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private float slimeGravity = 12f;
    [SerializeField] private float slimeSpeed = 5f;
    [SerializeField] private float slimePatrolSpeed = 2f;
    [SerializeField] private float slimeChaseRange = 10f;

    protected override void Start()
    {
        base.Start();
        rb.gravityScale = slimeGravity;
        speed = slimeSpeed;
        patrolSpeed = slimePatrolSpeed;
        chaseRange = slimeChaseRange;
    }
}
