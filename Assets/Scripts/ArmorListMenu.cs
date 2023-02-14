using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static ArmorData;

public class ArmorListMenu : MonoBehaviour
{

	public GameObject armorHolder;
	public GameObject nickNameText;
	public GameObject descriptionText;
	public GameObject checkButton;
	public GameObject readyButton;
	public GameObject[] statBars;
	public string chosenArmor;
	public float[] statNums;
	public int locationInList = 0;
	private string[] armorNames = new string[] {"Flowerpot"};
	private string[] armorDescriptors = new string[] {"You're not entirely sure what it is, but you found it in the junkyard. Seems like it might've held flowers."};

	public void Awake() {
		OnChooseArmor();
		ArmorData armor = new ArmorData(armorNames[locationInList]);
		PopulateArmorDataIndicators(armor);
	}

	public void Update() {
		if (armorNames[locationInList] == chosenArmor) {
			checkButton.GetComponent<Image>().color = new Color32(0, 255, 0, 100);
		} else {
			checkButton.GetComponent<Image>().color = new Color32(255, 0, 0, 100);
		}
		armorHolder.transform.GetChild(locationInList).RotateAround(armorHolder.transform.GetChild(locationInList).GetChild(0).GetComponent<Renderer>().bounds.center, Vector3.up, 30 * Time.unscaledDeltaTime);
	}

	public void OnClickLeft() {
		armorHolder.transform.GetChild(locationInList).gameObject.SetActive(false);
		if (locationInList == 0) {
			locationInList = 2;
		} else {
			locationInList--;
		}
		armorHolder.transform.GetChild(locationInList).gameObject.SetActive(true);
		ArmorData armor = new ArmorData(armorNames[locationInList]);
		PopulateArmorDataIndicators(armor);
	}

	public void OnClickRight() {
		armorHolder.transform.GetChild(locationInList).gameObject.SetActive(false);
		if (locationInList == 2) {
			locationInList = 0;
		} else {
			locationInList++;
		}
		armorHolder.transform.GetChild(locationInList).gameObject.SetActive(true);
		ArmorData armor = new ArmorData(armorNames[locationInList]);
		PopulateArmorDataIndicators(armor);
	}

	public void OnChooseArmor() {
		chosenArmor = armorNames[locationInList];
		GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>().selectedArmor = new ArmorData(armorNames[locationInList]);
	}

	void PopulateArmorDataIndicators(ArmorData armor) {
		nickNameText.GetComponent<TMP_Text>().text = "\"" + armor.nickName + "\"";
		descriptionText.GetComponent<TMP_Text>().text = armorDescriptors[locationInList];
		// float[] statNums = new float[] {1 / (armor.fireRate * (chosenAttachments[1] ? 0.75f : 1)) * 50, (armor.upRecoil + armor.backRecoil) * 300, armor.shotDamage * 50, armor.reloadTime * 100, (armor.magSize + (chosenAttachments[0] ? 10 : 0)) * 3};
		// for (int i = 0; i < 5; i++) {
		// 	statBars[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(statNums[i], statBars[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.y);
		// }
	}
}
