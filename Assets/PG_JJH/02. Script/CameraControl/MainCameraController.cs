using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MainCameraController : MonoBehaviour
{
    // 흑백 효과를 적용할 그냥 메인 카메라. --> 기본 카메라이고 그냥 기본 카메라로 쓴다.
    // 글로벌 블룸을 받을 카메라. 

    public Camera mainCamera;
    public Camera subCamera;

    // 메인카메라의 블룸 마스크 -> everything 
    // 서브카메라의 블룸 마스크 --> 로컬블룸 

    private void Start()
    {
        subCamera.transform.SetParent(mainCamera.transform);
        subCamera.transform.localPosition = Vector3.zero;
        subCamera.transform.localRotation = Quaternion.identity;

        mainCamera.depth = 0;
        subCamera.depth = 1;

        SetUpSubCamera();
        SetUpMainCamera();
        Manager.UI.CameraInit();
    }

    private void SetUpMainCamera()
    {
        mainCamera.clearFlags = CameraClearFlags.Skybox;
        //mainCamera.cullingMask = LayerMask.GetMask("Everything");

        UniversalAdditionalCameraData mainCamera1 = mainCamera.GetComponent<UniversalAdditionalCameraData>();
        if(mainCamera1 != null )
        {
            mainCamera1.renderPostProcessing = true;
           // mainCamera1.volumeLayerMask = LayerMask.GetMask("GlobalVolume");
        }


    }

    private void Update()
    {
        
    }

    private void SetUpSubCamera()
    {
        // Depth only -> 서브 카메라가 메인 카메라의 정보를 덮어 쓰지 않으면서 메인 카메라의 결과 위에 추가적인 요소를 렌더링한다. 
        subCamera.clearFlags = CameraClearFlags.Depth;
        // 컬링 마스크를 통해 특정 레이어에 있는 객체만 렌더링 가능하다. 
        subCamera.cullingMask = LayerMask.GetMask("LocalVolume");
        subCamera.depth = mainCamera.depth + 1;
        // 0 보다 1이 위에 그려진다. -> 메인 위에 서브 카메라 렌더링을 그리기 위함. 


        UniversalAdditionalCameraData subCamera1 = subCamera.GetComponent<UniversalAdditionalCameraData>();
        if(subCamera != null)
        {
            subCamera1.renderPostProcessing = true;
            //subCamera1.volumeLayerMask = LayerMask.GetMask("LocalVolume");
        }
        


    }


}
