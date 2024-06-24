using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        // MonoBehavior를 상속하지 않은 일반 c# 클래스는 씬 전환되도 참조가 파괴되지 않는다.
        public InventoryData inventoryData = new InventoryData();

        // 인벤토리의 슬롯을 관리해줄 딕셔너리
        public Dictionary<int, InventorySlot> inventorySlots
            = new Dictionary<int, InventorySlot>();


        // 슬롯 아이디를 자동으로 할당해 주기 위한 변수
        private int currentSlotID = 0;

        public static UnityEvent ExitGame_InventoryEvent = new UnityEvent();

        public bool isEnable { get; set; } = false;

        protected override void Awake()
        {
            // 저장 후 종료 등. 
            base.Awake();
            ExitGame_InventoryEvent.AddListener(ExitGameSave); // 게임 종료시에 인벤토리 저장 필요.

        }

        private void Start()
        {
            LoadInventoryData(); // 싱글턴 이므로 게임 시작시 인벤토리 데이터를 Load 
            RestoreItemInScene();

        }

        // 슬롯을 등록하는 메서드 --> 슬롯과 매니저의 순서를 맞추기 위해서. 딕셔너리의 key를 이용한다. 
        public void RegisterSlot(InventorySlot slot) // 슬롯에서 불러서 id 이용해서 list와 맞춘다.
        {
            if (slot.slotID == -1)
            {
                Debug.Log("Manager의 Register 등록됨");
                slot.slotID = currentSlotID++; // 할당 후 +1 
            }

            if (!inventorySlots.ContainsKey(slot.slotID))
            {
                Debug.Log(" slot 키 할당됨."); // 이게 지금 
                inventorySlots.Add(slot.slotID, slot); //키가 없을 때만 삭제 
            }

        }

        // 슬롯 등록 해제하는 메서드
        public void UnregisterSlot(InventorySlot slot)
        {
            if (inventorySlots.ContainsKey(slot.slotID))
            {
                inventorySlots.Remove(slot.slotID); // 키가 있을 때만 삭제
            }
        }

        // 인벤토리 데이터를 업데이트하는 메서드 --> Socket 에는 첫번째 자식으로 [Attach]가 새성됩니다. 
        public void UpdateInventoryData()
        {
            inventoryData.items.Clear(); // 기존 아이템 리스트를 초기화 (무결성 유지 위해서)
            foreach (var slot in inventorySlots.Values) // value를 통해 InventorySlot의 값을 확인.
            {
                InventoryItem item = null;
                //모든 자식 오브젝트를 순회하여 InventoryItem 찾기
                foreach (Transform child in slot.itemTransform)
                {
                    item = child.GetComponent<InventoryItem>();
                    if (item != null)
                    {
                        break;
                    }
                }
                if (item != null)
                {
                    InvenItem data = new InvenItem
                    {
                        itemID = item.itemData.itemID,
                        itemName = item.itemData.itemName,
                        stackType = item.itemData.stackType,
                        slotID = slot.slotID,
                        itemCount =item.itemData.itemCount

                    };
                    /*Debug.Log(data.itemID);
                    Debug.Log(data.itemName);
                    Debug.Log(data.stackType);
                    Debug.Log(data.slotID);
                    Debug.Log(data.itemCount);*/

                    //여기서 업데이트 된 오리지널 트랜스폼을 save 해버리는 문제가 발생하고 있다. 
                    //data.SaveOriginalTransform(item.transform);
                    inventoryData.items.Add(data); //업데이트 한 값을 리스트에 저장 
                    
                }
            }
            SaveInventoryData(); // JSON으로 저장 --> 업데이트 이후 그 상태 저장. 
        }

        public void SaveInventoryData() // 인벤토리의 데이터 저장
        {
            string json = inventoryData.ToJson(); // return JsonUtility.ToJson(this); 
            PlayerPrefs.SetString("InventoryData", json);
        }

        public void LoadInventoryData() // 인벤토리의 데이터를 json으로 로드함.
        {
            string json = PlayerPrefs.GetString("InventoryData", "{}");
            inventoryData = InventoryData.FromJson(json);
        }

        // 로드한 인벤토리 데이터에 따라 인벤토리에 아이템 생성 및 복원
        public void RestoreItemInScene() // 씬을 전환한 후에 실제 오브젝트 프리팹을 생성해준다. 
        {
            foreach (var item in inventoryData.items) // List에 접근 
            {
                if (inventorySlots.TryGetValue(item.slotID, out InventorySlot slot))
                {
                    // 생성해줄 때 포지션 이랑 로테이션 어떻게 지정해 줄지 고민해야해. --> 자신의 앞에 나와야하니까
                    Debug.Log("씬 전환 혹은 load 시 아이템 복원");
                    GameObject itemObject = InstantiateItem(item);
                    Debug.Log("생성된 슬롯의 이름들" + slot.name);
                    itemObject.transform.SetParent(slot.itemTransform , false); //Id에 맞는 슬롯의 자식으로 들어감.
                    
                    Debug.Log($"생성되는 아이템들의 위치 + {itemObject.transform.position}");
                    // 해당 슬롯의 아이템 카운트 저장 필요함. 
                    slot.ItemCount = item.itemCount;
                    Debug.Log($"아이템 프리팹 생성시의 카운트 ->{item.itemCount}");
                    itemObject.transform.localPosition = Vector3.zero;
                    itemObject.transform.localRotation = Quaternion.identity;
                    //ResizeItemToFitSlot(itemObject.transform, slot); // 슬롯 크기에 맞게 아이템 크기 조정
                    //SetupInteractable(itemObject, item); // grab이 만약 사라지면 다시 붙여줌. 
                    
                }
            }
        }


        private IEnumerator RestoreItemRoutine()
        {
            yield return new WaitForEndOfFrame();
            RestoreItemInScene();
        }

        public void RestoreItem()
        {
            StartCoroutine(RestoreItemRoutine());
        }


        private GameObject InstantiateItem(InvenItem itemData)
        {
            GameObject itemPrefab = Resources.Load<GameObject>($"{itemData.itemID}"); // 이름 맞추기
            if (itemPrefab != null)
            {
                GameObject itemObject = Instantiate(itemPrefab); // 이 부분 위치 조절 필요하다.
                // 실제 아이템은 모두 인벤토리 아이템 스크립트를 가지고 있어햐 하기 때문에.
                InventoryItem inventoryItem = itemObject.GetComponent<InventoryItem>();
                inventoryItem.itemData = itemData;
                return itemObject;
            }
            else
            {
                Debug.LogError($"Failed to load item prefab with ID: {itemData.itemID}");
            }

            return null;
        }
        private void SetupInteractable(GameObject itemObject, InvenItem itemData)
        {
            // 만약 Grab이 사라졌다면 다시 붙여주기 위한 함수. 
            XRGrabInteractable grabInteractable = itemObject.GetComponent<XRGrabInteractable>();
            if (grabInteractable == null)
            {
                grabInteractable = itemObject.AddComponent<XRGrabInteractable>();
            }
        }

        public void ExitGameSave()
        {
            UpdateInventoryData(); // 현재 인벤토리의 상태를 저장한다. 게임종료 또는 저장 후 종료 등에 실시한다.
        }

       
    }
}

