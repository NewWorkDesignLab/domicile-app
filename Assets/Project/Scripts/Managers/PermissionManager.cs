using System;
using UnityEngine;

public static class PermissionManager {
    public static void ManagePermissions (Action onDenial, Action onGranted) {
        RequestPermission (NativeGallery.PermissionType.Read);
        RequestPermission (NativeGallery.PermissionType.Write);
        if (CheckPermissions ())
            onGranted ();
        else
            onDenial ();
    }

    public static bool CheckPermissions () {
        NativeGallery.Permission permissionRead = NativeGallery.CheckPermission (NativeGallery.PermissionType.Read);
        NativeGallery.Permission permissionWrite = NativeGallery.CheckPermission (NativeGallery.PermissionType.Write);
        return (permissionRead == NativeGallery.Permission.Granted && permissionWrite == NativeGallery.Permission.Granted);
    }

    public static void RequestPermission (NativeGallery.PermissionType permissionType) {
        NativeGallery.Permission permission = NativeGallery.CheckPermission (permissionType);
        if (permission != NativeGallery.Permission.Granted) {
            NativeGallery.RequestPermission (permissionType);
        }
    }

    public static void OpenSettings () {
        if (NativeGallery.CanOpenSettings ()) {
            NativeGallery.OpenSettings ();
        } else {
            Debug.Log("[PermissionManager OpenSettings] Could not open Settings.");
            // TODO: Toast-Manager display Message to open Settings and grant permissions
            // TODO: Text-Manager to get all stringt from one Location (later multi-lang possible)
        }
    }
}
