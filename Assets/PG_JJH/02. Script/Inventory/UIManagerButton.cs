using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;

namespace JJH
{
    public class UIManagerButton : MonoBehaviour
    {
        [SerializeField] Jc.UIManager UIManager;
        [SerializeField] PlayerInteractor playerInteractor;

        // 만약에 ui에 버튼 추가 할 일이 생기면
        // 스크립트를 분리 해서 좀 더 보기 편하게 버튼 이벤트를 모아둘 용도의 스크립트
        public void closePopUpButton()
        {
            UIManager.CloseInfoGroup(); 
        }


        // playerInterActor 에서 Private라 어떻게 접근해야 할지를 모르겠다. 
        public void OnpopUpButton()
        {
            
        }

    }
}


