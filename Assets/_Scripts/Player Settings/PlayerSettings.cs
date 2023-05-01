using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerSettings : MonoBehaviour
{

	[SerializeField] GameObject postProcessing;
	[SerializeField] private AudioSource[] audioSource;

	[SerializeField] private bool isMainMenu;

	private void Start() {
		// ApplySettings();
	}

	public void ApplySettings() {
		if (isMainMenu) return; // Prevents null pointer errors in main menu

		// GameObject.FindGameObjectWithTag("LookController").GetComponent<PlayerAim>()._horizontalSens = PlayerPrefs.GetFloat("controls_SENSITIVITY");
		// GameObject.FindGameObjectWithTag("LookController").GetComponent<PlayerAim>()._verticalSens = PlayerPrefs.GetFloat("controls_SENSITIVITY");

		foreach (AudioSource audio in audioSource) {
			// audio.volume = PlayerPrefs.GetFloat("audio_MASTER") * PlayerPrefs.GetFloat("audio_EFFECTS");
		}

		// Screen.brightness = (float)(PlayerPrefs.GetFloat("video_BRIGHTNESS") * 1.5);
		// GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = PlayerPrefs.GetFloat("video_SHAKE") * 2;
	}
}
