using System;
using UnityEngine;

public static class PermissionManager {
    public static void ManagePermissions (Action onDenial, Action onShouldAsk, Action onGranted) {
        RequestPermission (NativeGallery.PermissionType.Read);
        RequestPermission (NativeGallery.PermissionType.Write);
        switch (CheckPermissions ()) {
            case NativeGallery.Permission.Granted:
                onGranted ();
                break;
            case NativeGallery.Permission.ShouldAsk:
                onShouldAsk ();
                break;
            case NativeGallery.Permission.Denied:
                onDenial ();
                break;
            default:
                onDenial ();
                break;
        }
    }

    public static NativeGallery.Permission CheckPermissions () {
        NativeGallery.Permission permissionRead = NativeGallery.CheckPermission (NativeGallery.PermissionType.Read);
        NativeGallery.Permission permissionWrite = NativeGallery.CheckPermission (NativeGallery.PermissionType.Write);
        if (permissionRead == NativeGallery.Permission.Granted && permissionWrite == NativeGallery.Permission.Granted) {
            return NativeGallery.Permission.Granted;
        } else if (permissionRead == NativeGallery.Permission.ShouldAsk || permissionWrite == NativeGallery.Permission.ShouldAsk) {
            return NativeGallery.Permission.ShouldAsk;
        } else {
            return NativeGallery.Permission.Denied;
        }
    }

    public static void RequestPermission (NativeGallery.PermissionType permissionType) {
        NativeGallery.Permission permission = NativeGallery.CheckPermission (permissionType);
        if (permission != NativeGallery.Permission.Granted) {
            NativeGallery.RequestPermission (permissionType);
        }
    }

    public static void OpenSettings (Action failure) {
        if (NativeGallery.CanOpenSettings ()) {
            NativeGallery.OpenSettings ();
        } else {
            Debug.Log ("[PermissionManager OpenSettings] Could not open Settings.");
            failure.Invoke ();
        }
    }
}
