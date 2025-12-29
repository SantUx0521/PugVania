using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, animationLength);
    }
}