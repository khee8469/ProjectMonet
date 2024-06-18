using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    // 아이템을 체크 하기 위한 인터페이스 상속 
    public class InventoryItem : InteractObject, IInventory
    {
        // 실제 아이템이 가지고 있을 아이템의 기본적인 id , 타입 등의 데이터 
        public InvenItem itemData; // 실제 아이템의 데이터 --가지고 있어야 데이터 쓸 수 있을듯? 
        private Transform originalParent; // 원래 부모 trasform 


        private void Start()
        {
            InitializeItemData();
        }


        private void InitializeItemData()
        {
            if (itemData == null)
            {
                itemData = new InvenItem();
            }

        }


        // 아이템이 잡혔을 때의 처리 
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            
        }


        // 아이템이 놓이는 순간에 슬롯 안에 있는지 확인.
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            

        }


    }
}



