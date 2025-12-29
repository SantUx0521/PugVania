using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointFunction : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name == PlayerSavedData.Instance.respawnScene)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                player.transform.position = PlayerSavedData.Instance.respawnPoint;

                GameManager.Instance.Heal(10); 
                GameManager.Instance.LlaveDave = PlayerSavedData.Instance.llaveDave;
                GameManager.Instance.playerState.ableToDash = PlayerSavedData.Instance.ableToDash;
                player.GetComponent<MovementPlayer>().resetPlayer();
            }
        }
    }
}
