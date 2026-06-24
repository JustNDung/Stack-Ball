using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource _audioSource;
    public bool sound = true;

    private void Awake()
    {
        MakeSingleton();
        _audioSource = GetComponent<AudioSource>();
    }

    private void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SoundOnOff()
    {
        sound = !sound;
    }

    public void PlaySoundFX(AudioClip clip, float volume)
    {
        if (sound)
        {
            _audioSource.PlayOneShot(clip, volume);
        }
    }
}