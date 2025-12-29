using UnityEngine;

public class DaveKey : MonoBehaviour
{
    GameManager gameManager;
    public bool canEnter;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    void Update()
    {
        if (canEnter && Input.GetAxisRaw("Vertical") < 0 && gameManager.LlaveDave == false)
        {
            gameManager.LlaveDave = true;
            Debug.Log("Llave Obtenida");
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
