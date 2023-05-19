using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;

public class LoginConnect : MonoBehaviour
{

  private const string loginURL = "https://csprojectdatabase.000webhostapp.com/login.php";
  private string _activeAccountUsername;
  private string _activeAccountHashedPassword;
  private bool _loggedIn;

  private bool _signInSuccessful;
  private bool _registerSuccessful;
  private bool _deleteSuccessful;

  void Awake() {
    if (GameObject.FindGameObjectsWithTag("ConnectionManager").Length > 1 && !_loggedIn) Destroy(this.gameObject);
    DontDestroyOnLoad(this.gameObject);
    //Uncomment for testing/debugging
    //StartCoroutine(Login("admin", "godhelpme"));
  }

  void Start() {
    //For debugging: uncomment and remove all other database calls.
    //StartCoroutine(GetComponent<LeaderboardConnect>().DoPostScores("admin", "0A1AC6758704F09F8347B214AC2892C4F0BBFCEEEE0359EF99D78388D4D53D54", 11500));
  }

  internal static string GetStringHash(string input) {
    if (String.IsNullOrEmpty(input))
      return String.Empty;
    using (var sha = new System.Security.Cryptography.SHA256Managed()) {
      byte[] textData = System.Text.Encoding.UTF8.GetBytes(input);
      byte[] hash = sha.ComputeHash(textData);
      return BitConverter.ToString(hash).Replace("-", String.Empty);
    }
  }

  public string GetActiveAuthPass() {
    return _activeAccountHashedPassword;
  }

  public string GetActiveAccountName() {
    return _activeAccountUsername;
  }

  public bool IsLoggedIn() {
    return _loggedIn;
  }

  // Login ---------------------------
  // NOTE: Since these are both coroutines, it's possible to merge these. Same goes for register.
  public IEnumerator Login(string username, string password, Action<bool> callback=null) {
    string hashedPassword = GetStringHash(password);
    yield return StartCoroutine(DoLogin(username, hashedPassword, returnValue => {
      _signInSuccessful = returnValue;
    }));
    if (_signInSuccessful == false) {
      callback(false);
      yield break;
    }
    else {
      Debug.Log("Login Connect: Login successfully authenticated!");
      _loggedIn = true;
      _activeAccountUsername = username;
      _activeAccountHashedPassword = hashedPassword;
      callback(true);
    }
  }
  private IEnumerator DoLogin(string username, string password, Action<bool> callback=null) {
    WWWForm form = new WWWForm();
    form.AddField("check_credentials", "true");
    form.AddField("name", username);
    form.AddField("password", password);

    using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form)) {
      yield return www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.responseCode == 401) {
        Debug.Log($"LoginConnect: Error: {www.error}");
        callback(false);
      }
      else {
        //Debug.Log(www.downloadHandler.text);
        callback(true);
      }
    }
  }

  // Logout --------------------------

  public void Logout() {
    _loggedIn = false;
    _activeAccountUsername = "";
    _activeAccountHashedPassword = "";
  }

  // Register ------------------------

  public IEnumerator Register(string email, string username, string password, Action<bool> callback=null) {
    // Confirm check has been moved to AccountInterface
    string hashedPassword = GetStringHash(password);
    yield return StartCoroutine(DoRegister(email, username, hashedPassword, returnValue => _registerSuccessful = returnValue));
    if (_registerSuccessful == false) {
      callback(false);
      yield break;
    }
    else {
      Debug.Log("LoginConnect: Successfully registered account!");
      _loggedIn = true;
      _activeAccountUsername = username;
      _activeAccountHashedPassword = hashedPassword;
      callback(true);
    }
  } 
  private IEnumerator DoRegister(string email, string username, string hashedPassword, Action<bool> callback=null) {
    WWWForm form = new WWWForm();
    form.AddField("register_account", "true");
    form.AddField("name", username);
    form.AddField("email", email);
    form.AddField("password", hashedPassword);

    using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form)) {
      yield return www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.responseCode == 401) {
        Debug.Log($"LoginConnect: Register Account Error: {www.error}");
        callback(false);
      }
      else {
        Debug.Log($"LoginConnect: RegisterAccount downloaded text: {www.downloadHandler.text}");
        callback(true);
      }
    }
  }

  // Delete Account -------------------

  public IEnumerator DeleteAccount(string password, Action<bool> callback=null) {
    // Confirm check has been moved to AccountInterface
    yield return StartCoroutine(DoDeleteAccount(_activeAccountUsername, GetStringHash(password), returnValue => _deleteSuccessful = returnValue));
    if (_deleteSuccessful) {
      Debug.Log("LoginConnect: Successfully deleted account!");
      Logout();
      callback(true);
    }
    else {
      Debug.Log("LoginConnect: Failed to delete account!");
      callback(false);
    }
  }
  private IEnumerator DoDeleteAccount(string name, string hashedPassword, Action<bool> callback=null) {
    WWWForm form = new WWWForm();
    form.AddField("delete_account", "true");
    form.AddField("name", name);
    form.AddField("password", hashedPassword);

    using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form)) {
      yield return www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.responseCode == 401) {
        Debug.Log($"LoginConnect: Delete Account Error: {www.error}");
        callback(false);
      }
      else {
        Debug.Log($"LoginConnect: Delete Account downloaded text: {www.downloadHandler.text}");
        callback(true);
      }
    }
  }
}