using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class ItemObject : InteractObject
    {
        [Header("아이템 오브젝트 세팅")]
        [SerializeField]
        private int itemID;
        public int ItemID { get { return itemID; }}
        
        [Header("오브젝트 풀 사이즈")]
        [SerializeField]
        private int pullingSize;
        public int PulllingSize { get { return pullingSize; }}  

        [Space(10)]
        [Header("밸런싱")]
        [Space(5)]
        [SerializeField]
        private Vector3 originScale;

        protected override void Awake()
        {
            base.Awake();
            originScale = transform.localScale; 
        }

        public void ResetScale()
        {
            transform.localScale = originScale;
        }

    }
}
