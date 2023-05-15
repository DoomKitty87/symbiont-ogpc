using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class SwitchableObjectRegister 
{
  public bool _currentlySwitchedTo;
  public SwitchableObject _switchComponent;

  public SwitchableObjectRegister(bool currentlySwitchedTo, SwitchableObject switchableObject) {
    _currentlySwitchedTo = currentlySwitchedTo;
    _switchComponent = switchableObject;
  }
}

public class ViewSwitcher : MonoBehaviour
{
  // On a trigger, activate an effect (tbd, fov increase animation for now) + sounds and change the
  // _currentPlayerCamera to the camera of the currently targeted switchable object.
  //
  // Visually show the currently targeted object through an outline.
  // (Someone please figure out how to do those)
  //
  // Determine the currently targeted switchable object through a raycast,
  // and determine whether it is switchable through a "switchable" tag.
  //
  // A component on the target, SwitchableObject, should take care of the tagging.
  // SwitchableObject, once activated through this script after switching the cameras,
  // should turn on and off scripts, activate player inputs, etc.
  //
  // This should probably also switch off the player input for the current target.
  
  [Header("Objects")]
  [Header("If this is not checked, you'll need to assign \nan object in Current Object to start off with when the game loads.")]
  [SerializeField] private bool _startWithRandomObject;
  public SwitchableObject _currentObjectInhabiting;
  [SerializeField] private SwitchableObject _selectedSwitchableObject;
  [Header("Note: Configurations for selection raycasts are on each object")]
  [Header("Input")]
  [SerializeField] private string _switchAxis;
  [SerializeField] private float _secondsSinceLastSwitch = 0f;
  [SerializeField] private float _switchCooldown;
  private bool _switchAxisDown = false; // Only true on first frame axis is detected
  private bool _switchAxisHeldLastFrame = false;
  
  [Header("Switching Effect")]
  [SerializeField] private AnimationCurve _effectInCurve;
  [SerializeField] private AnimationCurve _effectOutCurve;
  [Range(0, 1)][SerializeField] private float _switchPoint;
  [SerializeField] private float _endFov;
  [SerializeField] private float _effectDuration;
  [SerializeField] private bool _playingEffect;

  private void Start() {
    if (!IsInputAxisValid(_switchAxis)) {
      Debug.LogError("ViewSwitcher: SwitchAxis is invalid! Please set it to a valid input axis in the Input Manager.");
    }
    if (_currentObjectInhabiting == null) {
      if (_startWithRandomObject) {
        StartCoroutine(FindSwitchableObject());
      }
      else {
        Debug.LogError("ViewSwitcher: Current Object is null! Please assign an object to start off with when the game loads.");
      }
    }
    else {
      _currentObjectInhabiting.SwitchTo();
    }
  }
  private IEnumerator FindSwitchableObject() {
    SwitchableObject switchableObject;
    while (true) {
      switchableObject = GameObject.FindGameObjectWithTag("SwitchableObject").GetComponent<SwitchableObject>();
      if (switchableObject != null) {
        break;
      }
      yield return null;
    }
    _currentObjectInhabiting = switchableObject;
    _currentObjectInhabiting.SwitchTo();
  }

  private void Update() {
    _secondsSinceLastSwitch += Time.deltaTime;

    CheckForNewSelectedObject();
    UpdateInhabitInput();
    InhabitIfInputDetected();
  }

  // -------------------------------------------------

  private void CheckForNewSelectedObject() {
    SwitchableObject targetedObject = RaycastForSwitchableObject();
    if (targetedObject == null) {
      if (_selectedSwitchableObject != null) _selectedSwitchableObject.Selected(false);
      _selectedSwitchableObject = null;
      return;  
    } 
    if (targetedObject == _selectedSwitchableObject) {
      return;
    }
    else {
      _selectedSwitchableObject = targetedObject;
      _selectedSwitchableObject.Selected(true);
    }
  }
  private void UpdateInhabitInput() {
    if (Input.GetAxisRaw(_switchAxis) == 1) {
      if (_switchAxisHeldLastFrame) {
        _switchAxisDown = false;
        return;
      }
      else {
        _switchAxisDown = true;
      }
    }
    else {
      _switchAxisDown = false;
    }
  }
  private void InhabitIfInputDetected() {
    if (_selectedSwitchableObject == null) return;
    if (_selectedSwitchableObject == _currentObjectInhabiting) {
      Debug.LogWarning("ViewSwitcher: The SelectedSwitchableObject is the same as the currently inhabited object! Is the raycast intersecting with it's origin's collider?");
      return;
    }
    if (_switchAxisDown && CanSwitch()) {
      PlayFovEffectAndSwitch();
    }
  }
  private void PlayFovEffectAndSwitch() {
    if (_playingEffect) return;
    else {
      StartCoroutine(PlayFovEffectAndSwitchCoroutine());
    }
  }
  private IEnumerator PlayFovEffectAndSwitchCoroutine() {
    float duration = _effectDuration * GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>().GetTeleportSpeed();
    _playingEffect = true;
    float timeElapsed = 0f;
    float startFov = _currentObjectInhabiting._objectCameras[0].fieldOfView;
    VolumeProfile volumeProfile = GameObject.FindGameObjectWithTag("Post Processing").GetComponent<Volume>().profile;
    Bloom bloom;
    LensDistortion lensDistortion;
    ColorAdjustments colorAdjustments;
    ChromaticAberration chromaticAberration;

    volumeProfile.TryGet(out bloom);
    volumeProfile.TryGet(out lensDistortion);
    volumeProfile.TryGet(out colorAdjustments);
    volumeProfile.TryGet(out chromaticAberration);

    float lensInit = (float)lensDistortion.intensity;
    float bloomInit = (float)bloom.intensity;
    float exposureInit = (float)colorAdjustments.postExposure;
    float aberrationInit = (float)chromaticAberration.intensity;

    // Kept in memory in case the player looks away from object during effect
    SwitchableObject selectedObject = _selectedSwitchableObject;
    while (timeElapsed < duration / 2) {
      //_currentObjectInhabiting._objectCameras[0].fieldOfView = Mathf.Lerp(startFov, _endFov, _effectInCurve.Evaluate(timeElapsed / (_effectDuration / 2)));
      bloom.intensity.Override(Mathf.Lerp(bloomInit, bloomInit * 5, _effectInCurve.Evaluate(timeElapsed / (duration / 2))));
      lensDistortion.intensity.Override(Mathf.Lerp(lensInit, -0.6f, _effectInCurve.Evaluate(timeElapsed / (duration / 2))));
      colorAdjustments.postExposure.Override(Mathf.Lerp(exposureInit, 0.7f, _effectInCurve.Evaluate(timeElapsed / (duration / 2))));
      chromaticAberration.intensity.Override(Mathf.Lerp(aberrationInit, 1, _effectInCurve.Evaluate(timeElapsed / (duration / 2))));
      yield return null;
      timeElapsed += Time.deltaTime;
    }
    //_currentObjectInhabiting._objectCameras[0].fieldOfView = _endFov;
    SwitchToSelected(selectedObject);
    timeElapsed = 0f;
    while (timeElapsed < duration / 2) {
      //_currentObjectInhabiting._objectCameras[0].fieldOfView = Mathf.Lerp(startFov, _endFov, _effectOutCurve.Evaluate(timeElapsed / (_effectDuration / 2)));
      bloom.intensity.Override(Mathf.Lerp(bloomInit, bloomInit * 5, _effectOutCurve.Evaluate(timeElapsed / (duration / 2))));
      lensDistortion.intensity.Override(Mathf.Lerp(lensInit, -0.6f, _effectOutCurve.Evaluate(timeElapsed / (duration / 2))));
      colorAdjustments.postExposure.Override(Mathf.Lerp(exposureInit, 0.7f, _effectOutCurve.Evaluate(timeElapsed / (duration / 2))));
      chromaticAberration.intensity.Override(Mathf.Lerp(aberrationInit, 1, _effectOutCurve.Evaluate(timeElapsed / (duration / 2))));
      yield return null;
      timeElapsed += Time.deltaTime;
    }
    _currentObjectInhabiting._objectCameras[0].fieldOfView = startFov;
    _playingEffect = false;

    bloom.intensity.Override(bloomInit);
    lensDistortion.intensity.Override(lensInit);
    colorAdjustments.postExposure.Override(exposureInit);
    chromaticAberration.intensity.Override(aberrationInit);
  }
  private void SwitchToSelected(SwitchableObject selectedObject) {
    Quaternion initRot = _currentObjectInhabiting._rotationBase.rotation;
    
    _secondsSinceLastSwitch = 0f;
    selectedObject.SwitchTo();
    _currentObjectInhabiting.SwitchAway();
    GameObject nowInRoom = _currentObjectInhabiting.gameObject;
    GameObject switchToRoom = selectedObject.gameObject;
    while (!nowInRoom.CompareTag("Room")) {
      nowInRoom = nowInRoom.transform.parent.gameObject;
    }
    while (!switchToRoom.CompareTag("Room")) {
      switchToRoom = switchToRoom.transform.parent.gameObject;
    }

    if (nowInRoom != switchToRoom) {
      switchToRoom.GetComponent<RoomHandler>().CloseDoors();
    }
    _currentObjectInhabiting.gameObject.GetComponent<HealthManager>()._currentHealth -= GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>().GetMaxHealthIncrease();
    _currentObjectInhabiting.gameObject.GetComponent<HealthManager>()._maxHealth -= GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>().GetMaxHealthIncrease();
    _currentObjectInhabiting = selectedObject;
    _selectedSwitchableObject = null;
    _currentObjectInhabiting._rotationBase.rotation = initRot * switchToRoom.transform.rotation;
    _currentObjectInhabiting.gameObject.GetComponent<HealthManager>().Heal(GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>().GetHealOnTeleport());
    float initHealth = _currentObjectInhabiting.gameObject.GetComponent<HealthManager>()._currentHealth;
    _currentObjectInhabiting.gameObject.GetComponent<HealthManager>()._currentHealth += GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>().GetMaxHealthIncrease();
    _currentObjectInhabiting.gameObject.GetComponent<HealthManager>()._maxHealth += GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>().GetMaxHealthIncrease();
    _currentObjectInhabiting.gameObject.GetComponent<HealthManager>().UpdateHealth(initHealth, _currentObjectInhabiting.gameObject.GetComponent<HealthManager>()._currentHealth, _currentObjectInhabiting.gameObject.GetComponent<HealthManager>()._maxHealth);
    GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerTracker>().Switched();
  }
  private bool CanSwitch() {
    if (_secondsSinceLastSwitch <= (_effectDuration * GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>().GetTeleportSpeed()) + (_switchCooldown * GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>().GetTeleportSpeed()) || _playingEffect) {
      return false;
    }
    else {
      return true;
    }
  }
  private SwitchableObject RaycastForSwitchableObject() {
    Transform raycastOrigin = _currentObjectInhabiting._raycastOrigin;
    Vector3 raycastOriginOffset = _currentObjectInhabiting._raycastOriginOffset;
    float raycastDistance = _currentObjectInhabiting._raycastDistance;
    LayerMask layerMask = _currentObjectInhabiting._layerMask;
    bool showRaycast = _currentObjectInhabiting._showRaycast;
    RaycastHit hit;
    bool didHitCollider = Physics.Raycast(raycastOrigin.position + raycastOriginOffset, raycastOrigin.forward, out hit, raycastDistance, layerMask);
    if (showRaycast) {
      DrawDebugRaycast(raycastOrigin.position + raycastOriginOffset, raycastOrigin.forward, raycastDistance, didHitCollider ? Color.yellow : Color.green);
    }
    // Nothing hit
    if (hit.collider == null) return null;
    // Door hit
    if (hit.collider.gameObject.CompareTag("DoorGraphic")) {
      didHitCollider = Physics.Raycast(GameObject.FindGameObjectWithTag("Handler").GetComponent<RoomGenNew>()._currentRoom.GetComponent<RoomHandler>()._instantiatedCamera.transform.position + raycastOriginOffset, GameObject.FindGameObjectWithTag("Handler").GetComponent<RoomGenNew>()._currentRoom.GetComponent<RoomHandler>()._instantiatedCamera.transform.forward, out hit, raycastDistance, layerMask);
      DrawDebugRaycast(GameObject.FindGameObjectWithTag("Handler").GetComponent<RoomGenNew>()._currentRoom.GetComponent<RoomHandler>()._instantiatedCamera.transform.position + raycastOriginOffset, GameObject.FindGameObjectWithTag("Handler").GetComponent<RoomGenNew>()._currentRoom.GetComponent<RoomHandler>()._instantiatedCamera.transform.forward, raycastDistance, didHitCollider ? Color.yellow : Color.green);
    }
    //Second null check to avoid error throws
    if (hit.collider == null) return null;
    // Switchable Object hit
    if (hit.collider.GetComponent<SwitchableObject>() != null) {
      return hit.collider.GetComponent<SwitchableObject>();
    }
    // Something else hit
    return null;
  }
  private bool IsInputAxisValid(string axisName) {
    try {
      Input.GetAxis(axisName);
      return true;
    }
    catch (System.Exception e){
      Debug.LogError(e);
      return false;
    }
  }
  private void DrawDebugRaycast(Vector3 origin, Vector3 direction, float distance, Color color) {
    Debug.DrawLine(origin, (direction.normalized * distance) + origin, color, Time.deltaTime);
  }
}
