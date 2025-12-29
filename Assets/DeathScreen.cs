using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class deathScreen : MonoBehaviour
{
    [SerializeField] GameObject screen;
    [SerializeField] GameObject botonPrincipal;
    public MovementPlayer player;
    public GameManager gameManager;

    public void deploy()
    {
        Seleccionar(botonPrincipal);
        screen.SetActive(true);
    }
    public void continuar()
    {
        player.isDead = true;
        PlayerSavedData.Instance.health = 10;
        PlayerSavedData.Instance.llaveDave = gameManager.LlaveDave;
        PlayerSavedData.Instance.ableToDash = gameManager.playerState.ableToDash;
        string checkpointScene = PlayerSavedData.Instance.respawnScene;

        if (!string.IsNullOrEmpty(checkpointScene))
        {
            screen.SetActive(false);
            SceneManager.LoadScene(checkpointScene);
        }
        else
        {
            screen.SetActive(false);
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
    }
    public void backToMenu()
    {
        screen.SetActive(false);
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }
    void Seleccionar(GameObject boton)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(boton);
    }
}
