using UnityEngine;

public class PlayerSavedData : MonoBehaviour
{
    public static PlayerSavedData Instance;

    public bool llaveDave = false;
    public bool ableToDash = false;
    public Vector2 respawnPoint;
    public string respawnScene;
    public int health = 10;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetAll()
    {
        llaveDave = false;
        ableToDash = false;
        respawnPoint = Vector2.zero;
        respawnScene = "";
        health = 10;
    }
}