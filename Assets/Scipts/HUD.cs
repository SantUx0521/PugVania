using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    [System.Serializable]
    public struct HeartUI
    {
        public Image heartImage;    
        public Sprite fullSprite;   
        public Sprite halfSprite;  
        public Sprite emptySprite;  
    }
    public HeartUI[] hearts;
    public static HUD Instance;

    void Awake()
    {
           if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            int heartValue = currentHealth - (i * 2); 

            if (heartValue >= 2)
                hearts[i].heartImage.sprite = hearts[i].fullSprite;
            else if (heartValue == 1)
                hearts[i].heartImage.sprite = hearts[i].halfSprite;
            else
                hearts[i].heartImage.sprite = hearts[i].emptySprite;
        }
    }
}
