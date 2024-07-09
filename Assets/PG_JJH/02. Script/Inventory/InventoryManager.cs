using Jc;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [SerializeField]
        private Dictionary<int, SlotData> inventorySlotDatas = new Dictionary<int, SlotData>();
        public Dictionary<int, SlotData> InventorySlotDatas { get { return inventorySlotDatas; } }


        // MonoBehavior를 상속하지 않은 일반 c# 클래스는 씬 전환되도 참조가 파괴되지 않는다.
        // 일단 new 생성 빼기는 했는데.. 빼는게 맞나?> 싱글턴인디.. 
        public InventoryData inventoryData;

        [Tooltip("인벤토리 없이 아이템 추가 시 반복을 방지하기 위한 check 변수")]
        public static int check;

        [Tooltip("pop up을 open 할 때 체크 해 줄 변수 ")]
        public bool is_AddRemoveItem { get; set; } 

        // 슬롯 아이디를 자동으로 할당해 주기 위한 변수
        //private int currentSlotID = 0;

        public static UnityEvent ExitGame_InventoryEvent = new UnityEvent();

        [SerializeField]
        private int maxSlotCount;
        public bool isEnable { get; set; } = true;

        // 슬롯을 등록하는 메서드 --> 슬롯과 매니저의 순서를 맞추기 위해서. 딕셔너리의 key를 이용한다. 
        public void RegisterSlot(InventorySlot slot) // 슬롯에서 불러서 id 이용해서 list와 맞춘다.
        {
            if (!InventorySlotDatas.ContainsKey(slot.slotID))
            {
                SlotData slotData = new SlotData();
                slotData.id_slot = slot.slotID;
                slotData.id_item = slot.ItemID;
                slotData.count = slot.ItemCount;

                InventorySlotDatas.Add(slot.slotID, slotData);
                maxSlotCount--;
                if(maxSlotCount <= 0)
                {
                    Manager.UI.CloseInfoGroup();
                }
            }
        }

        // Load slot 대신에 slot dictionary 를 읽어서 내 인벤토리 상태를 update 해 줄 함수를 만들자.
        // csv 를 지속적으로 update 한다고 생각하지 말고 게임 중에는 저장된 딕셔너리에서 저장된 값을 불러와서 아이템을 ADD 해주는 방식으로 수정해야 한다.

        public void AddItem(int _itemID, int itemCount = 1) // npc가 넣어주는 아이템 관리 
        {
            // 지금 모든 슬롯에 들어 가고 있음 -> 하나만 하고 나가야함.             
            foreach (int key in InventorySlotDatas.Keys)
            {
                //if (check > 0) break;  --> 이미 아래에서 break 하는데 해 줄 필요없지.. 
                if (InventorySlotDatas[key].id_item == -1)
                {
                    SlotData temp = new SlotData();
                    temp.id_item = _itemID;
                    temp.id_slot = key;
                    temp.count = itemCount;
                    InventorySlotDatas[key] = temp;
                    break; // 한 슬롯에서만 생성해 줘야함. 넣어 줄 때. 
                }
                // 슬롯 한 개 에만 add 해줘야하고 추가로 소켓에 제대로 들어가야한다. 
                // 야 이거 싱글턴에서 부르는건대 왜 여러개 들어가냐? 말이 안되는데 ?? 
            }

            Debug.Log($"ADD ITEM 시에 변수 상태{is_AddRemoveItem} ");
        }

        /*public void LoadSlot()
        {
            Manager.PlableData.InitSlot();

            foreach (SlotData slotData in Manager.PlableData.InventorySlotDatas)
            {
                if (slotData.id_item == -1) continue;

                InventorySlot slot = Manager.Inventory.inventorySlots[slotData.id_slot];
                InventoryItem item = Instantiate(Manager.Inventory.itemPrefabDic[slotData.id_item]);

                Debug.Log("item prefab 생성" + item.name);

                // 이 해당 슬롯에 이제 해당하는 item id 값을 가진 프리팹을 붙여준다.
                slot.SetRayHovering(true);
                slot.interactionManager.SelectEnter(slot as IXRSelectInteractor, item as IXRSelectInteractable);
                slot.AddItem(item);
                for (int i = 0; i < slotData.count; i++)
                {
                    slot.AddItemNumber();
                }
            }

            Manager.UI.CloseInfoGroup();
        }
*/

    }
}

