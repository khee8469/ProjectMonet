using System.Collections;
using UnityEngine;

using Jc;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
namespace JJH
{
    public class LobbyScene : BaseScene
    {
        [SerializeField] Transform playerStartTr;

        private ColorAdjustments colorAdjustments;
        [SerializeField] private int SceneID = 0;
        [SerializeField] Volume globalVolume;



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

