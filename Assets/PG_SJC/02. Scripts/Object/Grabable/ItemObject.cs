using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public class ItemData
    {
        [Header("아이템 획득 여부")]
        public bool isAccepted;

        [Header("아이템 사용완료 여부")]
        public bool isClear;

        [Header("아이템 ID")]
        public int itemID;

        [Header("아이템 이미지")]
        public Sprite itemSprite;
    }

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

        [SerializeField]
        private ItemData itemData;
        public ItemData ItemData { get { return itemData; } }

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
