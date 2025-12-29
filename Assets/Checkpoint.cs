using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public GameManager gameManager;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    void OnTriggerEnter2D(Collider2D _other)
    {
        gameManager = FindAnyObjectByType<GameManager>();
        if (_other.CompareTag("Player"))
        {
            PlayerSavedData.Instance.llaveDave = gameManager.LlaveDave;
            PlayerSavedData.Instance.ableToDash = gameManager.playerState.ableToDash;
            PlayerSavedData.Instance.respawnPoint = transform.position;
            PlayerSavedData.Instance.respawnScene = SceneManager.GetActiveScene().name;
        }
    }
}
