using UnityEngine;

public class PawItem : MonoBehaviour
{
    public PlayerStateList pState;
    public TextController textController;
    public Text textData;

    void Awake()
    {
        pState = FindAnyObjectByType<PlayerStateList>();
        if (pState.ableToDash == true)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            FindAnyObjectByType<TextController>().ActiveCartel(textData);
            pState.ableToDash = true;
            Destroy(gameObject);
        }
    }
}
