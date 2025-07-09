using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_MUSIC_SOUND_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set; }
    [SerializeField] private AudioSource audioSource;
    private float volume = .3f;
    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_MUSIC_SOUND_VOLUME, .3f);
        audioSource.volume = volume;
    }
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1)
        {
            volume = 0;
        }
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_MUSIC_SOUND_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolumeMusic()
    {
        return volume;
    }
}
