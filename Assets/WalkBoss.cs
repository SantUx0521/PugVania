using UnityEngine;

public class WalkBoss : StateMachineBehaviour
{
    private RoyalGuard boss;
    private Rigidbody2D rb;
    private float cont;
    private float timebystep = 0.5f;
    [SerializeField] private float velocity;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<RoyalGuard>();
        rb = boss.rb;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.lookingPlayer();
        rb.linearVelocity = new Vector2(velocity, rb.linearVelocity.y) * animator.transform.right;
        cont += Time.deltaTime;
        if (cont >= timebystep)
        {
            cont = 0f;
            boss.audioSource.PlayOneShot(boss.steps);
        }
            
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
