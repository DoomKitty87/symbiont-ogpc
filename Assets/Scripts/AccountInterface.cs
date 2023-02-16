using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccountInterface : MonoBehaviour
{

  private LoginConnect loginManager;
  [SerializeField] private TMP_InputField loginUser, loginPass, deleteUser, deletePass, deleteConfirmPass, regUser, regEmail, regPass, regConfirmPass;

  void Start() {
    loginManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LoginConnect>();
  }

  public void OnClickLogin() {
    string name = loginUser.text;
    string password = loginPass.text;
    StartCoroutine(loginManager.Login(name, password));
  }

  public void OnClickLogout() {
    loginManager.Logout();
  }

  public void OnClickDelete() {
    string name = deleteUser.text;
    string password = deletePass.text;
    string confirmpassword = deleteConfirmPass.text;
    string result = loginManager.DeleteAccount(name, password, confirmpassword);
    if (result == "Passwords did not match.") print(result);
  }

  public void OnClickRegister() {
    string name = regUser.text;
    string email = regEmail.text;
    string password = regPass.text;
    string confirmpassword = regConfirmPass.text;
    string result = loginManager.Register(email, name, password, confirmpassword);
    if (result == "Passwords did not match.") print(result);
  }
}