using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //si alguien ve esto, solo soy una persona, la inconsistencia en los nombres entre español e ingles vienen porque a veces me olvido de hacerlo todo en ingles y lo acabo mezclando XD
    [SerializeField] GameObject menu;
    [SerializeField] GameObject Opciones; //por ejemplo, además en este caso copio y pego lo que ya esta hecho para ahorrar tiempo
    [SerializeField] GameObject botonOpciones;
    [SerializeField] GameObject botonPrincipal;
    [SerializeField] GameObject Gameplay;
    [SerializeField] PlayerStateList player;
    [SerializeField] GameObject PlayerData;
    private bool pausedGame;

    void Awake()
    {
        Seleccionar(botonPrincipal);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausedGame == true)
            {
                resume();
                pausedGame = false;
            }
            else
            {
                Seleccionar(botonPrincipal);
                Time.timeScale = 0f;
                menu.SetActive(true);
                pausedGame = true;
            }
        }

        if (pausedGame == true)
        {
            player.canMove = false;
            player.invincible = true;
        }
    }
    void Seleccionar(GameObject boton)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(boton);
    }
    public void resume()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        Opciones.SetActive(false);
        player.canMove = true;
        player.invincible = false;
        pausedGame = false;
    }

    public void options()
    {
        menu.SetActive(false);
        Opciones.SetActive(true);
        Seleccionar(botonOpciones);
    }

    public void goBack()
    {
        Opciones.SetActive(false);
        menu.SetActive(true);
        Seleccionar(botonPrincipal);
    }

    public void exit()
    {
        Destroy(Gameplay);
        Destroy(PlayerData);
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleMenu");
    }

}
