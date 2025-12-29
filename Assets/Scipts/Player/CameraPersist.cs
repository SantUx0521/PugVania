using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraPersist : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
