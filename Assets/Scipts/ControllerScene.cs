using UnityEngine;

public class ControllerScene : MonoBehaviour
{
    public GameObject Player;
    public GameObject CinematicPlayer;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
            Player.SetActive(false);

        CinematicPlayer.SetActive(true);
    }

    public void DesactivarJugador()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
            Player.SetActive(false);

        CinematicPlayer.SetActive(true);
    }

    public void ActivarJugador()
    {
        Player.transform.position = CinematicPlayer.transform.position;
        MovementPlayer.Instance.pState.canMove = true;
        Player.SetActive(true);
        CinematicPlayer.SetActive(false);
    }
}
