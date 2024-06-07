using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [CreateAssetMenu(fileName = "CapturedTexture Data", menuName = "Scriptable Object/CapturedTexture Data", order = int.MinValue)]
    public class CapturedTextureData : ScriptableObject
    {
        [Header("액자 스프라이트")]
        [SerializeField]
        private Sprite sprite;

        [Space(5)]
        [Header("활성화 될 위치")]
        [SerializeField]
        private Vector3 activePos;

        [Space(5)]
        [Header("활성화 될 회전 값")]
        [SerializeField]
        private Quaternion activeRot;
    }
}
