using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerSettings : MonoBehaviour
{

	[SerializeField] GameObject postProcessing;
	[SerializeField] private AudioSource[] audioSource;

	private Bloom bloom;

	private void Start() {
		postProcessing.GetComponent<Volume>().profile.TryGet(out bloom);

		ApplySettings();
	}

	public void ApplySettings() {
		GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Controls_Sensitivity") * 2000;
		GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Controls_Sensitivity") * 2000;

		foreach (AudioSource audio in audioSource) {
			audio.volume = PlayerPrefs.GetFloat("Audio_Master") * PlayerPrefs.GetFloat("Audio_Effects");
		}

		Screen.brightness = (float)(PlayerPrefs.GetFloat("Video_Brightness") * 1.5);
		bloom.intensity.value = PlayerPrefs.GetFloat("Video_Bloom") * 3;
		Debug.Log(Screen.brightness);
		Debug.Log((float)(PlayerPrefs.GetFloat("Video_Brightness") * 1.5));

	}
}
