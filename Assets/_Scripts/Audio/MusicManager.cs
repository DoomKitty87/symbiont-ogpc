using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicManager : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private AudioSource _audioSource;
  [Header("Settings")]
  [SerializeField] private int _currentClipIndex;
  [SerializeField] private List<AudioClip> _musicClips;
  [SerializeField] private bool _shuffle;
  [SerializeField] private List<int> _clipIndexesPlayed;
  [SerializeField] private GameObject _nowPlayingPopup;
  [SerializeField] private GameObject _mainCanvas;

  private bool _playing;

  private void Start() {
    UpdateVolume();
    StartPlaying();
  }

  private void UpdateVolume() {
    _audioSource.volume = PlayerPrefs.GetFloat("SOUND_VOLUME_MASTER") / 100; 
    _audioSource.volume = _audioSource.volume * (PlayerPrefs.GetFloat("SOUND_VOLUME_MUSIC") / 100);
  }

  private void UpdateShuffle() {
    _shuffle = PlayerPrefs.GetInt("SOUND_SHUFFLE_SONGS") == 1;
  }

  private void Update() {
    UpdateVolume();
    UpdateShuffle();
    if (!_playing) StartPlaying();
  }

  private void StartPlaying() {
    if (!_shuffle) {
      StartCoroutine(PlayInOrder());
    }
    else {
      StartCoroutine(PlayShuffled());
    }
  }

  private IEnumerator PlayInOrder() {
    _playing = true;
    foreach (AudioClip track in _musicClips) {
      _audioSource.clip = track;
      _currentClipIndex = _musicClips.IndexOf(track);
      _audioSource.Play();
      StartCoroutine(NewSongPlaying(_audioSource.clip));
      yield return new WaitForSeconds(_audioSource.clip.length);
      Debug.Log("MusicManager: Song ended.");
      _clipIndexesPlayed.Add(_currentClipIndex);
    }
    _playing = false;
  }

  private IEnumerator PlayShuffled() {
    _playing = true;
    List<int> played = new List<int>();
    for (int i = 0; i < _musicClips.Count; i++) {
      int indexToPlay = Random.Range(0, _musicClips.Count);
      if (played.Contains(indexToPlay)) {
        if (played.Count >= _musicClips.Count) {
          played.Clear();
        }
        else {
          while (played.Contains(indexToPlay)) indexToPlay = Random.Range(0, _musicClips.Count);
        }
      }
      played.Add(indexToPlay);
      _audioSource.clip = _musicClips[indexToPlay];
      _audioSource.Play();
      StartCoroutine(NewSongPlaying(_audioSource.clip));
      yield return new WaitForSeconds(_audioSource.clip.length);
      Debug.Log("MusicManager: Song ended.");
    }
    _playing = false;
  }

  private IEnumerator NewSongPlaying(AudioClip clip) {
    GameObject tmp = Instantiate(_nowPlayingPopup, Vector3.zero, Quaternion.identity, _mainCanvas.transform);
    Transform infoHolder = tmp.transform.GetChild(0);
    infoHolder.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = clip.name + " | " + clip.length.ToString();
    float timeElapsed = 0;
    while (timeElapsed < 1) {
      infoHolder.transform.position = new Vector3(Mathf.Lerp(Screen.width + (infoHolder.gameObject.GetComponent<RectTransform>().rect.x / 2), Screen.width - (infoHolder.gameObject.GetComponent<RectTransform>().rect.x / 2), timeElapsed), 0, 0);
      timeElapsed += Time.deltaTime;
    }
    yield return new WaitForSeconds(1.5f);
    timeElapsed = 0;
    while (timeElapsed < 1) {
      infoHolder.transform.position = new Vector3(Mathf.Lerp(Screen.width + (infoHolder.gameObject.GetComponent<RectTransform>().rect.x / 2), Screen.width - (infoHolder.gameObject.GetComponent<RectTransform>().rect.x / 2), 1 - timeElapsed), 0, 0);
      timeElapsed += Time.deltaTime;
    }
    Destroy(tmp);
  }

  // private IEnumerator PlayShuffled() {
  //   foreach (AudioClip track in _musicClips) {
  //     int indexToPlay = Random.Range(0, _musicClips.Count);
  //     _currentClipIndex = _musicClips.IndexOf(track);
  //     while (indexToPlay == _currentClipIndex || _clipIndexesPlayed.Contains(indexToPlay)) {
  //       indexToPlay = Random.Range(0, _musicClips.Count);
  //     }
  //     _audioSource.clip = _musicClips[indexToPlay];
  //     _audioSource.Play();
  //     while (_audioSource.isPlaying) {
  //       yield return null;
  //     }
  //     _clipIndexesPlayed.Add(_currentClipIndex);
  //   }
  // }
}
