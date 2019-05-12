using System;
using UnityEngine;

namespace Dasony.Libs.Android {
	public class Permissions {
		private static Permissions instance;

		public static Permissions Instance {         
			get {
				if (instance == null) {
					instance = new Permissions();
				}

				return instance;
			}
        }

		#if UNITY_ANDROID
		private AndroidJavaObject permissionObject;
        #endif

		private IPermissionResult permissionResult;
        
		private Action<string, bool> checkResult;
		private Action<string, bool> rationaleResult;
		private Action<PermissionData[]> requestResult;

        /*
         * Dangerous Permissions
         */

		public const string READ_CALENDAR           = "android.permission.READ_CALENDAR";
		public const string WRITE_CALENDAR          = "android.permission.WRITE_CALENDAR";

		public const string CAMERA                  = "android.permission.CAMERA";

		public const string READ_CONTACTS           = "android.permission.READ_CONTACTS";
		public const string WRITE_CONTACTS          = "android.permission.WRITE_CONTACTS";
		public const string GET_ACCOUNTS            = "android.permission.GET_ACCOUNTS";

		public const string ACCESS_FINE_LOCATION    = "android.permission.ACCESS_FINE_LOCATION";
		public const string ACCESS_COARSE_LOCATION  = "android.permission.ACCESS_COARSE_LOCATION";

		public const string RECORD_AUDIO            = "android.permission.RECORD_AUDIO"; // Mic

		public const string READ_PHONE_STATE        = "android.permission.READ_PHONE_STATE";
		public const string CALL_PHONE              = "android.permission.CALL_PHONE";
		public const string READ_CALL_LOG           = "android.permission.READ_CALL_LOG";
		public const string WRITE_CALL_LOG          = "android.permission.WRITE_CALL_LOG";
		public const string ADD_VOICEMAIL           = "android.permission.ADD_VOICEMAIL";
		public const string USE_SIP                 = "android.permission.USE_SIP";
		public const string PROCESS_OUTGOING_CALLS  = "android.permission.PROCESS_OUTGOING_CALLS";

		public const string BODY_SENSORS            = "android.permission.BODY_SENSORS";

		public const string SEND_SMS                = "android.permission.SEND_SMS";
		public const string RECEIVE_SMS             = "android.permission.RECEIVE_SMS";
		public const string READ_SMS                = "android.permission.READ_SMS";
		public const string RECEIVE_WAP_PUSH        = "android.permission.RECEIVE_WAP_PUSH";
		public const string RECEIVE_MMS             = "android.permission.RECEIVE_MMS";

		public const string READ_EXTERNAL_STORAGE   = "android.permission.READ_EXTERNAL_STORAGE";
		public const string WRITE_EXTERNAL_STORAGE  = "android.permission.WRITE_EXTERNAL_STORAGE";

		// https://developer.android.com/reference/android/Manifest.permission

		private Permissions() {
			#if UNITY_ANDROID
        	AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        	AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject>("currentActivity");

			permissionResult = new CheckResult();
         
			permissionResult.OnCheckPermissionCallback += (permission, value) => {
				if(checkResult != null) {
					checkResult(permission, value);
				}
			};

			permissionResult.OnCheckPermissionRationaleCallback += (permission, value) => {
				if (rationaleResult != null) {
					rationaleResult(permission, value);
                }
			};

			permissionResult.OnRequestResultCallback += (datas) => {
				if(requestResult != null) {
					requestResult(datas);
				}
			};

			permissionObject = new AndroidJavaObject ("net.dasony.libs.permissions.PermissionsForUnity", activity, permissionResult);
         
			#endif
        }

		public void CheckPermission (string permission, Action<string, bool> result) {
            #if UNITY_ANDROID
			if(result != null) {
				checkResult += result;
			}

        	if(permissionObject != null) {
				permissionObject.Call("CheckPermission", permission);
			}
            #endif
		}

		public void CheckRationale (string permission, Action<string, bool> result) {
            #if UNITY_ANDROID
			if (result != null) {
				rationaleResult += result;
            }

            if (permissionObject != null) {
				permissionObject.Call("CheckPermissionRationale", permission);
            }
            #endif
        }
      
		public void RequestPermissions (string[] permissions, int requestCode, Action<PermissionData[]> result) {
			#if UNITY_ANDROID
			if(result != null) {
				requestResult += result;
			}

            if (permissionObject != null) {
				permissionObject.Call("RequestPermissions", new object[2] { permissions, requestCode });
            }
            #endif
		}   
	}

	public delegate void CheckResultDelegate(string permission, bool value);
	public delegate void RequestResultDelegate(PermissionData[] datas);

	public class CheckResult :
    #if UNITY_ANDROID
	AndroidJavaProxy,
    #endif
	IPermissionResult 

	{
		#if UNITY_ANDROID
		public CheckResult() : base ("net.dasony.libs.permissions.IPermissionResult") { }
        #endif


		public event CheckResultDelegate OnCheckPermissionCallback;
        public event CheckResultDelegate OnCheckPermissionRationaleCallback;
		public event RequestResultDelegate OnRequestResultCallback;

		void onCheckPermissionResult (string permission, bool isGranted) {
			if (OnCheckPermissionCallback != null) OnCheckPermissionCallback(permission, isGranted);
		}

		void onCheckPermissionRationaleResult (string permission, bool isExplain) {
			if (OnCheckPermissionRationaleCallback != null) OnCheckPermissionRationaleCallback(permission, isExplain);
        }

		void onRequestPermissionsResult (AndroidJavaObject data) {
			AndroidJavaObject[] datas = data.Get<AndroidJavaObject[]>("datas");

			if(datas != null) {
				PermissionData[] tmpDatas = new PermissionData[datas.Length];

				for (int i = 0; i < tmpDatas.Length; i++) {
					tmpDatas[i] = new PermissionData(datas[i]);
				}

				if (OnRequestResultCallback != null) {
                    OnRequestResultCallback(tmpDatas);
                }
			}         
        }
	}

	public interface IPermissionResult {
		event CheckResultDelegate OnCheckPermissionCallback;
		event CheckResultDelegate OnCheckPermissionRationaleCallback;
		event RequestResultDelegate OnRequestResultCallback;
	}

	public class PermissionData {
		public string permission;
		public bool granted;

		public PermissionData (AndroidJavaObject o) {
            if (o != null) {
                permission = o.Get<string>("permission");
				granted = o.Get<bool>("granted");
            }
        }
	}
}