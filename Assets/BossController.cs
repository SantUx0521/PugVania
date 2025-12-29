using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject boss;
    public GameObject cinematicBoss;
    void Start()
    {
        cinematicBoss.SetActive(true);
    }

    // Update is called once per frame
    public void ActiveBoss()
    {
        boss.transform.position = cinematicBoss.transform.position;
        boss.SetActive(true);
        cinematicBoss.SetActive(false);
        
    }
}
