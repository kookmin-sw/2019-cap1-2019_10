using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dasony.Libs.Android;

public class Initialize : MonoBehaviour
{
    //Permissions checker;

    //public string[] WantedPermissions = { "android.permission.READ_EXTERNAL_STORAGE",
    //                                     "android.permission.WRITE_EXTERNAL_STORAGE",
    //                                     "android.permission.CAMERA",
    //                                     "android.permission.INTERNET",
    //                                     "android.permission.RECORD_AUDIO" };


    //private string currentPermission;
    //public GameObject canvas_popup;
    //public GameObject canvas_denied;

    //Permissions AndroidPermissionsManager;

    //private void Start()
    //{
    //    if (permissionCheck == null)
    //    {
    //        permissionCheck = new AndroidPermission();
    //        permissionCheck.Init();

    //        permissionCheck.OnCheckExplainAction = OnCheckExplain;
    //        permissionCheck.OnCheckNonExplainAction = OnCheckNonExplain;
    //        permissionCheck.OnCheckAlreadyAction = OnCheckAlready;
    //        permissionCheck.OnCheckFailedAction = OnCheckFailed;

    //        permissionCheck.OnResultAction = OnRequestResult;
    //    }

    //    CheckPermissions();

    //    checker = Permissions.Instance;
    //}

    //// 최초 불려지는 스크립트. 
    //// 다음씬에서 사용되기전에 버튼을 누르면 호출하거나 최초 씬의 start에서 호출하면 됨
    //public void CallPermission()
    //{
    //    bool isAnyFailed = false;

    //    // 한개라도 permission이 제대로 안되있다면 true를 반환함
    //    for (int i = 0; i < WantedPermissions.Length; i++)
    //    {
    //        if (CheckPermissions(WantedPermissions[i]) == false)
    //        {
    //            isAnyFailed = true;
    //            //return;
    //        }
    //    }

    //    if (isAnyFailed)
    //    {
    //        Debug.LogWarning("권한이 없습니다, 권한 승인을 해주세요");
    //        // 퍼미션을 왜 요청하는지 설명하는 팝업을 이 때 ON 시켜주면 됨
    //        // 팝업에는 버튼 하나가 있고 누르면 OnGrantButtonPress()를 호출해야함
    //        canvas_popup.SetActive(true);
    //    }
    //    else
    //    {
    //        // 성공한 부분. 권한을 가지고 하고싶은 일을 하면 됨
    //        Debug.Log("퍼미션 확인 완료..");
    //    }
    //}


    //// 퍼미션을 체크한다.
    //private bool CheckPermissions(string a_permission)
    //{
    //    // 안드로이드가 아니면 ㅍㅊ true를 리턴시킨다.
    //    if (Application.platform != RuntimePlatform.Android)
    //    {
    //        return true;
    //    }

    //    return AndroidPermissionsManager.IsPermissionGranted(a_permission);
    //}

    //// 권한 승인 버튼에 할당합시다.
    //public void OnGrantButtonPress()
    //{

    //    for (int i = 0; i < WantedPermissions.Length; i++)
    //    {
    //        AndroidPermissionsManager.RequestPermission(new[] { WantedPermissions[i] }, new AndroidPermissionCallback(
    //        grantedPermission =>
    //        {
    //            // 권한이 승인 되었다.
    //            CallPermission();
    //        },
    //        deniedPermission =>
    //        {
    //            canvas_denied.SetActive(true);
    //            // 권한이 거절되었다.
    //        },
    //        deniedPermissionAndDontAskAgain =>
    //        {
    //            // 권한이 거절된데다가 다시 묻지마시오를 눌러버렸다.
    //            // 안드로이드 설정창 권한에서 직접 변경 할 수 있다는 팝업을 띄우는 방식을 취해야함. 
    //            canvas_denied.SetActive(true);
    //        }));
    //    }
    //}

    //// 거절했다면 닫는 기능
    //public void PressDeniedCanvasButton()
    //{
    //    canvas_denied.SetActive(false);
    //}





    //public void CheckPermission()
    //{
    //    checker.CheckPermission(Permissions.CAMERA, (permission, granted) =>
    //    {
    //        if (!granted)
    //        {
    //            // rationale 이나 request 구현
    //        }
    //        else
    //        {
    //            // 허가된 상태
    //        }
    //    });
    //}

    //public void CheckPermissionRationale()
    //{
    //    checker.CheckRationale(Permissions.CAMERA, (permission, isExplain) =>
    //    {
    //        if (!isExplain)
    //        {
    //            // 설명할 UI 출력
    //        }
    //        else
    //        {
    //            // 설명 필요없으니까 권한 요청
    //        }
    //    });
    //}

    //public void RequestPermission()
    //{
    //    checker.RequestPermissions(new string[] { Permissions.CAMERA }, 0, (permissionDatas) =>
    //    {
    //        if (permissionDatas != null)
    //        {
    //            if (permissionDatas[0].granted)
    //            {
    //                //1개의 권한을 요청했기에 인덱스 0번 데이터만 비교
    //                //권한 허가됨 권한을 이용한 기능 구현 혹은 메서드호출
    //            }
    //            else
    //            {
    //                //권한 거부됨
    //                //그래서 기능을 사용할 수 없음을 사용자에게 알려주는 UI 출력
    //            }
    //        }
    //        else
    //        {
    //            //권한 허가 요청 실패
    //        }
    //    });
    //}
}
