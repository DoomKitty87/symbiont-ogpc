using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{

	[SerializeField] private AudioSource[] audioSource;

	private void Start() {
		ApplySettings();
	}

	public void ApplySettings() {
		GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Controls_Sensitivity") * 2000;
		GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Controls_Sensitivity") * 2000;

		foreach (AudioSource audio in audioSource) {
			audio.volume = PlayerPrefs.GetFloat("Audio_Master") * PlayerPrefs.GetFloat("Audio_Effects");
		}
	}
}
