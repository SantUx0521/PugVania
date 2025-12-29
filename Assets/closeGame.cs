using UnityEngine;
using UnityEngine.SceneManagement;

public class closeGame : MonoBehaviour
{
    public void closingGame()
    {

        FadeManager.Instance.FadeToScene("TitleMenu", Vector2.zero);
    }
}
