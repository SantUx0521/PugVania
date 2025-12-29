using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondDoorFunction : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private string TransitionTo;
    [SerializeField] private Vector2 exitdirection;
    public bool canEnter;

    void Update()
    {
        if (canEnter && Input.GetAxisRaw("Vertical") > 0)
        {
            GameManager.Instance.transitionedfromscene = SceneManager.GetActiveScene().name;
            GameManager.Instance.exitDirection = exitdirection;
            FadeManager.Instance.FadeToScene(TransitionTo, exitdirection);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canEnter = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canEnter = false;
    }
}
