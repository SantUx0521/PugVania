using UnityEngine;
using UnityEngine.EventSystems;
public class RoyalGuardDesition : MonoBehaviour
{
    [SerializeField] private GameObject royalGuard;
    [SerializeField] private GameObject TextBox;
    [SerializeField] private GameObject Buttons;
    [SerializeField] GameObject botonPrincipal;
    [SerializeField] GameObject cinematic;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip kill;
    BossesCount bossCount;

    private void Awake()
    {
        Seleccionar(botonPrincipal);
    }
    public void killGuard()
    {
        //mata al guardia, lo asigna en una variable del jugador, y destruye el objeto, además dentro de un archivo del jugador se guarda que el guardia ha sido eliminado para utilizarlo luego en otras escenas
        bossCount = GameObject.FindGameObjectWithTag("Player").GetComponent<BossesCount>();
        bossCount.killGuard();
        //fade out para que no se vea el guardia al morir.
        FadeManager.Instance.ocasionallyFade();
        audioSource.PlayOneShot(kill);
        Destroy(royalGuard);
        TextBox.SetActive(false);
        Buttons.SetActive(false);
        MovementPlayer.Instance.pState.canMove = true;
    }

    public void forgiveGuard()
    {
        //perdonamos al guardia, se le asigna esta variable al jugador, el guardia se va con una animacion y luego se destruye el objeto, se guarda en el archivo del jugador que el guardia ha sido perdonado
        bossCount = GameObject.FindGameObjectWithTag("Player").GetComponent<BossesCount>();
        bossCount.RoyalGuardAlive = true;
        bossCount.rute = BossesCount.Ruta.Buena;
        TextBox.SetActive(false);
        Buttons.SetActive(false);
        FadeManager.Instance.ocasionallyFade();
        MovementPlayer.Instance.pState.canMove = true;
        //realizar animacion de irse
        cinematic.SetActive(true);
        Destroy(gameObject);
        
    }
    void Seleccionar(GameObject boton)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(boton);
    }
}
