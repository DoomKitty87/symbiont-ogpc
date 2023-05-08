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
  [SerializeField] private string _registerUnsuccessfulError, _registerPasswordIncorrectError, _registerNoCredentialsEnteredError;
  [SerializeField] private FadeElementInOut _registerUsernameErrorFade;
  [SerializeField] private TextMeshProUGUI _registerUsernameErrorText;
  [SerializeField] private FadeElementInOut _registerPasswordErrorFade;
  [SerializeField] private TextMeshProUGUI _registerPasswordErrorText;
  [SerializeField] private FadeElementInOut _registerButtonFade;
  [SerializeField] private Button _registerButton;

  [Header("These will be available once signed in.")]

  [Header("Logout")]
  [SerializeField] private Button _logOutButton;

  [Header("Delete")]
  [SerializeField] private TMP_InputField _deletePassword;
  [SerializeField] private TMP_InputField _deleteConfirmPassword;
  [SerializeField] private string _deleteInputEmptyError, _deletePasswordsNoMatchError, _deletePasswordInvaildError;
  [SerializeField] private FadeElementInOut _deleteErrorFade;
  [SerializeField] private TextMeshProUGUI _deleteErrorText;
  [SerializeField] private FadeElementInOut _deleteButtonFade;
  [SerializeField] private Button _deleteButton;
  [Header("Account Display")]
  [SerializeField] private TextMeshProUGUI _activeAccountDisplayText;

  
  [Header("Events")]
  [SerializeField] private UnityEvent _onLoginSuccess;
  [SerializeField] private UnityEvent _onLogout;

  // Coroutine multicall protection
  private bool _isWaitingForLogin = false;
  private bool _isWaitingForRegister = false;
  private bool _isWaitingForDelete = false;

  // Coroutine callback variables
  private bool _isLoginSuccessful = false;
  private bool _isRegisterSuccessful = false;
  private bool _isDeleteSuccessful = false;

  void Start() {
    _loginManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LoginConnect>();
    _isWaitingForLogin = false;
    _isWaitingForRegister = false;
    _isWaitingForDelete = false;

    _loginButton.onClick.AddListener(OnClickLogin);
    _logOutButton.onClick.AddListener(OnClickLogout);
    _registerButton.onClick.AddListener(OnClickRegister);
    _deleteButton.onClick.AddListener(OnClickDelete);
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
    yield return StartCoroutine(_loginManager.Login(username, password, returnValue => _isLoginSuccessful = returnValue));
    if (_isLoginSuccessful) {
      _activeAccountDisplayText.text = _loginManager.GetActiveAccountName();
      _onLoginSuccess?.Invoke();
    }
    else {
      DisplayLoginError(_loginInvalidError);
      _loginButtonFade.FadeIn(true);
    }
    // Reset coroutine callback
    _isLoginSuccessful = false;
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

  // Logout ---------------------------------

  public void OnClickLogout() {
    _loginManager.Logout();
    _activeAccountDisplayText.text = "Not logged in.";
    _loginUsername.text = "";
    _loginPassword.text = "";
    _registerUsername.text = "";
    _registerPassword.text = "";
    _registerConfirmPassword.text = "";
    _loginButtonFade.FadeIn(false);
    _registerButtonFade.FadeIn(false);
    _onLogout?.Invoke();
  }

  // Register ---------------------------------

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
    StartCoroutine(OnClickRegisterCoroutine(_registerUsername.text, _registerPassword.text));
  }
  private IEnumerator OnClickRegisterCoroutine(string name, string password) {
    _isWaitingForRegister = true;
    
    // TODO: Remove email from database
    string email = "no_email";

    yield return StartCoroutine(_loginManager.Register(email, name, password, returnValue => _isRegisterSuccessful = returnValue));
    if (_isRegisterSuccessful) {
      print("AccountInterface: Recived Register Success");
      _activeAccountDisplayText.text = _loginManager.GetActiveAccountName();
      _onLoginSuccess?.Invoke();
    }
    else {
      print("AccountInterface: Recived Register Error");
      DisplayUsernameRegisterError(_registerUnsuccessfulError);
      _registerButtonFade.FadeIn(true);
    }
    // Reset coroutine callback
    _isRegisterSuccessful = false;
    _isWaitingForRegister = false;
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

  // Delete ---------------------------------

  public void OnClickDelete() {
    _deleteButtonFade.FadeOut(false);
    if (_isWaitingForDelete) return;
    if (_deletePassword.text == "" || _deleteConfirmPassword.text == "") {
      DisplayDeleteError(_deleteInputEmptyError);
      _deleteButtonFade.FadeIn(false);
      return;
    }
    if (_deletePassword.text != _deleteConfirmPassword.text) {
      DisplayDeleteError(_deletePasswordsNoMatchError);
      _deleteButtonFade.FadeIn(false);
      return;
    }
    StartCoroutine(OnClickDeleteCoroutine());
  }
  private IEnumerator OnClickDeleteCoroutine() {
    _isWaitingForDelete = true;
    HideDeleteError();
    yield return StartCoroutine(_loginManager.DeleteAccount(returnValue => _isDeleteSuccessful = returnValue));
    if (_isDeleteSuccessful) {
      _activeAccountDisplayText.text = "Not logged in.";
      _onLogout?.Invoke();
    }
    else {
      DisplayDeleteError(_deletePasswordInvaildError);
      _deleteButtonFade.FadeIn(true);
    }
    // Reset coroutine callback
    _isDeleteSuccessful = false;
    _isWaitingForDelete = false;
  }

  private void DisplayDeleteError(string textToDisplay) {
    _deleteErrorText.text = textToDisplay;
    _deleteErrorFade.FadeIn(true);
  }

  private void HideDeleteError() {
    _deleteErrorFade.FadeOut(false);
  }
}