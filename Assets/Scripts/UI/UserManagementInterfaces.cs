using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserManagementInterfaces : MonoBehaviour
{
  [Header("Login")]
  public GameObject loginGroup;
  public InputField loginEmailField;
  public InputField loginPasswordField;
  public Toggle loginSaveToogle;

  // [Header("Register")]
  // public GameObject registerGroup;
  // public InputField registerEmailField;
  // public InputField registerPasswordField;
  // public InputField registerPasswordConfirmationField;

  // [Header("Forget Password")]
  // public GameObject forgetPasswordGroup;
  // public InputField forgetPasswordEmailField;

  // [Header("Resend Confirmation")]
  // public GameObject resendConfirmationGroup;
  // public InputField resendConfirmationEmailField;

  void Start()
  {
    ShowLoginGroup();
  }

  public void ButtonSignIn()
  {
    string email = loginEmailField.text;
    string password = loginPasswordField.text;
    ServerManager.auth.SignIn(email, password, (success) =>
    {
      DataManager.getValue.saveLogin = loginSaveToogle.isOn;
      DataManager.getValue.userEmail = email;
      DataManager.instance.Save();
      Debug.Log("Sign in successfull: " + success);
      SceneManager.LoadScene("MainMenuScene");
    }, (error) =>
    {
      Debug.LogError("Sign in Error: " + error);
    });
  }
  public void ButtonRegister()
  {
    // string email = registerEmailField.text;
    // string password = registerPasswordField.text;
    // string passwordConfirmation = registerPasswordConfirmationField.text;
    // ServerManager.auth.Register(email, password, passwordConfirmation, (success) =>
    // {
    //   Debug.Log("Register successfull: " + success);
    //   ShowLoginGroup();
    // }, (error) =>
    // {
    //   Debug.LogError("Register Error: " + error);
    // });
  }
  public void ButtonResetPassword()
  {

  }
  public void ButtonResendConfirmation()
  {

  }

  public void ShowRegisterGroup()
  {
    // DisableAllGroups();
    // registerGroup.gameObject.SetActive(true);
  }
  public void ShowLoginGroup()
  {
    DisableAllGroups();
    loginGroup.gameObject.SetActive(true);
  }
  public void ShowForgetPasswordGroup()
  {
    // DisableAllGroups();
    // forgetPasswordGroup.gameObject.SetActive(true);
  }
  public void ShowResendConfirmationGroup()
  {
    // DisableAllGroups();
    // resendConfirmationGroup.gameObject.SetActive(true);
  }
  void DisableAllGroups()
  {
    loginGroup.gameObject.SetActive(false);
    // registerGroup.gameObject.SetActive(false);
    // forgetPasswordGroup.gameObject.SetActive(false);
    // resendConfirmationGroup.gameObject.SetActive(false);
  }


  public void OpenBrowserRegister()
  {
    Application.OpenURL(String.Format("{0}/benutzer/registrieren", ServerManager.instance.host));
  }
  public void OpenBrowserForgetPassword()
  {
    Application.OpenURL(String.Format("{0}/benutzer/passwort/neu", ServerManager.instance.host));
  }
  public void OpenBrowserResendConfirmation()
  {
    Application.OpenURL(String.Format("{0}/benutzer/bestaetigen/neu", ServerManager.instance.host));
  }
}
