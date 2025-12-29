using UnityEngine;

public class spawn : MonoBehaviour
{
    void Start()
    {
        Transform spawnPoint = GameObject.FindWithTag("Spawnpoint")?.transform;
        if (spawnPoint != null && MovementPlayer.Instance != null)
        {
            MovementPlayer.Instance.transform.position = spawnPoint.position;
        }
    }
}
