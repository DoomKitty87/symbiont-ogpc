using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static GunData;

public class GunListMenu : MonoBehaviour
{

	public GameObject gunHolder;
	public GameObject nickNameText;
	public GameObject modelNameText;
	public GameObject checkButton;
	public GameObject readyButton;
	public GameObject[] statBars;
	public string chosenGun;
	public float[] statNums;
	public int locationInList = 0;
	private string[] gunNames = new string[] {"Pistol", "Assault Rifle", "Heavy Rifle"};
  private bool[] chosenAttachments = new bool[1];
  private PersistentData dataContainer;

	public void Awake() {
		OnChooseGun();
		GunData gun = new GunData(gunNames[locationInList]);
    dataContainer = GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>();
		PopulateGunDataIndicators(gun);
	}

	public void Update() {
		if (gunNames[locationInList] == chosenGun) {
			checkButton.GetComponent<Image>().color = new Color32(0, 255, 0, 100);
		} else {
			checkButton.GetComponent<Image>().color = new Color32(255, 0, 0, 100);
		}
		gunHolder.transform.GetChild(locationInList).RotateAround(gunHolder.transform.GetChild(locationInList).GetChild(0).GetComponent<Renderer>().bounds.center, Vector3.up, 30 * Time.unscaledDeltaTime);
	}

	public void OnClickLeft() {
		gunHolder.transform.GetChild(locationInList).gameObject.SetActive(false);
		if (locationInList == 0) {
			locationInList = 2;
		} else {
			locationInList--;
		}
		gunHolder.transform.GetChild(locationInList).gameObject.SetActive(true);
		GunData gun = new GunData(gunNames[locationInList]);
		PopulateGunDataIndicators(gun);
	}

	public void OnClickRight() {
		gunHolder.transform.GetChild(locationInList).gameObject.SetActive(false);
		if (locationInList == 2) {
			locationInList = 0;
		} else {
			locationInList++;
		}
		gunHolder.transform.GetChild(locationInList).gameObject.SetActive(true);
		GunData gun = new GunData(gunNames[locationInList]);
		PopulateGunDataIndicators(gun);
	}

	public void OnClickReady() {
		SceneManager.LoadScene("SampleScene");
	}

	public void OnChooseGun() {
		chosenGun = gunNames[locationInList];
		dataContainer.selectedGun = new GunData(gunNames[locationInList]);
	}

  public void OnClickAttachment(int id) {
    chosenAttachments[id] ^= true;
    transform.GetChild(1).GetChild(id).GetComponent<Image>().color = (chosenAttachments[id] ? Color.green : Color.red);
  }

	void PopulateGunDataIndicators(GunData gun) {
		nickNameText.GetComponent<TMP_Text>().text = "\"" + gun.nickName + "\"";
		modelNameText.GetComponent<TMP_Text>().text = gun.modelName;
		float[] statNums = new float[] {1 / gun.fireRate * 50, (gun.upRecoil + gun.backRecoil) * 300, gun.shotSpread * 50, gun.reloadTime * 100, gun.magSize * 3};
		for (int i = 0; i < 5; i++) {
			statBars[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(statNums[i], statBars[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.y);
		}
	}
}