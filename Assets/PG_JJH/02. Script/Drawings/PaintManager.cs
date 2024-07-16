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
            /*Manager.PlableData.paintDataList = new Dictionary<int, bool>();

            Manager.PlableData.paintDataList.Add(3, true);*/
            
            if (Manager.PlayableData.paintDataList == null) return;

            if (Manager.PlayableData.paintDataList.Count < 1)
                return;

            foreach (int key in Manager.PlayableData.paintDataList.Keys)
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


