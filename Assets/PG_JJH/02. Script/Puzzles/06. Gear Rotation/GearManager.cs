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

        [Tooltip(" 톱니바퀴 프리팹을 참조하고 있는 모음")]
        [field: SerializeField] public List<GearObject> GearObjects { get; set; }

        // 장식용 기어들은 같은 GearObject를 두되 interactionLayer를 id=-1 이면 nothing으로 되게 한다.

        [Tooltip("완성 시 재생될 톱니바퀴 움직이는 소리")]
        [SerializeField] public AudioClip gearRotationSound;

        [Tooltip("실제 기어가 있는지 없는지를 파악 할 List")]
        public List<GearObject> realGears = new List<GearObject>();

        [Tooltip("기어 소켓 저장")]
        [SerializeField] private GearSocket[] gearSockets;

        // OnClear 호출을 update 된 ipuzzle 에서 부르기 때문에 관리를 겹치지 않도록 잘 해줘야한다.
        public override void OnClearPuzzle()
        {
            Debug.Log("온 클리어 퍼즐 발동");

            base.OnClearPuzzle();
            GearRotation();
            lightHouse.pillar_Of_Light.gameObject.SetActive(true); // 빛 기둥 켜주기.

            for (int i = 0; i < buttonPanels.Length; i++)
            {
                buttonPanels[i].enabled = true; // 버튼 켜주기.
            }

            //SoundPlay(); --> 사운드는 어디서 관리 할 지? 
        }

        public void GearRotation()
        {
            for (int i = 0; i < GearObjects.Count; i++) // 둘이 숫자는 어차피 똑같다. 
            {
                if (realGears[i] != null) // 실제 오브젝트가 있는 경우와 아닌 경우를 따로 파악하기
                {
                    GearObjects[i] = realGears[i];
                }
                else // real 이 null 이라면 새롭게 씬을 시작 했을 때 Complete 된 상태이므로 
                {
                    // 프리팹 생성 후 소켓으로 넣어줘야한다. 
                    GearObject obj = Instantiate(GearObjects[i]);

                    for (int j = 0; j < gearSockets.Length; j++)
                    {
                        // 타겟과 일치하는 소켓이라면
                        if (gearSockets[j].TargetGear.itemData.itemID == obj.itemData.itemID)
                        {
                            gearSockets[j].interactionManager.SelectEnter
                                (gearSockets[j] as IXRSelectInteractor, obj as IXRSelectInteractable);
                        }
                    }
                }

                GearObjects[i].StartRotate(); // 완성 되면 기어의 회전을 시작한다.
                // 콜라이더 꺼주기.
                GearObjects[i].GetComponent<Collider>().enabled = false;
            }
        }

        public void SoundPlay()
        {
            // loop 재생 정상적인 상태인지? 
            Manager.Sound.PlaySFXLoop(gearRotationSound);
        }
    }
}


