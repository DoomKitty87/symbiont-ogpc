using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class AccountInterface : MonoBehaviour
{
  [Header("References")]
  private LoginConnect _loginManager;

  [Header("Login")]
  [SerializeField] private TMP_InputField _loginUsername;
  [SerializeField] private TMP_InputField _loginPassword;
  [SerializeField] private string _loginInvalidError, _loginNoCredentialsEnteredError;
  [SerializeField] private FadeElementInOut _loginErrorFade;
  [SerializeField] private TextMeshProUGUI _loginErrorText;
  [SerializeField] private FadeElementInOut _loginButtonFade;
  [SerializeField] private Button _loginButton;

  [Header("Register")] 
  [SerializeField] private TMP_InputField _registerUsername;
  [SerializeField] private TMP_InputField _registerPassword;
  [SerializeField] private TMP_InputField _registerConfirmPassword;
  [SerializeField] private string _registerPasswordIncorrectError, _registerNoCredentialsEnteredError;
  [SerializeField] private FadeElementInOut _registerUsernameErrorFade;
  [SerializeField] private TextMeshProUGUI _registerUsernameErrorText;
    [SerializeField] private FadeElementInOut _registerPasswordErrorFade;
  [SerializeField] private TextMeshProUGUI _registerPasswordErrorText;
  [SerializeField] private FadeElementInOut _registerButtonFade;
  [SerializeField] private Button _registerButton;

  [Header("These will be available once signed in.")]

  [Header("Delete")]
  [SerializeField] private TMP_InputField _deletePassword;
  [SerializeField] private TMP_InputField _deleteConfirmPassword;
  [SerializeField] private FadeElementInOut _deleteErrorFade;
  [SerializeField] private TextMeshProUGUI _deleteErrorText;
  [SerializeField] private FadeElementInOut _deleteButtonFade;
  [SerializeField] private Button _deleteButton;
  [Header("Account Display")]
  [SerializeField] private TextMeshProUGUI _activeAccountDisplayText;

  
  [Header("Events")]
  [SerializeField] private UnityEvent _onLoginSuccess;
  [SerializeField] private UnityEvent _onRegisterSuccess;


  // Coroutine multicall protection
  private bool _isWaitingForLogin = false;
  private bool _isWaitingForRegister = false;

  void Start() {
    _loginManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LoginConnect>();
    _isWaitingForLogin = false;

    _loginButton.onClick.AddListener(OnClickLogin);
    _registerButton.onClick.AddListener(OnClickRegister);
  }

  // For all account functions: OnClick will check for basics like password confirmation and empty fields.
  // It will then call the coroutine, which will handle the actual database interaction.
  // The coroutine will then call the appropriate function to display errors or update the UI.

  // Login ---------------------------------

  public void OnClickLogin() {
    _loginButtonFade.FadeOut(false);
    if (_isWaitingForLogin) return;
    if (_loginUsername.text == "" || _loginPassword.text == "") {
      DisplayLoginError(_loginNoCredentialsEnteredError);
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
      _onLoginSuccess?.Invoke();
    }
    else {
      DisplayLoginError(_loginInvalidError);
      _loginButtonFade.FadeIn(true);
    }
    _isWaitingForLogin = false;
  }

  // Login Helper Functions

  private void DisplayLoginError(string textToDisplay) {
    _loginErrorText.text = textToDisplay;
    _loginErrorFade.FadeIn(true);
  }

  private void HideLoginError() {
    _loginErrorFade.FadeOut(false);
  }

  // Register ---------------------------------

  // TODO: Finish implementing register; this will require a change to the database,
  // and a callback coroutine from LoginConnect, just like login

  public void OnClickRegister() {
    _registerButtonFade.FadeOut(false);
    if (_isWaitingForRegister) return;
    if (_registerUsername.text == "" || _registerPassword.text == "" || _registerConfirmPassword.text == "") {
      DisplayPasswordRegisterError(_registerNoCredentialsEnteredError);
      _registerButtonFade.FadeIn(false);
      return;
    }
    if (_registerPassword.text != _registerConfirmPassword.text) {
      DisplayPasswordRegisterError(_registerPasswordIncorrectError);
      _registerButtonFade.FadeIn(false);
      return;
    }
    HideUsernameRegisterError();
    HidePasswordRegisterError();

    // TODO: Remove email from database
    string email = "no_email";

    string name = _registerUsername.text;
    string password = _registerPassword.text;
    _loginManager.Register(email, name, password);
  }

  private void DisplayUsernameRegisterError(string textToDisplay) {
    _registerUsernameErrorText.text = textToDisplay;
    _registerUsernameErrorFade.FadeIn(true);
  }
  private void DisplayPasswordRegisterError(string textToDisplay) {
    _registerPasswordErrorText.text = textToDisplay;
    _registerPasswordErrorFade.FadeIn(true);
  }

  private void HideUsernameRegisterError() {
    _registerUsernameErrorFade.FadeOut(false);
  }
  private void HidePasswordRegisterError() {
    _registerPasswordErrorFade.FadeOut(false);
  }

  // TODO: Implement logout

  public void OnClickLogout() {
    _loginManager.Logout();
    _activeAccountDisplayText.text = "Not logged in.";
  }

  // TODO: Figure out if Delete should live in a separate script because of it being on the leaderboard page

  public void OnClickDelete() {
    string password = _deletePassword.text;
    string confirmpassword = _deleteConfirmPassword.text;
    string result = _loginManager.DeleteAccount(name, password);
    if (result == "Passwords did not match.") print(result);
  }
  private void OnClickDeleteCoroutine(string username, string password) {
    StartCoroutine(_loginManager.DeleteAccount(username, password));
  }

}