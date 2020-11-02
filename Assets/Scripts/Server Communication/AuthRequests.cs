using UnityEngine;
using System;

public class AuthRequests : MonoBehaviour
{
  // CHECK TOKENS
  public void CheckToken()
  {
    CheckToken((ignore) => { }, (ignore) => { });
  }
  public void CheckToken(Action<string> onFailure)
  {
    CheckToken((ignore) => { }, onFailure);
  }
  public void CheckToken(Action<string> onSuccess, Action<string> onFailure)
  {
    API.instance.GetRequest("/api/auth/validate_token", "", onSuccess, onFailure);
  }


  // SIGN IN
  public void SignIn(string email, string password)
  {
    SignIn(email, password, (ignore) => { }, (ignore) => { });
  }
  public void SignIn(string email, string password, Action<string> onFailure)
  {
    SignIn(email, password, (ignore) => { }, onFailure);
  }
  public void SignIn(string email, string password, Action<string> onSuccess, Action<string> onFailure)
  {
    var data = new SignInData(email, password);
    string json = JsonUtility.ToJson(data);
    API.instance.PostRequest("/api/auth/sign_in", json, onSuccess, onFailure);
  }


  // SIGN OUT
  public void SignOut()
  {
    SignOut((ignore) => { }, (ignore) => { });
  }
  public void SignOut(Action<string> onFailure)
  {
    SignOut((ignore) => { }, onFailure);
  }
  public void SignOut(Action<string> onSuccess, Action<string> onFailure)
  {
    API.instance.DeleteRequest("/api/auth/sign_out", "", onSuccess, onFailure);
  }


  // REGISTER
  public void Register(string email, string password, string passwordConfirmation)
  {
    Register(email, password, passwordConfirmation, (ignore) => { }, (ignore) => { });
  }
  public void Register(string email, string password, string passwordConfirmation, Action<string> onFailure)
  {
    Register(email, password, passwordConfirmation, (ignore) => { }, onFailure);
  }
  public void Register(string email, string password, string passwordConfirmation, Action<string> onSuccess, Action<string> onFailure)
  {
    string confirmSuccessUrl = API.instance.host;
    var data = new RegisterData(email, password, passwordConfirmation, confirmSuccessUrl);
    string json = JsonUtility.ToJson(data);
    API.instance.PostRequest("/api/auth", json, onSuccess, onFailure);
  }


  // DELETE ACCOUNT
  public void DeleteAccount()
  {
    DeleteAccount((ignore) => { }, (ignore) => { });
  }
  public void DeleteAccount(Action<string> onFailure)
  {
    DeleteAccount((ignore) => { }, onFailure);
  }
  public void DeleteAccount(Action<string> onSuccess, Action<string> onFailure)
  {
    API.instance.DeleteRequest("/api/auth", "", onSuccess, onFailure);
  }


  // UPDATE ACCOUNT
  public void UpdateAccount(string password, string passwordConfirmation, string currentPassword)
  {
    UpdateAccount(password, passwordConfirmation, currentPassword, (ignore) => { }, (ignore) => { });
  }
  public void UpdateAccount(string password, string passwordConfirmation, string currentPassword, Action<string> onFailure)
  {
    UpdateAccount(password, passwordConfirmation, currentPassword, (ignore) => { }, onFailure);
  }
  public void UpdateAccount(string password, string passwordConfirmation, string currentPassword, Action<string> onSuccess, Action<string> onFailure)
  {
    var data = new UpdateAccountData(password, passwordConfirmation, currentPassword);
    string json = JsonUtility.ToJson(data);
    API.instance.PutRequest("/api/auth", json, onSuccess, onFailure);
  }


  // SEND PASSWORD RESET EMAIL
  public void SendPasswordReset(string email, string redirectUrl)
  {
    SendPasswordReset(email, redirectUrl, (ignore) => { }, (ignore) => { });
  }
  public void SendPasswordReset(string email, string redirectUrl, Action<string> onFailure)
  {
    SendPasswordReset(email, redirectUrl, (ignore) => { }, onFailure);
  }
  public void SendPasswordReset(string email, string redirectUrl, Action<string> onSuccess, Action<string> onFailure)
  {
    var data = new PasswordResetData(email, redirectUrl);
    string json = JsonUtility.ToJson(data);
    API.instance.PostRequest("/api/auth/password", json, onSuccess, onFailure);
  }


  // CHANGE USERS PASSWORD
  public void ChangePassword(string password, string passwordConfirmation, string currentPassword)
  {
    ChangePassword(password, passwordConfirmation, currentPassword, (ignore) => { }, (ignore) => { });
  }
  public void ChangePassword(string password, string passwordConfirmation, string currentPassword, Action<string> onFailure)
  {
    ChangePassword(password, passwordConfirmation, currentPassword, (ignore) => { }, onFailure);
  }
  public void ChangePassword(string password, string passwordConfirmation, string currentPassword, Action<string> onSuccess, Action<string> onFailure)
  {
    var data = new ChangePasswordData(password, passwordConfirmation, currentPassword);
    string json = JsonUtility.ToJson(data);
    API.instance.PutRequest("/api/auth/password", json, onSuccess, onFailure);
  }


  // RESEND ACCOUNT CONFIRMATION EMAIL
  public void ResendPasswordConfirmation(string email, string redirectUrl)
  {
    ResendPasswordConfirmation(email, redirectUrl, (ignore) => { }, (ignore) => { });
  }
  public void ResendPasswordConfirmation(string email, string redirectUrl, Action<string> onFailure)
  {
    ResendPasswordConfirmation(email, redirectUrl, (ignore) => { }, onFailure);
  }
  public void ResendPasswordConfirmation(string email, string redirectUrl, Action<string> onSuccess, Action<string> onFailure)
  {
    var data = new ResendConfirmationData(email, redirectUrl);
    string json = JsonUtility.ToJson(data);
    API.instance.PostRequest("/api/auth/confirmation", json, onSuccess, onFailure);
  }
}




[Serializable]
class SignInData
{
  public string email;
  public string password;
  public SignInData(string email, string password)
  {
    this.email = email;
    this.password = password;
  }
}

[Serializable]
class RegisterData
{
  public string email;
  public string password;
  public string password_confirmation;
  public string confirm_success_url;
  public RegisterData(string email, string password, string passwordConfirmation, string confirmSuccessUrl)
  {
    this.email = email;
    this.password = password;
    this.password_confirmation = passwordConfirmation;
    this.confirm_success_url = confirmSuccessUrl;
  }
}

[Serializable]
class UpdateAccountData
{
  public string password;
  public string password_confirmation;
  public string current_password;
  public UpdateAccountData(string password, string passwordConfirmation, string currentPassword)
  {
    this.password = password;
    this.password_confirmation = passwordConfirmation;
    this.current_password = currentPassword;
  }
}

[Serializable]
class PasswordResetData
{
  public string email;
  public string redirect_url;
  public PasswordResetData(string email, string redirectUrl)
  {
    this.email = email;
    this.redirect_url = redirectUrl;
  }
}

[Serializable]
class ChangePasswordData
{
  public string password;
  public string password_confirmation;
  public string current_password;
  public ChangePasswordData(string password, string passwordConfirmation, string currentPassword)
  {
    this.password = password;
    this.password_confirmation = passwordConfirmation;
    this.current_password = currentPassword;
  }
}

[Serializable]
class ResendConfirmationData
{
  public string email;
  public string redirect_url;
  public ResendConfirmationData(string email, string redirectUrl)
  {
    this.email = email;
    this.redirect_url = redirectUrl;
  }
}
