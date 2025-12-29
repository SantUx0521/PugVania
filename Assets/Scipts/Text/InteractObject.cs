using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public Text textData;
    public TextController textController;
    private bool canEnter = false;

    private void Update()
    {
        if (canEnter)
        {
            GameManager.Instance.down.SetActive(true);
        }
        else
        {
            GameManager.Instance.down.SetActive(false);
        }
        
        if (canEnter && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("Vertical") < 0) && textController.isDialogueActive == false)
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
        {
            canEnter = false;
        }
    }
    
}
