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
            if (Manager.PlableData.paintDataList.Count < 1)
                return;
            foreach (int key in Manager.PlableData.paintDataList.Keys)
            {
                // 나중에 set으로 변경

                // key가 id
                foreach(PaintBucket bucket in buckets)
                {
                    if(key == bucket.PaintItemID)
                    {
                        // 활성화 시켜주기 (스폰시켜주기)
                        bucket.gameObject.SetActive(true); 
                        break;
                    }
                }
            }
        }
        public void CheckOnOff()
        {

        }



    }
}


