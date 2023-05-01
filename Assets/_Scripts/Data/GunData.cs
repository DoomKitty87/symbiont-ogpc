public class GunData
{

  public string name;
  public string manufacturer;
  public string modelName;
  public string nickName;
  public float fireRate;
  public float upRecoil;
  public float backRecoil;
  public float recoilRecovery;
  public float shotSpread;
  public float reloadTime;
  public float shotDamage;
  public bool upRecoilAnim;
  public int shotColor;
  public int magSize;
  public int id;
  public Attachment[] attachments;

  public GunData(string inName) {
    name = inName;
    switch(name) {
      case "Pistol":
        id = 0;
        manufacturer = "ElectroArms";
        modelName = "ZAP29";
        nickName = "Bugzapper";
        fireRate = 0.5f;
        upRecoil = 0.2f;
        backRecoil = 0.3f;
        recoilRecovery = 0.4f;
        shotSpread = 3f;
        reloadTime = 1f;
        upRecoilAnim = false;
        shotColor = 2;
        magSize = 10;
        shotDamage = 3;
        break;
      case "Assault Rifle":
        id = 1;
        manufacturer = "PJSC Molnii Ruki";
        modelName = "TOK35";
        nickName = "Liberator";
        fireRate = 0.125f;
        upRecoil = 0.05f;
        backRecoil = 0.09f; // For GunController anim
        recoilRecovery = 0.5f;
        shotSpread = 5f;
        reloadTime = 1.5f;
        upRecoilAnim = false;
        shotColor = 0;
        magSize = 30;
        shotDamage = 2;
        break;
      case "Heavy Rifle":
        id = 2;
        manufacturer = "Krieslauf AG";
        modelName = "OHM100";
        nickName = "Resistor";
        fireRate = 0.5f;
        upRecoil = 0.05f;
        backRecoil = 0.25f;
        recoilRecovery = 0.4f;
        shotSpread = 2.5f;
        reloadTime = 2f;
        upRecoilAnim = false;
        shotColor = 1;
        magSize = 8;
        shotDamage = 7;
        break;
    }
  }
}

public class Attachment
{

  public string name;
  public int type;
  public float value;

  public Attachment(string inName) {
    name = inName;
    switch(name) {
      case "Li-Ion Battery":
        type = 0;
        value = 10;
        break;
      case "Accelerator":
        type = 1;
        value = 0.75f;
        break;
    }
  }
}