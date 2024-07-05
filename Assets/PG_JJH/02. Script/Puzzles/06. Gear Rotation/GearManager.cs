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
        [SerializeField] private LightPanelButton[] buttonPanels;

        [Tooltip(" 톱니바퀴 모음")]
        [SerializeField] private List<GearObject> GearObjects;


        // OnClear 호출을 update 된 ipuzzle 에서 부르기 때문에 관리를 겹치지 않도록 잘 해줘야한다.
        public override void OnClearPuzzle()
        {
            base.OnClearPuzzle();
            lightHouse.pillar_Of_Light.gameObject.SetActive(true); // 빛 기둥 켜주기.
            
            for(int i=0; i <buttonPanels.Length; i++)
            {
                buttonPanels[i].enabled = true; // 버튼 켜주기.
            }

            GearRotation();

        }

        public void GearRotation()
        {
            for(int i=0;i<GearObjects.Count;i++)
            {
                GearObjects[i].StartRotate(); // 완성 되면 기어의 회전을 시작한다.
            }
        }
        




    }
}


