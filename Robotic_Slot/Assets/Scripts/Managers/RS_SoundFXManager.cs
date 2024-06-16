using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RS_SoundFXManager : MonoBehaviour
{
    public static RS_SoundFXManager Instance;
    [SerializeField] private AudioSource _audioSource;

    private Dictionary<AudioClip, List<AudioSource>> _playingAudioSources = new Dictionary<AudioClip, List<AudioSource>>();

    private void Awake()
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

    public void PlaySound(AudioClip clip, float volume, float playLength = 0f)
    {
        if (clip == null || _audioSource == null) return;

        AudioSource audioSource = Instantiate(_audioSource, transform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        if (!_playingAudioSources.ContainsKey(clip))
        {
            _playingAudioSources[clip] = new List<AudioSource>();
        }
        _playingAudioSources[clip].Add(audioSource);

        float duration = playLength > 0f ? playLength : clip.length;
        StartCoroutine(DestroyAudioSourceAfterDelay(audioSource, duration));
    }

    private IEnumerator DestroyAudioSourceAfterDelay(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (audioSource != null)
        {
            _playingAudioSources[audioSource.clip].Remove(audioSource);
            Destroy(audioSource.gameObject);
        }
    }

    public void StopSound(AudioClip clip)
    {
        if (clip == null || !_playingAudioSources.ContainsKey(clip)) return;

        List<AudioSource> sourcesToStop = _playingAudioSources[clip];
        foreach (AudioSource audioSource in sourcesToStop)
        {
            if (audioSource != null)
            {
                audioSource.Stop();
                Destroy(audioSource.gameObject);
            }
        }
        _playingAudioSources.Remove(clip);
    }

}
