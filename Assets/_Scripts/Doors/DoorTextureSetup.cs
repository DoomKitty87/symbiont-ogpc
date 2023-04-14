using UnityEngine;

public class DoorTextureSetup : MonoBehaviour {

	[SerializeField] private Camera otherCamera;

	[SerializeField] private Material cameraMat;

	private void Start() {
		if (otherCamera.targetTexture != null) otherCamera.targetTexture.Release();
		otherCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMat.mainTexture = otherCamera.targetTexture;
	}
}