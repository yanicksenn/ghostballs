using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----------Audio Source-------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [Header("----------Audio Clip-------------")]
    public AudioClip background;
    public AudioClip button;
    public AudioClip arrow;
    public AudioClip falling;
    public AudioClip succeed;
    public AudioClip pocessing;

    private void Start()
    {
        musicSource.clip=background;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip){
        SFXSource.PlayOneShot(clip);
    }
}
