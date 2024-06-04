using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jc
{
    public class SliceManager : MonoBehaviour
    {
        [Header("화면을 절단할 카메라 (스크린 캡쳐 용 카메라)")]
        [SerializeField]
        private Camera slicingCamera;

        public static List<Sliceable> sliceables;

        // 카메라 절두체의 정점 위치
        // 우상-우하-좌하-좌상
        // 0~3 near 
        // 4~7 far
        private Vector3[] frustumVertices = new Vector3[8];
    }
}
