using UnityEngine;

public class DoorCameraFollow : MonoBehaviour {
  public Transform door;
  public Transform otherDoor;
  public Transform playerCameraTransform;

  [SerializeField] [Range(0, 2)] private float nearClipOffset = 0.05f;
  [SerializeField] [Range(0, 1)] private float nearClipLimit = 0.2f;

  private Camera playerCam;
  private Camera portalCam;

  private void Awake() {
    door = transform.parent;
    portalCam = GetComponent<Camera>();
  }

  private void Update() {
		// Finds and sets the current active player object
		playerCameraTransform = GetCurrentActivePlayer();

    playerCam = playerCameraTransform.GetComponent<Camera>();

    if (otherDoor) {
      Vector3 distanceBetweenObject = otherDoor.position - playerCameraTransform.position;

      transform.position = door.position + (door.rotation * Quaternion.Inverse(otherDoor.rotation) * new Vector3(distanceBetweenObject.x, -distanceBetweenObject.y, distanceBetweenObject.z)); // Position of door + door rotation offset + distance offset between doors

      transform.rotation = Quaternion.Euler(0, 180f, 0) * (door.rotation * Quaternion.Inverse(otherDoor.rotation)) * playerCameraTransform.rotation; // Initial 180 degree y rotation + door rotation offset + player camera rotation

      SetNearClipPlane();
    } else {
      Debug.Log("Variable otherDoor isn't assigned to gameObject " + gameObject.name);
      Destroy(gameObject);
    }

    Transform GetCurrentActivePlayer() {
      return GameObject.FindWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.GetComponent<SwitchableObject>()._raycastOrigin;
    }
  }

  private void SetNearClipPlane() {
		// Using from Sebastian Lague's tutorial on portals
		Transform clipPlane = door.transform;
		int dot = System.Math.Sign(Vector3.Dot(clipPlane.right, door.transform.position - transform.position));

		Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
		Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(clipPlane.right) * dot;
		float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal) + nearClipOffset;

		// Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
		if (Mathf.Abs(camSpaceDst) > nearClipLimit) {
			Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

			// Update projection based on new clip plane
			// Calculate matrix with player cam so that player camera settings (fov, etc) are used
			portalCam.projectionMatrix = playerCam.CalculateObliqueMatrix(clipPlaneCameraSpace);
		} else {
			portalCam.projectionMatrix = playerCam.projectionMatrix;
		}
	}
}
