using System.Collections;
using UnityEngine;

public class BedFunction : MonoBehaviour
{
    private GameManager gameManager;
    private bool canSleep = false;
    public bool isSaving = false;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    void Update()
    {
        if (canSleep)
        {
            GameManager.Instance.up.SetActive(true);
        }
        else
        {
            GameManager.Instance.up.SetActive(false);
        }
        if (canSleep && Input.GetAxisRaw("Vertical") > 0 && !isSaving)
        {
            StartCoroutine(SleepCoroutine());
        }
    }

    public IEnumerator SleepCoroutine()
    {
        isSaving = true;
        gameManager.player.anim.SetTrigger("sitting");
        MovementPlayer.Instance.pState.canMove = false;
        FadeManager.Instance.ocasionallyFade();
        gameManager.Heal(GameManager.MAX_HEALTH);
        gameManager.hud.UpdateHearts(gameManager._currentHealth);
        gameManager.save();
        yield return new WaitForSeconds(0.5f);
        MovementPlayer.Instance.pState.canMove = true;
        isSaving = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canSleep = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canSleep = false;
        }
    }
}
