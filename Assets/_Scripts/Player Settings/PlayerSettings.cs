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
		ApplySettings();
	}

	public void ApplySettings() {
		if (isMainMenu) return; // Prevents null pointer errors in main menu

		GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("controls_SENSITIVITY") * 4000;
		GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("controls_SENSITIVITY") * 4000;

		foreach (AudioSource audio in audioSource) {
			// audio.volume = PlayerPrefs.GetFloat("audio_MASTER") * PlayerPrefs.GetFloat("audio_EFFECTS");
		}

		Screen.brightness = (float)(PlayerPrefs.GetFloat("video_BRIGHTNESS") * 1.5);
		GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = PlayerPrefs.GetFloat("video_SHAKE") * 2;
	}
}
