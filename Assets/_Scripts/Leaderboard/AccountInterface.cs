using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccountInterface : MonoBehaviour
{

  private LoginConnect loginManager;
  [SerializeField] private TMP_InputField loginUser, loginPass, regUser, regPass, regConfirmPass;
  //[SerializeField] private TMP_InputField deleteUser, deletePass, deleteConfirmPass, regEmail
  [SerializeField] private TextMeshProUGUI activeAccount;

  void Start() {
    loginManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LoginConnect>();
  }

  public void OnClickLogin() {
    string name = loginUser.text;
    string password = loginPass.text;
    StartCoroutine(loginManager.Login(name, password));
    if (loginManager.IsLoggedIn()) activeAccount.text = loginManager.GetActiveAccountName();
  }

  public void OnClickLogout() {
    loginManager.Logout();
    activeAccount.text = "Not logged in.";
  }

  /*
  public void OnClickDelete() {
    string name = deleteUser.text;
    string password = deletePass.text;
    string confirmpassword = deleteConfirmPass.text;
    string result = loginManager.DeleteAccount(name, password, confirmpassword);
    if (result == "Passwords did not match.") print(result);
  }
  */

  public void OnClickRegister() {
    string name = regUser.text;
    string email = "no_email";
    string password = regPass.text;
    string confirmpassword = regConfirmPass.text;
    string result = loginManager.Register(email, name, password, confirmpassword);
    if (result == "Passwords did not match.") print(result);
  }
}