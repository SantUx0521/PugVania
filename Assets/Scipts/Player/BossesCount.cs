using UnityEngine;

public class BossesCount : MonoBehaviour
{
    [SerializeField] public int killCount;
    [SerializeField] public bool RoyalGuardAlive; 
    [SerializeField] public Ruta rute;
    public enum Ruta
    {
        Neutral,
        Malvada,
        Buena
    }

    public void killGuard()
    {
        //se asigna al guardia real como muerto, se incrementa el contador en 1, teniendo esto en cuenta de cara al final de juego. ruta = malvada
        RoyalGuardAlive = false;
        rute = Ruta.Malvada;
        killCount += 1;
    }
}
