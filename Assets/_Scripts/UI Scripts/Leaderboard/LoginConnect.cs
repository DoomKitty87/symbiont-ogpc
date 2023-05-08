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
  private bool _registerSuccesful;

  void Awake() {
    if (GameObject.FindGameObjectsWithTag("ConnectionManager").Length > 1) Destroy(this.gameObject);
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
  // TODO: Since these are both coroutines, shouldn't we merge these?
  public IEnumerator Login(string name, string password, Action<bool> callback=null) {
    string hashedPassword = GetStringHash(password);
    yield return StartCoroutine(DoLogin(name, hashedPassword, returnValue => {
      _signInSuccessful = returnValue;
    }));
    if (_signInSuccessful == false) {
      callback(false);
      yield break;
    }
    else {
      print("Logged in successfully");
      callback(true);
      _loggedIn = true;
      _activeAccountUsername = name;
      _activeAccountHashedPassword = hashedPassword;
    }
  }
  private IEnumerator DoLogin(string name, string password, Action<bool> callback=null) {
    WWWForm form = new WWWForm();
    form.AddField("check_credentials", "true");
    form.AddField("name", name);
    form.AddField("password", password);

    using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form)) {
      yield return www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.responseCode == 401) {
        Debug.Log($"LoginConnect: Error: {www.error}");
        callback(false);
      }
      else {
        Debug.Log("Successfully authenticated!");
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

  public bool Register(string email, string name, string password) {
    // Confirm check has been moved to AccountInterface
    string hashedPassword = GetStringHash(password);
    StartCoroutine(DoRegister(email, name, hashedPassword, returnValue => { _registerSuccesful = returnValue; }));
    return _registerSuccesful;
  }
  private IEnumerator DoRegister(string email, string name, string hashedPassword, Action<bool> callback=null) {
    WWWForm form = new WWWForm();
    form.AddField("register_account", "true");
    form.AddField("name", name);
    form.AddField("email", email);
    form.AddField("password", hashedPassword);

    using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form)) {
      yield return www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) {
        Debug.Log($"LoginConnect: Register Account Error: {www.error}");
        callback(false);
      }
      else {
        Debug.Log($"LoginConnect: RegisterAccount downloaded text: {www.downloadHandler.text}");
        Debug.Log("LoginConnect: Successfully registered account!");
        _loggedIn = true;
        _activeAccountUsername = name;
        _activeAccountHashedPassword = hashedPassword;
        callback(true);
      }
    }
  }

  // Delete Account -------------------

  public string DeleteAccount(string name, string password) {
    // Confirm check has been moved to AccountInterface
    // TODO: Get rid of password and name parameters; the user is already logged in, so they aren't needed
    string hashedPassword = GetStringHash(password);
    StartCoroutine(DoDeleteAccount(name, hashedPassword));
    Logout();
    return "Account successfully deleted.";
  }
  private IEnumerator DoDeleteAccount(string name, string hashedPassword) {
    WWWForm form = new WWWForm();
    form.AddField("delete_account", "true");
    form.AddField("name", name);
    form.AddField("password", hashedPassword);

    using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form)) {
      yield return www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) {
        Debug.Log(www.error);
      }
      else {
        Debug.Log("Successfully deleted account!");
      }
    }
  }
}