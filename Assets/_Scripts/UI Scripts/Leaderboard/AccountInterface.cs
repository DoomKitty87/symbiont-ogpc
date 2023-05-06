using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AccountInterface : MonoBehaviour
{
  [Header("References")]
  private LoginConnect _loginManager;
  [Header("Login")]
  [SerializeField] private TMP_InputField _loginUsername, _loginPassword;
  [SerializeField] private TextMeshProUGUI _loginErrorText;
  [SerializeField] private Button _loginButton;
  [Header("Register")] 
  [SerializeField] private TMP_InputField _registerUsername, _registerPassword, _registerConfirmPassword;
  [SerializeField] private TextMeshProUGUI _registerErrorText;
  [SerializeField] private Button _registerButton;
  [Header("Delete")]
  [SerializeField] private TMP_InputField _deleteUsername, deletePass, deleteConfirmPass
  [SerializeField] private TextMeshProUGUI _activeAccountDisplay;

  void Start() {
    _loginManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LoginConnect>();
  }

  public void OnClickLogin() {
    string name = _loginUsername.text;
    string password = _loginPassword.text;
    StartCoroutine(_loginManager.Login(name, password));
    if (_loginManager.IsLoggedIn()) _activeAccountDisplay.text = _loginManager.GetActiveAccountName();
  }

  public void OnClickLogout() {
    _loginManager.Logout();
    _activeAccountDisplay.text = "Not logged in.";
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
    string name = _registerUsername.text;
    string email = "no_email";
    string password = _registerPassword.text;
    string confirmpassword = _registerConfirmPassword.text;
    string result = _loginManager.Register(email, name, password, confirmpassword);
    if (result == "Passwords did not match.") print(result);
  }
}