using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;

namespace JJH
{
    // 여러 곳에서 Enum에 접근 예상 --> 클래스 외부에서 enum 선언
    public enum InventoryObjectType
    {
        // 한 손 , 두 손 , 이벤트용 아이템

        OneHandItem , TwoHandItem,  END
    }

    // 이벤트 아이템인지 체크 하는 부분은 나중에 수정하자.
    public enum EventItem
    {
        Non_Event , Event , END
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
        public int itemID; // 이 ID를 이용해서 일치하는 프리팹을 생성해주는 방식으로 할까?
        public string itemName; // 아이템의 이름 
        //public InventoryObjectType objectType; // 아이템의 한 손 , 두 손 , 이벤트용 
        public StackTypeItem stackType; // 겹쳐질 수 있는지의 여부 
        public int slotID; // 슬롯 ID를 저장

        // 실제 오브젝트를 인벤토리에 넣는 상황을 가정하여 물체의 Transform을 저장한다.
        /*public Vector3 originalPosition; // 아이템의 원래 위치 
        public Quaternion originalRotation; // 아이템의 원래 회전 값*/
        public Vector3 originalScale; // 아이템의 원래 스케일 값

        // position 과 roatation 은 굳이 저장할 필요가 없을 듯함. --> Scale만 조정해주기 때문에.
        public void SaveOriginalTransform (Transform transform)
        {
            originalScale = transform.localScale;

        }
        // 물체의 트랜스폼을 원상복구 한다. --> 스케일을 제외한 부분은 저장하면 안될듯 하다. 
        public void RestoreOriginalTransform(Transform transform)
        {
            transform.localScale = originalScale;
            
        }
    }

    [System.Serializable]
    public class InventoryData
    {
        // 아이템의 데이터를 관리할 스크립트 (데이터 전용)

        public List<InvenItem> items = new List<InvenItem>();

        // 아이템을 Add 시에 이 슬롯데이터를 slot에서 item의 id와 count를 저장한다. 
        public List<SlotData> slotDatas = new List<SlotData>();

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


    public struct SlotData
    {
        public int id;
        public int itemCount;

    }
}


