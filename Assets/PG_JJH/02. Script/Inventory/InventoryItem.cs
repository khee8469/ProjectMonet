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
        [Header("아이템이 가지고 있을 데이터")]
        public InvenItem itemData; // 실제 아이템의 데이터 --가지고 있어야 데이터 쓸 수 있을듯? 
        private Transform originalParent; // 원래 부모 trasform 

        [Tooltip("현재 그랩되어 있는지를 확인하는 bool 변수")]
        private bool isGraped;

        public bool ISGraped { get { return isGraped; } }

        private void Start()
        {
            InitializeItemData();
            isGraped = false;
        }


        private void InitializeItemData()
        {
            if (itemData == null)
            {
                itemData = new InvenItem();
                Debug.Log("itemData가 null이어서 새로운 InvenItem 인스턴스를 생성했습니다.");
            }
        }

        //XR interaction을 똑같이 상속 하기 때문에 소켓에 닿앗을 때도 발동하는 문제가 발생한다. 

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            // 상호작용이 그랩 인터랙터에 의한 것인지 확인
            if (args.interactorObject is XRGrabInteractable) // IS로 형변환 체크 가능하면 TRUE RETURN 
            {
                isGrabbed = true;
                Debug.Log($"{gameObject.name} 의 그랩이 true 상태로 변경됨");
            }
        }

        // 아이템이 놓이는 순간에 슬롯 안에 있는지 확인.
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            if (args.interactorObject is XRGrabInteractable)
            {
                StartCoroutine(ExitRoutine()); // 0.7초 딜레이 
                isGrabbed = false;
                Debug.Log($"{gameObject.name} 의 그랩이 false상태로변경됨");
            }
        }

        private IEnumerator ExitRoutine()
        {
            yield return new WaitForSeconds(0.7f);
        }


    }
}



