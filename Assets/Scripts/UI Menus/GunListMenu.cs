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
  private string[] attachmentNames = new string[] {"Li-Ion Battery", "Accelerator"};
  private bool[] chosenAttachments;

	public void Awake() {
		OnChooseGun();
		GunData gun = new GunData(gunNames[locationInList]);
		chosenAttachments = new bool[attachmentNames.Length];
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
    PopulateAttachments();
	}

	public void OnChooseGun() {
		chosenGun = gunNames[locationInList];
		
		GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>().selectedPrimary = new GunData(gunNames[locationInList]);
	}

  public void OnClickAttachment(int id) {
    if (!GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>().unlockedAttachments.Contains(id)) return;
    chosenAttachments[id] ^= true;
    transform.parent.GetChild(1).GetChild(id).gameObject.GetComponent<Image>().color = chosenAttachments[id] ? Color.green : Color.red;
		PopulateGunDataIndicators(new GunData(gunNames[locationInList]));
  }

  public void PopulateAttachments() {
    GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>().selectedAttachments = new List<Attachment>();
		if (chosenAttachments == null) chosenAttachments = new bool[0];
    for (int i = 0; i < chosenAttachments.Length; i++) {
      if (chosenAttachments[i]) GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>().selectedAttachments.Add(new Attachment(attachmentNames[i]));
    }
  }

	void PopulateGunDataIndicators(GunData gun) {
		nickNameText.GetComponent<TMP_Text>().text = "\"" + gun.nickName + "\"";
		modelNameText.GetComponent<TMP_Text>().text = gun.modelName;
		float[] statNums = new float[] {1 / (gun.fireRate * (chosenAttachments[1] ? 0.75f : 1)) * 50, (gun.upRecoil + gun.backRecoil) * 300, gun.shotDamage * 50, gun.reloadTime * 100, (gun.magSize + (chosenAttachments[0] ? 10 : 0)) * 3};
		for (int i = 0; i < 5; i++) {
			statBars[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(statNums[i], statBars[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.y);
		}
	}
}