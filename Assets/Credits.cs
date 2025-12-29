using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    GameObject gameplay;
    public void goCredits()
    {
        gameplay = GameObject.FindGameObjectWithTag("Gameplay");
        GameManager.Instance.transitionedfromscene = SceneManager.GetActiveScene().name;
        Destroy(gameplay);
        FadeManager.Instance.FadeToScene("Credits", Vector2.zero);
    }
}
