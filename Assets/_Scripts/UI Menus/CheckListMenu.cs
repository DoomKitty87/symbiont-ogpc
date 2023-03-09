using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static GunData;

public class CheckListMenu : MonoBehaviour
{

	public GameObject gunHolder;
	public GameObject nickNameText;
	public GameObject modelNameText;
	public GameObject checkButton;
	public GameObject readyButton;
	public GameObject[] statBars;
	public string chosenGun;
	public float[] statNums;
	private string[] gunNames = new string[] {"Pistol", "Assault Rifle", "Heavy Rifle"};

	public void Awake() {
		try {
			chosenGun = GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>().selectedPrimary.name;
		} 
		catch (NullReferenceException) {
			GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>().selectedPrimary = new GunData("Pistol");
			chosenGun = "Pistol";
		}
		GunData gun = new GunData(chosenGun);
		PopulateGunDataIndicators(gun);
		gunHolder.transform.GetChild(Array.IndexOf(gunNames, chosenGun)).gameObject.SetActive(true);
	}

	public void Update() {
		gunHolder.transform.GetChild(Array.IndexOf(gunNames, chosenGun)).RotateAround(gunHolder.transform.GetChild(Array.IndexOf(gunNames, chosenGun)).GetChild(0).GetComponent<Renderer>().bounds.center, Vector3.up, 30 * Time.unscaledDeltaTime);
	}
	public void OnClickReady() {
		SceneManager.LoadScene("SampleScene");
	}

	void PopulateGunDataIndicators(GunData gun) {
		nickNameText.GetComponent<TMP_Text>().text = "\"" + gun.nickName + "\"";
		modelNameText.GetComponent<TMP_Text>().text = gun.modelName;
		List<Attachment> selectedAttachments = GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>().selectedAttachments;
		float[] statNums = new float[] {1 / (gun.fireRate * (selectedAttachments.Contains(new Attachment("Accelerator")) ? 0.75f : 1)) * 50, (gun.upRecoil + gun.backRecoil) * 300, gun.shotDamage * 50, gun.reloadTime * 100, gun.magSize + (selectedAttachments.Contains(new Attachment("Li-Ion Battery")) ? 10 : 0) * 3};
		for (int i = 0; i < 5; i++) {
			statBars[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(statNums[i], statBars[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.y);
		}
	}
}
