using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource attackSource;
    public AudioClip pasos;
    public AudioClip atack;
    public AudioClip fallin;
    public AudioClip dash;

    public void playPasos()
    {
        audioSource.PlayOneShot(pasos);
    }

    public void playAtack()
    {
        attackSource.PlayOneShot(atack);
    }

    public void playDash()
    {
        audioSource.PlayOneShot(dash);
    }

    public void playfallin()
    {
        audioSource.PlayOneShot(fallin);
    }
}
