using UnityEngine;
using UnityEngine.UI;

public class heartController : MonoBehaviour
{
    MovementPlayer player;
    private GameObject[] heartContainers;
    private Image[] heartFills;
    public Transform heartsParent;
    public GameObject heartContainerPrefab;
    public int heartCount;
    

    void Start()
    {
        player = MovementPlayer.Instance;
        heartCount = player.maxHealth/2;
        heartContainers = new GameObject[heartCount];
        heartFills = new Image[heartCount];
        player.OnHeartChangedCallBack += UpdateHeartsHud;
        InstantiateHeartContainer();
        UpdateHeartsHud();
    }

    void InstantiateHeartContainer()
    {
        for (int i = 0; i < heartContainers.Length; i++) 
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);

            RectTransform rectTransform = temp.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(i * (rectTransform.rect.width + 10), 0);
            }

            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
            Debug.Log("Aparece imagen si");
        }
    }

    void SetHeartContainers()
    {
        heartCount = player.maxHealth / 2;
        for (int i = 0; i < heartContainers.Length ; i++)
        {
            if (i < heartCount)
            {
                heartContainers[i].SetActive(true);
                Debug.Log("Aparece true el heartC");
            }
            else
            {
                heartContainers[i].SetActive(false);
                Debug.Log("Situa como false el HeartC"); 
            }
        }
    }

    void SetHeartFill()
    {
        int currentHealth = player.Health_P;
        int maxPossibleHealth = player.maxHealth;
        int damageTaken = maxPossibleHealth - currentHealth;

        for (int i = 0; i < heartFills.Length; i++)
        {
            heartFills[i].fillAmount = 1f;
        }

        for (int i = heartFills.Length - 1; i >= 0; i--)
        {
            int damageToApply = Mathf.Min(damageTaken, 2);
            heartFills[i].fillAmount = (2 - damageToApply) / 2f; 
            damageTaken -= damageToApply;

            if (damageTaken <= 0) break;
        }
    }

    void UpdateHeartsHud()
    {
        SetHeartContainers();
        SetHeartFill();
        Debug.Log("Llama a la funcion si o no0");
    }
}