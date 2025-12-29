using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorFunction : MonoBehaviour
{
    [SerializeField] private string TransitionTo;
    private bool canEnter = false;
    void Update()
    {
        if (canEnter)
        {
            GameManager.Instance.up.SetActive(true);
        }
        else
        {
            GameManager.Instance.up.SetActive(false);
        }

        if (canEnter && Input.GetAxisRaw("Vertical") > 0)
        {
            GameManager.Instance.transitionedfromscene = SceneManager.GetActiveScene().name;
            FadeManager.Instance.FadeToDoor(TransitionTo);
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
