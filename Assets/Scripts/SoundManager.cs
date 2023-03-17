using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource _musicSource, _effectsSource,_effectsSource2, _musicSourceThemeMusic;

    private bool _isMusicOn  = true;

    void  Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }

    public void PlayLoudSound(AudioClip clip)
    {
        _effectsSource2.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        if (_isMusicOn)
        {
            _musicSourceThemeMusic.Stop();
            _isMusicOn = false;
        }
        else
        {
            _musicSourceThemeMusic.Play();
            _isMusicOn = true;
        }
    }

}
