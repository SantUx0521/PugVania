
[System.Serializable]
public class SaveFile
{
    public int health;
    public bool llaveDave;
    public bool ableToDash;
    public bool RoyalGuardAlive;
    public int killCount;
    public string ruta;
    public string actualScene;
    public float[] position = new float[3];

    public SaveFile(GameManager gameManager)
    {
        actualScene = gameManager.actualScene;
        health = gameManager._currentHealth;
        llaveDave = gameManager.LlaveDave;
        ruta = gameManager.bossCount.rute.ToString();
        ableToDash = gameManager.playerState.ableToDash;
        position[0] = gameManager.player.transform.position.x;
        position[1] = gameManager.player.transform.position.y;

        RoyalGuardAlive = gameManager.bossCount.RoyalGuardAlive;
        killCount = gameManager.bossCount.killCount;
    }
}
