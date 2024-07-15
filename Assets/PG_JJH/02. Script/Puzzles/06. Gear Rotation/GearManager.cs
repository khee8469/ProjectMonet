using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using Jc;
using System;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class GearManager : PaintRewardPuzzle
    {
        [Tooltip("불을 켜 줄 등대 ")]
        [SerializeField] private LightHouseHead lightHouse;
        [Tooltip("활성화 시켜 줄 버튼 패널")]
        [SerializeField] private LightPanelButton[] buttonPanels;

        [Tooltip("완성 시 재생될 톱니바퀴 움직이는 소리")]
        [SerializeField] public AudioClip gearRotationSound;

        [Tooltip("기어 소켓 저장")]
        [SerializeField] private GearSocket[] gearSockets;

        [Tooltip("기어 회전을 저장 시키기 위한 List ->실제 삽입 기어 add remove")]
        public List<GearObject> gearList;

        [Tooltip("삽입 되지 않는 장식 기어들의 회전을 위한 저장 list")]
        public List<DecoGearUpdate> decoGearList;
        


        // OnClear 호출을 update 된 ipuzzle 에서 부르기 때문에 관리를 겹치지 않도록 잘 해줘야한다.
        public override void OnClearPuzzle()
        {
            Debug.Log("온 클리어 퍼즐 발동");

            base.OnClearPuzzle();
            lightHouse.pillar_Of_Light.gameObject.SetActive(true); // 빛 기둥 켜주기.  --> 이거 일단 스포트 라이트로 변경 시켜주기. 

            for (int i = 0; i < buttonPanels.Length; i++)
            {
                buttonPanels[i].enabled = true; // 버튼 켜주기. --> 등대의 버튼 들 . 
            }

            StartCoroutine(DelayCoroutine()); // 딜레이 살짝 줘서 트랙 로테이션이 바로 켜지지 않도록 하기. 
            //SoundPlay(); --> 사운드는 어디서 관리 할 지? 
        }

        // onClear 발동 전에 살짝 딜레이 주기 위한 코루틴 (track을 바로 풀어버리면 문제 생겨서 )
        private IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(0.7f);
            foreach (GearObject gear in gearList)
            {
                gear.StartRotate();
                gear.OnNutActive();
                gear.trackRotation = false;
            }
            DecoGearRotate(); // 장식 기어들 회전 
        }

        public void DecoGearRotate()
        {
            foreach (var  gear in decoGearList)
            {
                gear.RotateRoutine(); // 인수 없이 그냥 돌리고 --> 내부에서 direction 값 줘서 회전 시키자. 
            }
        }



        public void SoundPlay()
        {
            // loop 재생 정상적인 상태인지? 
            Manager.Sound.PlaySFXLoop(gearRotationSound);
        }
    }
}


