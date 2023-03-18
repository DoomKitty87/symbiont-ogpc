using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    private GameObject _currActiveTarget;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            CheckForTarget();
            Debug.Log("Click");
        }

        FollowTarget();
    }

    private void FollowTarget() {
        transform.position = _currActiveTarget.transform.position;
    }

    private void CheckForTarget() {
        // TODO:
        // Put in check for if a target is 'teleportable'

        RaycastHit hit;

        float maxTeleportDistance = 100.0f;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxTeleportDistance)) {
            GameObject target = hit.transform.gameObject;
            if (target.CompareTag("Target")){
                TeleportToTarget(target);
            }
        }
    }

    private void TeleportToTarget(GameObject target) {
        // TODO:
        // Lerp the camera location towards target
        _currActiveTarget = target;
    }
}
