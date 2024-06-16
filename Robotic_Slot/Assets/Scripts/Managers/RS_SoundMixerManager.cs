using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RS_SoundMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] GameObject _audio;
    private bool mute = false;
    private float volume;
    public void SetMasterVolumer(float level)
    {
        volume = level;
        if (!mute)
        {
            _audioMixer.SetFloat("MasterVolume", Mathf.Log10(level)*20);
        }
    }

    public void toggleAudio()
    {
        if (!mute)
        {
            mute = true;
            _audioMixer.SetFloat("MasterVolume", Mathf.Log10(-80));
            _audio.SetActive(false);
        }
        else
        {
            mute = false;
            _audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
            _audio.SetActive(true);
        }
    }
}
