using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using System;
using UnityEngine.InputSystem;

namespace JJH
{
    public class InventoryController : MonoBehaviour
    {
        [Header("인벤토리 컨트롤 관련")]
        private AudioSource AudioSource;

        /*[Tooltip("아이템이 떨어질 위치를 정하기 위한 player를 참조한다.")]*/

        [Tooltip("인벤토리 On / Off를 위한 bool 변수")]
        private bool isOpen;

        // 인벤토리 아이템을 관리할 컨트롤러 --> 실제 동작 등을 여기서 실행
        // 잡고 있는 도중 , 인벤토리 내에서 아이템을 놓는 상황 등등 event 체크 필요

        // 플레이어에게 붙어서 플레이어의 컨트롤러와 연계 되어 인벤토리를 On / Off 한다. 

        private void Start()
        {
            AudioSource = GetComponent<AudioSource>();
        }

       /* public void OnMenuButton(InputAction.CallbackContext context)
        {
            Debug.Log("인벤토리 키 매핑 성공"); // L키 매핑해뒀음. 

            if (context.performed)
            {
                OpenInventory();
                
            }
        }*/

        // 메인메뉴키와 연계되어 있어서 열고 닫고를 굳이 따로 해 줄 필요가없다. 

        //인벤토리를 여는 함수
        private void OpenInventory()
        {
            // 열었을 때 이벤트가 필요하다면.. 써야겠지?
        }

        // 인벤토리를 닫는 함수 --> event와 연결하여 조작 연계
        private void CloseInventory()
        {
            // 닫았을 때 이벤트가 필요하다면 사용해야겠지.. 
        }

        // 인벤토리에 직접 Add를 할 때 실패하면 사운드 발생.

    }

}
