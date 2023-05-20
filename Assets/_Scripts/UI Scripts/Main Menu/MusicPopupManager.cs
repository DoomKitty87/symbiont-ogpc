using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(MusicManager))]
public class MusicPopupManager : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private MusicManager _musicManager;
  [Header("Image References")]
  [SerializeField] private Image _songCompletionFillImage;
  [Header("Text References")]
  [SerializeField] private TextMeshProUGUI _songNameText, _songArtistText, _songCurrentTimeText, _songTotalTimeText;

  private void Start() {
    if (_musicManager == null) {
      _musicManager = gameObject.GetComponent<MusicManager>();
    }
    _songCompletionFillImage.fillAmount = 0;
  }

  private void Update() {
    UpdateSongCompletionFill();
    UpdateSongCurrentTimeText();
    UpdateSongTotalTimeText();
  }

  public void UpdateSongInfo() {
    UpdateSongNameText();
    UpdateSongArtistText();
  }

  private void UpdateSongCompletionFill() {
    if (_musicManager._isPaused) return;
    if (_musicManager._songTotalTime > 0) {
      _songCompletionFillImage.fillAmount = _musicManager._songCurrentTime / _musicManager._songTotalTime;
    }
    else {
      _songCompletionFillImage.fillAmount = 0;
    }
  }

  private void UpdateSongNameText() {
    if (_musicManager._currentTrack != null) {
      _songNameText.text = _musicManager._currentTrack._trackName;
    }
    else {
      _songNameText.text = "Not Playing";
    }
  }

  private void UpdateSongArtistText() {
    if (_musicManager._currentTrack != null) {
      _songArtistText.text = _musicManager._currentTrack._trackArtist;
    }
    else {
      _songArtistText.text = "N/A";
    }
  }

  private void UpdateSongCurrentTimeText() {
    if (_musicManager._isPaused) return; // don't update if paused (it will be 0)
    if (_musicManager._songCurrentTime > 0) {
      _songCurrentTimeText.text = SecondsToTimestamp(_musicManager._songCurrentTime);
    }
    else {
      _songCurrentTimeText.text = SecondsToTimestamp(0);
    }
  }

  private void UpdateSongTotalTimeText() {
    if (_musicManager._isPaused) return;
    if (_musicManager._songTotalTime > 0) {
      _songTotalTimeText.text = SecondsToTimestamp(_musicManager._songTotalTime);
    }
    else {
      _songTotalTimeText.text = SecondsToTimestamp(0);
    }
  }
  
  private string SecondsToTimestamp(float seconds) {
    // formatted like this: hh:mm:ss
    // but if hours is 0, it will be formatted like this: mm:ss
    int hours = Mathf.FloorToInt(seconds / 3600);
    int minutes = Mathf.FloorToInt(seconds / 60) % 60;
    int secs = Mathf.FloorToInt(seconds % 60);
    if (hours > 0) {
      return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, secs);
    }
    else {
      return string.Format("{0:00}:{1:00}", minutes, secs);
    }
  }
}
