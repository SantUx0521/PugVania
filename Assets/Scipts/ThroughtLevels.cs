using UnityEngine;
using UnityEngine.SceneManagement;

public class ThroughtLevels : MonoBehaviour
{
    [SerializeField] private Transform RightSpawn;
    [SerializeField] private Transform LeftSpawn;
    [SerializeField] private Transform UpSpawn;
    [SerializeField] private Transform DownSpawn;

    void Start()
    {
        Vector2 direction = GameManager.Instance.exitDirection;
        Transform spawnPoint = null;

        if (direction.x > 0.5f) spawnPoint = LeftSpawn; 
        else if (direction.x < -0.5f) spawnPoint = RightSpawn; 
        else if (direction.y > 0.5f) spawnPoint = DownSpawn; 
        else if (direction.y < -0.5f) spawnPoint = UpSpawn; 

        if (spawnPoint != null)
            MovementPlayer.Instance.transform.position = spawnPoint.position;
    }
}

