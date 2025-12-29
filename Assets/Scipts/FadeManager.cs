using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    public PlayerStateList pstate;
    private Animator anim;
    private bool isFading = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            anim = GetComponent<Animator>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeToScene(string sceneName, Vector2 exitDir)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScene(sceneName, exitDir));
        }
    }

    private IEnumerator FadeAndSwitchScene(string sceneName, Vector2 exitDir)
    {
        isFading = true;
        anim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.5f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.transitionedfromscene = SceneManager.GetActiveScene().name;
            GameManager.Instance.exitDirection = exitDir;
            GameManager.Instance.up.SetActive(false);
            GameManager.Instance.down.SetActive(false);
        }

        pstate = FindAnyObjectByType<PlayerStateList>();

        if (pstate != null)
        {
            pstate.dashing = false;
            pstate.canMove = true;
            pstate.jumping = false;
        }

        SceneManager.LoadScene(sceneName);

        yield return null;

        anim.SetTrigger("FadeOut");
        isFading = false;
    }


    public void FadeToDoor(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchDoor(sceneName));
        }
    }
    private IEnumerator FadeAndSwitchDoor(string sceneName)
    {
        isFading = true;
        anim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.5f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.transitionedfromscene = SceneManager.GetActiveScene().name;
            GameManager.Instance.up.SetActive(false);
            GameManager.Instance.down.SetActive(false);
        }

        SceneManager.LoadScene(sceneName);

        yield return null;

        anim.SetTrigger("FadeOut");
        isFading = false;
    }

    public void ocasionallyFade()
    {
        if (!isFading)
        {
            StartCoroutine(FadeSomeTime());
        }
    }
    private IEnumerator FadeSomeTime()
    {
        //hace un fade out, espera un segundo y luego hace un fade in
        isFading = true;
        anim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("FadeOut");
        isFading = false;
    }
}
