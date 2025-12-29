using UnityEngine;
using UnityEngine.SceneManagement;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private string TransitionTo;
    private bool canEnter = false;
    private GameManager gameManager;
    Animator anim;
    public Text textData;
    public TextController textController;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        anim = GetComponent<Animator>();
    }
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
        if (canEnter && Input.GetAxisRaw("Vertical") > 0 && gameManager.LlaveDave == true)
        {
            anim.SetTrigger("Unlocked");
            GameManager.Instance.transitionedfromscene = SceneManager.GetActiveScene().name;
            FadeManager.Instance.FadeToDoor(TransitionTo);
        }
        else if (canEnter && (Input.GetAxisRaw("Vertical") > 0) && textController.isDialogueActive == false && gameManager.LlaveDave == false)
        {
            FindAnyObjectByType<TextController>().ActiveCartel(textData);
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
