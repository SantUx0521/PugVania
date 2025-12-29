using UnityEngine;
using UnityEngine.Audio;

public class OpcionesMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public void FullScreen(bool FullScreen)
    {
        Screen.fullScreen = FullScreen;
    }

    public void changeVolume(float volume)
    {
        audioMixer.SetFloat("Volumen", volume);
    }
}
