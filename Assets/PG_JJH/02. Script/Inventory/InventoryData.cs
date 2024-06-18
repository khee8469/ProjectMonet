using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using UnityEngine.Experimental.GlobalIllumination;

namespace JJH
{
    // 여러 곳에서 Enum에 접근 예상 --> 클래스 외부에서 enum 선언ㄴ
    public enum InventoryObjectType
    {
        // 한 손 , 두 손 , 이벤트용 아이템

        OneHandItem , TwoHandItem, EventItem , END
    }

    public enum StackTypeItem
    {
        // 겹쳐질 수 있는 아이템 (수량이 있음) / 수량이 없는 아이템 --> 다음 위치에 넣어줘야함. 
        
        Non_Stack , Stackable , END
    }

    [System.Serializable]
    public class InvenItem
    {
        //실제 아이템들이 가지고 있을 정보를 저장한다. 
        public int itemID;
        public string itemName;
        public InventoryObjectType objectType;
        public StackTypeItem stackType;

        // 실제 오브젝트를 인벤토리에 넣는 상황을 가정하여 물체의 Transform을 저장한다.
        public Vector3 originalPosition;
        public Quaternion originalRotation;
        public Vector3 originalScale;



        public void SaveOriginalTransform (Transform transform)
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            originalScale = transform.localScale; // 스케일은 로컬 스케일. 

        }

        // 물체의 트랜스폼을 원상복구 한다. 
        public void RestoreOriginalTransform(Transform transform)
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            transform.localScale = originalScale;
        }


    }

    [System.Serializable]
    public class InventoryData
    {
        // 아이템의 데이터를 관리할 스크립트 (데이터 전용)

        public List<InvenItem> items = new List<InvenItem>();

        public string ToJson()
        {
            return JsonUtility.ToJson(this); // this -> 인벤토리 데이터 스크립트를 의미.
            // ToJson 내부적으로 null 일 때 빈 문자열을 return 해주도록 되어있다. 
        }

        public static InventoryData FromJson(string json)
        {
            return JsonUtility.FromJson<InventoryData>(json);
        }


    }
}


