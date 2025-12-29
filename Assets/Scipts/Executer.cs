using UnityEngine;
using UnityEngine.Playables;

public class Executer : MonoBehaviour
{
    public PlayableDirector playableDirector;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            
            playableDirector.Play();
        }
    }
}
