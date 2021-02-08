using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour {
    public InputGroupComponent emailInput;
    public InputGroupComponent passwordInput;
    public GameObject loadingIndicator;

    public void LoginUser () {
        loadingIndicator.SetActive (true);
        string email = emailInput.inputFieldReference.text;
        string password = passwordInput.inputFieldReference.text;

        User.SignIn (email, password, (success) => {
            // on user auth success
            DataManager.persistedData.userEmail = email;
            DataManager.Save ();
            SceneManager.LoadScene ("2_PreScenarioScene");
        }, () => {
            // on user auth error
            loadingIndicator.SetActive (false);
            // TODO specific messages for errors
            emailInput.DisplayError ("Ein Fehler ist aufgetreten");
            passwordInput.DisplayError ("Ein Fehler ist aufgetreten");
        });
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
