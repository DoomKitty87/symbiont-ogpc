using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MusicTrack {
  public string _trackName;
  public string _trackArtist;
  public AudioClip _trackAudioClip;
}

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private AudioSource _audioSource;

  [Header("Settings")]
  [SerializeField] private List<MusicTrack> _allTracks;
  [SerializeField] private bool _shuffleTracks;

  [Header("Debug")]
  [SerializeField] private List<MusicTrack> _trackQueue = new List<MusicTrack>();
  [SerializeField] private int _currentQueueIndex = 0;
  [Header("Bool")]
  public bool _isPaused;
  
  public MusicTrack _currentTrack {
    get {
      if (_trackQueue.Count > 0) return _trackQueue[_currentQueueIndex]; else return null;
    }
  }
  public float _songCurrentTime { 
    get {
      if (_audioSource.isPlaying) return _audioSource.time; else return -1;
    }
  }

  public float _songTotalTime { 
    get {
      if (_audioSource.isPlaying) return _audioSource.clip.length; else return -1;
    }
  }


  // Control Functions

  // the AudioSource should always already be playing before isPaused is set to false,
  // or else the song will be skipped to the next one in the queue

  public void PlayPause(bool pause) {
    if (pause) {
      Pause();
    }
    else {
      Play();
    }
  }

  public void Play() {
    if (_isPaused) {
      _audioSource.UnPause();
      _isPaused = false;
    }
    else {
      StartCoroutine(PlayTrackQueueUntilCompleted());
    }
  }

  public void Pause() {
    _isPaused = true;
    _audioSource.Pause();
  }

  public void Skip() {
    StopCoroutine(PlayTrackQueueUntilCompleted());
    _audioSource.Stop();
    _currentQueueIndex++;
    if (_currentQueueIndex >= _trackQueue.Count) _currentQueueIndex = 0;
    StartCoroutine(PlayTrackQueueUntilCompleted());
  }

  public void Previous() {
    StopCoroutine(PlayTrackQueueUntilCompleted());
    _audioSource.Stop();
    _currentQueueIndex--;
    if (_currentQueueIndex < 0) _currentQueueIndex = _trackQueue.Count - 1;
    StartCoroutine(PlayTrackQueueUntilCompleted());
  }



  // ------------------------------

  private void Start() {
    UpdateVolume();
    if (_shuffleTracks) {
      AddTracksToQueueAtRandom();
    }
    else {
      for (int i = 0; i < _allTracks.Count; i++) {
        _trackQueue.Add(_allTracks[i]);
      }
    }
    _currentQueueIndex = 0;
    Play();
  }
  private void AddTracksToQueueAtRandom() {
    List<MusicTrack> _tracksToAddToQueue = new List<MusicTrack>();
    for (int i = 0; i < _allTracks.Count; i++) {
      _tracksToAddToQueue.Add(_allTracks[i]);
    }
    while (_tracksToAddToQueue.Count > 0) {
      int randomIndex = Random.Range(0, _tracksToAddToQueue.Count);
      _trackQueue.Add(_tracksToAddToQueue[randomIndex]);
      _tracksToAddToQueue.RemoveAt(randomIndex);
    }
  }

  private void UpdateVolume() {
    _audioSource.volume = PlayerPrefs.GetFloat("SOUND_VOLUME_MASTER") / 100; 
    _audioSource.volume = _audioSource.volume * (PlayerPrefs.GetFloat("SOUND_VOLUME_MUSIC") / 100);
  }

  private void UpdateShuffle() {
    _shuffleTracks = PlayerPrefs.GetInt("SOUND_SHUFFLE_SONGS") == 1;
  }

  private void Update() {
    UpdateVolume();
    bool shuffleOld = _shuffleTracks;
    UpdateShuffle();
    if (shuffleOld != _shuffleTracks) {
      _trackQueue.Clear();
      if (_shuffleTracks) {
      AddTracksToQueueAtRandom();
      }
      else {
        for (int i = 0; i < _allTracks.Count; i++) {
          _trackQueue.Add(_allTracks[i]);
        }
      }
    }
  }

  private void ClearQueue() {
    _trackQueue.Clear();
  }

  private IEnumerator PlayTrackQueueUntilCompleted() {
    if (_trackQueue.Count == 0) {
      Debug.LogWarning("MusicManager: Track Queue is empty!");
      yield break;
    }
    while (_currentQueueIndex < _trackQueue.Count) {
      PlayTrack(_trackQueue[_currentQueueIndex]);
      while (_audioSource.isPlaying || (_audioSource.isPlaying == false && Application.isFocused == false) || _isPaused ) {
        yield return null;
      }
      _currentQueueIndex++;
      if (_currentQueueIndex >= _trackQueue.Count) _currentQueueIndex = 0;
    }
  }

  private void PlayTrack(MusicTrack track) {
    StartCoroutine(PlayTrackCoroutine(track));
  }
  private IEnumerator PlayTrackCoroutine(MusicTrack track) {
    _audioSource.clip = track._trackAudioClip;
    _audioSource.Play();
    if (SceneManager.GetActiveScene().name == "Main Menu") GetComponent<MusicPopupManager>().UpdateSongInfo();
    yield return new WaitForSeconds(track._trackAudioClip.length);
  }
}
