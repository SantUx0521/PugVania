using UnityEngine;

public class staticObjectAnimator : MonoBehaviour
{
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        anim.SetBool("Animation", true);
    }
}
