using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;

public class LoginConnect : MonoBehaviour
{

  private const string loginURL = "https://csprojectdatabase.000webhostapp.com/login.php";
  private string connPassword;
  private string activeAccount;
  private bool loggedIn;
  private bool auth;

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
    return connPassword;
  }

  public string GetActiveAccountName() {
    return activeAccount;
  }

  public bool IsLoggedIn() {
    return loggedIn;
  }

  public IEnumerator Login(string name, string password) {
    password = GetStringHash(password);
    yield return StartCoroutine(DoLogin(name, password, returnValue => {
      auth = returnValue;
    }));
    if (!auth) yield break;
    print("Logged in successfully");
    loggedIn = true;
    activeAccount = name;
    connPassword = password;
  }

  public void Logout() {
    loggedIn = false;
    connPassword = "";
    activeAccount = "";
  }

  public string Register(string email, string name, string password, string confirmpassword) {
    if (password != confirmpassword) return "Passwords did not match.";
    password = GetStringHash(password);
    StartCoroutine(DoRegister(email, name, password));
    return "Registered account.";
  }

  public string DeleteAccount(string name, string password, string confirmpassword) {
    if (password != confirmpassword) return "Passwords did not match.";
    password = GetStringHash(password);
    StartCoroutine(DoDeleteAccount(name, password));
    Logout();
    return "Account successfully deleted.";
  }

  private IEnumerator DoLogin(string name, string password, Action<bool> callback=null) {
    WWWForm form = new WWWForm();
    form.AddField("check_credentials", "true");
    form.AddField("name", name);
    form.AddField("password", password);

    using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form)) {
      yield return www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) {
        Debug.Log(www.error);
        callback(false);
      }
      else {
        Debug.Log("Successfully authenticated!");
        callback(true);
      }
    }
  }

  private IEnumerator DoRegister(string email, string name, string password) {
    WWWForm form = new WWWForm();
    form.AddField("register_account", "true");
    form.AddField("name", name);
    form.AddField("email", email);
    form.AddField("password", password);

    using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form)) {
      yield return www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) {
        Debug.Log(www.error);
      }
      else {
        Debug.Log("Successfully registered account!");
      }
    }
  }

  private IEnumerator DoDeleteAccount(string name, string password) {
    WWWForm form = new WWWForm();
    form.AddField("delete_account", "true");
    form.AddField("name", name);
    form.AddField("password", password);

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