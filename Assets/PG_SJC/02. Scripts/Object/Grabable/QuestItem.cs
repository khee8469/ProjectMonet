using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class QuestItem : ItemObject
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            // 이미 사용이 완료된 아이템이라면
            if(Manager.PlayableData.itemInfoDataDic.ContainsKey(ItemID))
            {
                if (Manager.PlayableData.itemInfoDataDic[ItemID].isClear)
                    Destroy(gameObject);
            }
        }

    }
}
