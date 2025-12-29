using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menú : MonoBehaviour
{
    public SaveFile playerSave;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject Gameplay;
    [SerializeField] GameObject player;
    [SerializeField] GameObject PlayerData;
    [SerializeField] GameObject Main;
    [SerializeField] GameObject Opciones;
    [SerializeField] GameObject Creditos;
    [SerializeField] GameObject botonPrincipal;
    [SerializeField] GameObject botonOpciones;
    [SerializeField] GameObject botonCreditos;
    void Awake()
    {
        Gameplay.SetActive(true);
        PlayerData.SetActive(true);
        Gameplay.SetActive(false);
        PlayerData.SetActive(false);
        Seleccionar(botonPrincipal);
    }
    public void play()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FadeManager.Instance.FadeToScene("ZonaInicial", Vector2.zero);
    }

    public void continueGame()
    {
        gameManager.load();
        SceneManager.sceneLoaded += OnSceneLoaded;
        FadeManager.Instance.FadeToScene(gameManager.actualScene, Vector2.zero);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Gameplay.SetActive(true);
        PlayerData.SetActive(true);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void config()
    {
        Main.SetActive(false);
        Opciones.SetActive(true);
        Seleccionar(botonOpciones);
    }

    public void credits()
    {
        Main.SetActive(false);
        Creditos.SetActive(true);
        Seleccionar(botonCreditos);
    }

    public void back()
    {
        if (Opciones == true)
        {
            Opciones.SetActive(false);
            Main.SetActive(true);
            Seleccionar(botonPrincipal);
        }
    }

    public void backToCredits()
    {
        if (Creditos == true)
        {
            Creditos.SetActive(false);
            Main.SetActive(true);
            Seleccionar(botonPrincipal);
        }
    }

    public void exit()
    {
        Application.Quit();
    }
    void Seleccionar(GameObject boton)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(boton);
    }
}
