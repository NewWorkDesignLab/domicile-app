using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserManagementInterfaces : MonoBehaviour {
  [Header ("Login")]
  public GameObject loginGroup;
  public InputField loginEmailField;
  public InputField loginPasswordField;
  public Toggle loginSaveToogle;

  void Start () {
    ShowLoginGroup ();
  }

  public void ButtonSignIn () {
    string email = loginEmailField.text;
    string password = loginPasswordField.text;
    User.SignIn (email, password, (success) => {
      DataManager.getValue.saveLogin = loginSaveToogle.isOn;
      DataManager.getValue.userEmail = email;
      DataManager.instance.Save ();
      SceneManager.LoadScene ("MainMenuScene");
    });
  }

  public void ShowLoginGroup () {
    DisableAllGroups ();
    loginGroup.gameObject.SetActive (true);
  }

  void DisableAllGroups () {
    loginGroup.gameObject.SetActive (false);
  }

  public void OpenBrowserRegister () {
    Application.OpenURL (String.Format ("{0}/benutzer/registrieren", ServerManager.Host ()));
  }
  public void OpenBrowserForgetPassword () {
    Application.OpenURL (String.Format ("{0}/benutzer/passwort/neu", ServerManager.Host ()));
  }
  public void OpenBrowserResendConfirmation () {
    Application.OpenURL (String.Format ("{0}/benutzer/bestaetigen/neu", ServerManager.Host ()));
  }
}
