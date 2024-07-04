using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using Jc;

namespace JJH
{
    public class GearManager : PuzzleManager
    {
        [Tooltip("불을 켜 줄 등대 ")]
        [SerializeField] private LightHouseHead lightHouse;
        [Tooltip("활성화 시켜 줄 버튼 패널")]
        [SerializeField] private LightPanelButton buttonPanel;

        [Tooltip(" 톱니바퀴 모음")]
        [SerializeField] private List<GearObject> GetObjects;



        

    }
}


