using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip palmTreeAudio, antiPestAudio, honkAudio;
    public AudioClip perfect, missed;
    [SerializeField] private AudioSource audioSources, BGMSource;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        BGMSource.loop = true;
        BGMSource.Play();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            PlayAudioClip(antiPestAudio);
    }

    public void PlayAudioClip(AudioClip clipAudio)
    {
        audioSources.PlayOneShot(clipAudio);
    }

    public void PlayHonkAudio()
    {
        PlayAudioClip(honkAudio);
    }

    public void PlayMissedAudio() { PlayAudioClip(missed); }

    public void PlayPerfectAudio() { PlayAudioClip(perfect); }
}
