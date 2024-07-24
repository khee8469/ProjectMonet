using System.Collections;
using UnityEngine;

using Jc;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
namespace JJH
{
    public class LobbyScene : BaseScene  // 로비는 뭐 딱히 볼륨 안쓰니까 
    {
        [Header("로비는 딱히 뭐 건드릴거 없는듯함")]
        [SerializeField] Transform playerStartTr;

        private ColorAdjustments colorAdjustments;
        [SerializeField] private int SceneID = 0;
        [SerializeField] Volume globalVolume;

        [Tooltip("혹시 로비 씬 에서 켜지는 거를 방지할 포워드렌더러")]
        [SerializeField] private UniversalRendererData data;

        private const string renderFeatureName = "OilPaint";

        private void Start()
        {
            if (data != null) // 포워드 렌더러가 Null이 아니라면 모네풍 적용 
            {
                foreach (var feature in data.rendererFeatures)
                {
                    if (feature.name == renderFeatureName)
                    {
                        feature.SetActive(false);  // 로비씬에서는 무조건 꺼주기. 
                    }
                }
            }
        }

        public override IEnumerator LoadingRoutine()
        {
            /*if(Manager.Scene.PlayerObject !=null)
            {
                Debug.Log("플레이어 위치 설정");
                Manager.Scene.PlayerObject.GetComponent<CharacterController>().enabled = false;
                Manager.Scene.PlayerObject.transform.position = playerStartTr.position;
                Manager.Scene.PlayerObject.transform.rotation = playerStartTr.rotation; 
                Manager.Scene.PlayerObject.GetComponent<CharacterController>().enabled = false;
                Manager.Scene.PlayerObject.GetComponent<CharacterController>().enabled = true;
            }
            else
            {
                Debug.Log("플레이어가 설정되지 않았습니다.");
            }
*/

            yield return null;

        }

    }

}

