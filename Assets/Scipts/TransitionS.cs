using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionS : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private string TransitionTo;
    [SerializeField] private Vector2 exitdirection;

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            GameManager.Instance.transitionedfromscene = SceneManager.GetActiveScene().name;
            GameManager.Instance.exitDirection = exitdirection;
            FadeManager.Instance.FadeToScene(TransitionTo, exitdirection);
        }
    }
}
