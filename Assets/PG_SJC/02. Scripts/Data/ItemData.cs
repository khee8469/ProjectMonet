using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jc
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
    public class ItemData : ScriptableObject
    {
        [Header("아이템 ID")]
        public int itemID;

        [Header("아이템 명")]
        public string itemName;

        [Header("아이템 이미지")]
        public Sprite itemSprite;

        [Header("아이템 프리팹")]
        public ItemObject itemPrefab;
    }
}
