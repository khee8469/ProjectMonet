using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jc
{
    public class InventorySlot : MonoBehaviour
    {
        [Header("에디터 세팅")]
        [SerializeField]
        private Image itemIMG;

        [Space(10)]
        [Header("밸런싱")]
        [Space(5)]
        [Header("슬롯에 속한 아이템 ID")]
        [SerializeField]
        private int getItemID;
        public int GetItemID
        {
            get { return getItemID; }
            set
            {
                getItemID = value;

                if(getItemID == -1)
                {
                    itemIMG.sprite = null;
                }
                else
                {
                    if(!Manager.Item.ItemDic.ContainsKey(getItemID))
                    {
                        Debug.Log($"{getItemID} 아이템에 대한 데이터가 존재하지 않습니다.");
                        return;
                    }

                    itemIMG.sprite = Manager.Item.ItemDic[getItemID].ItemData.itemSprite;
                }
            }
        }

        [SerializeField]
        private int itemCount;
        public int ItemCount
        {
            get { return itemCount; }
            set
            {
                itemCount = value;
                if(itemCount <= 0)
                {
                    itemCount = 0;
                    getItemID = -1;
                }
            }
        }

        // 아이템 넣기
        public bool PutInItem(ItemObject item)
        {
            // 존재하는 아이템이 없음
            if(getItemID == -1)
            {
                GetItemID = item.ItemID;
                ItemCount = 1;
            }
            // 존재하는 아이템 있음
            else
            {
                if (item.ItemID != getItemID)
                    return false;

                ItemCount++;
            }
            // 아이템 할당 후 삭제
            Destroy(item.gameObject);
            return true;
        }
        // 아이템 빼기
        public ItemObject TakeOutItem()
        {
            if (getItemID == -1)
                return null;
            if(Manager.Item.ItemDic[getItemID] == null)
            {
                Debug.Log($"{getItemID}에 해당하는 아이템이 존재하지 않습니다.");
                return null;
            }

            ItemCount--;
            return Instantiate(Manager.Item.ItemDic[getItemID]);
        }
    }
}