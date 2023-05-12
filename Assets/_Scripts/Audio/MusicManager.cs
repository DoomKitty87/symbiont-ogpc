using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private AudioSource _audioSource;
  [Header("Settings")]
  [SerializeField] private int _currentClipIndex;
  [SerializeField] private List<AudioClip> _musicClips;
  [SerializeField] private bool _shuffle;
  [SerializeField] private List<int> _clipIndexesPlayed;

  private void Start() {
    UpdateVolume();
    if (!_shuffle) {
      StartCoroutine(PlayInOrder());
    }
    else {
      StartCoroutine(PlayShuffled());
    }
  }

  private void UpdateVolume() {
    _audioSource.volume = PlayerPrefs.GetFloat("SOUND_VOLUME_MASTER") / 100; 
    _audioSource.volume = _audioSource.volume * (PlayerPrefs.GetFloat("SOUND_VOLUME_MUSIC") / 100);
  }

  private void Update() {
    UpdateVolume();
  }

  private IEnumerator PlayInOrder() {
    foreach (AudioClip track in _musicClips) {
      _audioSource.clip = track;
      _currentClipIndex = _musicClips.IndexOf(track);
      _audioSource.Play();
      while (_audioSource.isPlaying) {
        yield return null;
      }
      _clipIndexesPlayed.Add(_currentClipIndex);
    }
  }

  private IEnumerator PlayShuffled() {
    foreach (AudioClip track in _musicClips) {
      int indexToPlay = Random.Range(0, _musicClips.Count);
      _currentClipIndex = _musicClips.IndexOf(track);
      while (indexToPlay == _currentClipIndex || _clipIndexesPlayed.Contains(indexToPlay)) {
        indexToPlay = Random.Range(0, _musicClips.Count);
      }
      _audioSource.clip = _musicClips[indexToPlay];
      _audioSource.Play();
      while (_audioSource.isPlaying) {
        yield return null;
      }
      _clipIndexesPlayed.Add(_currentClipIndex);
    }
  }
}
