using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace JJH
{
    public class InventoryController : MonoBehaviour
    {
       // 인벤토리의 열고 닫고를 다른 곳에서 체크 하고 있기 때문에 굳이 여기서?
       // 만약 이벤트가 필요하다면 이제 인벤토리 On Off 이벤트를 달아 주는 식으로 하자. 

        public static UnityEvent<bool> InventoryEvent = new UnityEvent<bool>();

        private void Awake()
        {
            InventoryEvent.AddListener(OpenInventory);
            InventoryEvent.AddListener(CloseInventory);

        }

        private void OpenInventory(bool isOpened)
        {
            // 열었을 때 이벤트가 필요하다면.. 써야겠지?
        }

        // 인벤토리를 닫는 함수 --> event와 연결하여 조작 연계
        private void CloseInventory(bool isOpened)
        {
            // 닫았을 때 이벤트가 필요하다면 사용해야겠지.. 
        }

        // 인벤토리에 직접 Add를 할 때 실패하면 사운드 발생.

    }

}
