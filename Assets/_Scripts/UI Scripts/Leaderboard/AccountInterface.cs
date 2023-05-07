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
  [SerializeField] private TMP_InputField _loginUsername;
  [SerializeField] private TMP_InputField _loginPassword;
  [SerializeField] private string _passwordIncorrectError, _noCredentialsEnteredError;
  [SerializeField] private FadeElementInOut _loginErrorFade;
  [SerializeField] private TextMeshProUGUI _loginErrorText;
  [SerializeField] private FadeElementInOut _loginButtonFade;
  [SerializeField] private Button _loginButton;
  [Header("Register")] 
  [SerializeField] private TMP_InputField _registerUsername, _registerPassword, _registerConfirmPassword;
  [SerializeField] private TextMeshProUGUI _registerErrorText;
  [SerializeField] private Button _registerButton;
  [Header("Delete")]
  [SerializeField] private TMP_InputField _deleteUsername, deletePass, deleteConfirmPass;
  [Header("Account Display")]
  [SerializeField] private TextMeshProUGUI _activeAccountDisplayText;

  [SerializeField] private bool _isWaitingForLogin = false;

  void Start() {
    _loginManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LoginConnect>();
    _isWaitingForLogin = false;

    _loginButton.onClick.AddListener(OnClickLogin);
  }

  public void OnClickLogin() {
    _loginButtonFade.FadeOut(false);
    if (_isWaitingForLogin) return;
    if (_loginUsername.text == "" || _loginPassword.text == "") {
      DisplayLoginError(_noCredentialsEnteredError);
      _loginButtonFade.FadeIn(false);
      return;
    }
    StartCoroutine(OnClickLoginCoroutine(_loginUsername.text, _loginPassword.text));
  }

  private IEnumerator OnClickLoginCoroutine(string username, string password) {
    _isWaitingForLogin = true;
    HideLoginError();
    yield return StartCoroutine(_loginManager.Login(username, password));
    if (_loginManager.IsLoggedIn()) {
      _activeAccountDisplayText.text = _loginManager.GetActiveAccountName();
    }
    else {
      DisplayLoginError(_passwordIncorrectError);
      _loginButtonFade.FadeIn(true);
    }
    _isWaitingForLogin = false;
  }

  private void DisplayLoginError(string textToDisplay) {
    _loginErrorText.text = textToDisplay;
    _loginErrorFade.FadeIn(true);
  }

  private void HideLoginError() {
    _loginErrorFade.FadeOut(false);
  }

  public void OnClickLogout() {
    _loginManager.Logout();
    _activeAccountDisplayText.text = "Not logged in.";
  }

  public void OnClickDelete() {
    string name = _deleteUsername.text;
    string password = deletePass.text;
    string confirmpassword = deleteConfirmPass.text;
    string result = _loginManager.DeleteAccount(name, password, confirmpassword);
    if (result == "Passwords did not match.") print(result);
  }

  public void OnClickRegister() {
    string name = _registerUsername.text;
    string email = "no_email";
    string password = _registerPassword.text;
    string confirmpassword = _registerConfirmPassword.text;
    string result = _loginManager.Register(email, name, password, confirmpassword);
    if (result == "Passwords did not match.") print(result);
  }
}