using System;
using UnityEngine;

[Serializable]
public class User {

    // CHECK TOKENS
    public static void CheckToken (Action<string> onSuccess) {
        CheckToken (onSuccess, () => { });
    }
    public static void CheckToken (Action<string> onSuccess, Action onFailure) {
        ServerManager.GetRequest ("/api/auth/validate_token", "", onSuccess, onFailure);
    }

    // SIGN IN
    public static void SignIn (string email, string password, Action<string> onSuccess) {
        SignIn (email, password, onSuccess, () => { });
    }
    public static void SignIn (string email, string password, Action<string> onSuccess, Action onFailure) {
        var data = new SignInData (email, password);
        string json = JsonUtility.ToJson (data);
        ServerManager.PostRequest ("/api/auth/sign_in", json, onSuccess, onFailure);
    }

    // SIGN OUT
    public static void SignOut (Action<string> onSuccess) {
        SignOut (onSuccess, () => { });
    }
    public static void SignOut (Action<string> onSuccess, Action onFailure) {
        ServerManager.DeleteRequest ("/api/auth/sign_out", "", onSuccess, onFailure);
    }

    // REGISTER
    public static void Register (string email, string password, string passwordConfirmation, Action<string> onSuccess) {
        Register (email, password, passwordConfirmation, onSuccess, () => { });
    }
    public static void Register (string email, string password, string passwordConfirmation, Action<string> onSuccess, Action onFailure) {
        string confirmSuccessUrl = ServerManager.Host ();
        var data = new RegisterData (email, password, passwordConfirmation, confirmSuccessUrl);
        string json = JsonUtility.ToJson (data);
        ServerManager.PostRequest ("/api/auth", json, onSuccess, onFailure);
    }

    // DELETE ACCOUNT
    public static void DeleteAccount (Action<string> onSuccess) {
        DeleteAccount (onSuccess, () => { });
    }
    public static void DeleteAccount (Action<string> onSuccess, Action onFailure) {
        ServerManager.DeleteRequest ("/api/auth", "", onSuccess, onFailure);
    }

    // UPDATE ACCOUNT
    public static void UpdateAccount (string password, string passwordConfirmation, string currentPassword, Action<string> onSuccess) {
        UpdateAccount (password, passwordConfirmation, currentPassword, onSuccess, () => { });
    }
    public static void UpdateAccount (string password, string passwordConfirmation, string currentPassword, Action<string> onSuccess, Action onFailure) {
        var data = new UpdateAccountData (password, passwordConfirmation, currentPassword);
        string json = JsonUtility.ToJson (data);
        ServerManager.PutRequest ("/api/auth", json, onSuccess, onFailure);
    }

    // SEND PASSWORD RESET EMAIL
    public static void SendPasswordReset (string email, string redirectUrl, Action<string> onSuccess) {
        SendPasswordReset (email, redirectUrl, onSuccess, () => { });
    }
    public static void SendPasswordReset (string email, string redirectUrl, Action<string> onSuccess, Action onFailure) {
        var data = new PasswordResetData (email, redirectUrl);
        string json = JsonUtility.ToJson (data);
        ServerManager.PostRequest ("/api/auth/password", json, onSuccess, onFailure);
    }

    // CHANGE USERS PASSWORD
    public static void ChangePassword (string password, string passwordConfirmation, string currentPassword, Action<string> onSuccess) {
        ChangePassword (password, passwordConfirmation, currentPassword, onSuccess, () => { });
    }
    public static void ChangePassword (string password, string passwordConfirmation, string currentPassword, Action<string> onSuccess, Action onFailure) {
        var data = new ChangePasswordData (password, passwordConfirmation, currentPassword);
        string json = JsonUtility.ToJson (data);
        ServerManager.PutRequest ("/api/auth/password", json, onSuccess, onFailure);
    }

    // RESEND ACCOUNT CONFIRMATION EMAIL
    public static void ResendPasswordConfirmation (string email, string redirectUrl, Action<string> onSuccess) {
        ResendPasswordConfirmation (email, redirectUrl, onSuccess, () => { });
    }
    public static void ResendPasswordConfirmation (string email, string redirectUrl, Action<string> onSuccess, Action onFailure) {
        var data = new ResendConfirmationData (email, redirectUrl);
        string json = JsonUtility.ToJson (data);
        ServerManager.PostRequest ("/api/auth/confirmation", json, onSuccess, onFailure);
    }
}

[Serializable]
class SignInData {
    public string email;
    public string password;
    public SignInData (string email, string password) {
        this.email = email;
        this.password = password;
    }
}

[Serializable]
class RegisterData {
    public string email;
    public string password;
    public string password_confirmation;
    public string confirm_success_url;
    public RegisterData (string email, string password, string passwordConfirmation, string confirmSuccessUrl) {
        this.email = email;
        this.password = password;
        this.password_confirmation = passwordConfirmation;
        this.confirm_success_url = confirmSuccessUrl;
    }
}

[Serializable]
class UpdateAccountData {
    public string password;
    public string password_confirmation;
    public string current_password;
    public UpdateAccountData (string password, string passwordConfirmation, string currentPassword) {
        this.password = password;
        this.password_confirmation = passwordConfirmation;
        this.current_password = currentPassword;
    }
}

[Serializable]
class PasswordResetData {
    public string email;
    public string redirect_url;
    public PasswordResetData (string email, string redirectUrl) {
        this.email = email;
        this.redirect_url = redirectUrl;
    }
}

[Serializable]
class ChangePasswordData {
    public string password;
    public string password_confirmation;
    public string current_password;
    public ChangePasswordData (string password, string passwordConfirmation, string currentPassword) {
        this.password = password;
        this.password_confirmation = passwordConfirmation;
        this.current_password = currentPassword;
    }
}

[Serializable]
class ResendConfirmationData {
    public string email;
    public string redirect_url;
    public ResendConfirmationData (string email, string redirectUrl) {
        this.email = email;
        this.redirect_url = redirectUrl;
    }
}
