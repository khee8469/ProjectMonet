using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jc
{
    public class InventorySlot : MonoBehaviour
    {
        [Header("에디터 세팅")]
        [Space(5)]

        [Header("아이템 이미지")]
        [SerializeField]
        private Image itemContentIMG;

        [Header("호버링 타겟 이미지")]
        [SerializeField]
        private Image hoverIMG;

        [SerializeField]
        private Color hoverColor;
        private Color originColor;

        [Header("아이템 카운팅")]
        [SerializeField]
        private TextMeshProUGUI itemCountTMP;

        [Space(10)]
        [Header("밸런싱")]
        [Space(5)]
        [Header("슬롯에 속한 아이템 ID")]
        [SerializeField]
        private int getItemID = -1;
        // 아이템 이미지 변경
        public int GetItemID
        {
            get { return getItemID; }
            set
            {
                getItemID = value;

                if(getItemID == -1)
                {
                    itemContentIMG.enabled = false;
                    itemContentIMG.sprite = null;
                }
                else
                {
                    if(!Manager.Item.ItemDataDic.ContainsKey(getItemID))
                    {
                        Debug.Log($"{getItemID} 아이템에 대한 데이터가 존재하지 않습니다.");
                        return;
                    }
                    itemContentIMG.enabled = true;
                    itemContentIMG.sprite = Manager.Item.ItemDataDic[getItemID].itemSprite;
                }
            }
        }

        [SerializeField]
        private int itemCount;
        // getItemID 변경
        public int ItemCount
        {
            get { return itemCount; }
            set
            {
                itemCount = value;
                if(itemCount <= 0)
                {
                    itemCount = 0;
                    GetItemID = -1;
                    itemCountTMP.text = "";
                }
                else
                {
                    itemCountTMP.text = $"{itemCount}";
                }
            }
        }

        private Coroutine hoverRoutine;

        private void Awake()
        {
            originColor = hoverIMG.color;
        }

        private void OnDisable()
        {
            hoverIMG.color = originColor;
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
            if(Manager.Item.ItemDataDic[getItemID] == null)
            {
                Debug.Log($"{getItemID}에 해당하는 아이템이 존재하지 않습니다.");
                return null;
            }

            ItemObject item = Instantiate(Manager.Item.ItemDataDic[getItemID].itemPrefab);
            ItemCount--;
            return item;
        }

        #region 레이 인터렉터 호출
        public void OnHoverEnter()
        {
            if(hoverRoutine != null)
            {
                StopCoroutine(hoverRoutine);
                hoverRoutine = null;
            }

            hoverRoutine = StartCoroutine(HoverRoutine(hoverColor));
        }
        public void OnHoverExit()
        {
            if (hoverRoutine != null)
            {
                StopCoroutine(hoverRoutine);
                hoverRoutine = null;
            }

            hoverRoutine = StartCoroutine(HoverRoutine(originColor));
        }
        public void OnClickDown()
        {

        }
        public void OnClickUp()
        {

        }

        IEnumerator HoverRoutine(Color targetColor)
        {
            float rate = 0f;
            Color startColor = hoverIMG.color;
            Color endColor = targetColor;
            while(rate < 1f)
            {
                rate += Time.deltaTime * 3f;
                hoverIMG.color = Color.Lerp(startColor, endColor, rate);
                yield return null;
            }

            hoverRoutine = null;
            yield return null;
        }

        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    OnHoverEnter();
        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{
        //    OnHoverExit();
        //}

        #endregion
    }
}