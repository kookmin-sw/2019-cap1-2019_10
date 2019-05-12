using UnityEngine;
using UnityEngine.UI;

using Dasony.Libs.Android;

public class CheckPermission : MonoBehaviour {

	public Text resultText;
	public GameObject explainPopup;

	Permissions checker;

	// Use this for initialization
	void Start () {
		checker = Permissions.Instance;

		Check(Permissions.READ_CONTACTS);
	}
	
	public void Check (string permission) {
		if(checker != null) {         
			checker.CheckPermission(permission, (_permission, _isGranted) => {
				if (_permission != permission) return;

				string resultString = string.Format("Is '{0}' Granted? {1}", _permission, _isGranted);

                if (resultText != null) {
                    resultText.text = resultString;
                }

				if(!_isGranted) {
					checker.CheckRationale(permission, (__permission, __isExplain) => {
						if (__permission != permission) return;

						if(__isExplain) {
							if (explainPopup != null) explainPopup.gameObject.SetActive(true);
						} else {
							checker.RequestPermissions(new string[1] { __permission }, 0, RequestResult);
						}
					});
				}
			});
		}
	}

	public void Request () {
		checker.RequestPermissions(new string[1] { Permissions.READ_CONTACTS }, 0, RequestResult);

		if (explainPopup != null) explainPopup.gameObject.SetActive(false);
	}

	void RequestResult (PermissionData[] datas) {
		string resultString = string.Format("Is '{0}' Granted? {1}", datas[0].permission, datas[0].granted);

        if (resultText != null) {
            resultText.text = resultString;
        }
    }
}
