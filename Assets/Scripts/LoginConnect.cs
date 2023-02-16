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

  public string Login(string name, string password) {
    password = GetStringHash(password);
    StartCoroutine(DoLogin(name, password, returnValue => {
      auth = returnValue;
    }));
    if (!auth) return "Failed to login.";
    loggedIn = true;
    activeAccount = name;
    connPassword = password;
  }

  public bool Logout() {
    logggedIn = false;
    connPassword = "";
    activeAccount = "";
  }

  public string Register(string email, string name, string password) {
    password = GetStringHash(password);
    StartCoroutine(DoRegister(email, name, password));
  }

  public string DeleteAccount(string name, string password, string confirmpassword) {
    if (password != confirmpassword) return "Passwords did not match.";
    password = GetStringHash(password);
    StartCoroutine(DoDeleteAccount(name, password));
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