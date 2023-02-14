using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerSettings : MonoBehaviour
{

	[SerializeField] GameObject postProcessing;
	[SerializeField] private AudioSource[] audioSource;

	[SerializeField] private bool isMainMenu;

	private Bloom bloom;

	private void Start() {
		if (!isMainMenu) postProcessing.GetComponent<Volume>().profile.TryGet(out bloom);

		ApplySettings();
	}

	public void ApplySettings() {
		if (isMainMenu) return; // Prevents null pointer errors in main menu

		GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Controls_Sensitivity") * 2000;
		GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Controls_Sensitivity") * 2000;

		foreach (AudioSource audio in audioSource) {
			audio.volume = PlayerPrefs.GetFloat("Audio_Master") * PlayerPrefs.GetFloat("Audio_Effects");
		}

		Screen.brightness = (float)(PlayerPrefs.GetFloat("Video_Brightness") * 1.5);
		//bloom.intensity.value = PlayerPrefs.GetFloat("Video_Bloom") * 3;
		GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = PlayerPrefs.GetFloat("Video_Shake") * 2;
	}
}
