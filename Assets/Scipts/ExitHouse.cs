using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitHouse : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("ZonaInicial");
        }
    }
}
