using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
namespace JJH
{
    public class PaintManager : MonoBehaviour
    {
        public List<PaintBucket> buckets;

        private void OnEnable()
        {
            if (Manager.PlayableData.itemInfoDataDic == null) 
                return;
            if (Manager.PlayableData.itemInfoDataDic.Count < 1)
                return;

            foreach(PaintBucket bucket in buckets)
            {
                int paintItemID = bucket.PaintItemID;

                // 페인트 아이템 정보가 존재할 경우
                if(Manager.PlayableData.itemInfoDataDic.ContainsKey(paintItemID))
                {
                    // 페인트 아이템을 수령하지 않았거나 이미 사용완료(채색완료)된 경우 continue
                    if (!Manager.PlayableData.itemInfoDataDic[paintItemID].isAccepted || Manager.PlayableData.itemInfoDataDic[paintItemID].isClear)
                        continue;

                    // 아이템을 수령했고, 사용완료하지 않은 경우 활성화
                    bucket.gameObject.SetActive(true);
                }
            }
        }
        public void CheckOnOff()
        {
            
        }

        

    }
}


