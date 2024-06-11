using UnityEngine;
using JJH;

/// <summary>
/// This script creates a trail at the location of a gameobject with a particular width and color.
/// </summary>

namespace JJH
{
    public class CreateTrail : MonoBehaviour
    {
        // 붓에 붙어야하는 스크립트 

        public GameObject trailPrefab = null; // 트레일렌더러 프리팹은 인스펙터차에서 할당됨. 

        [SerializeField] bool isPainting;

        // width와 color를 내부에서 관리하고 있는데 이 부분을 
        // 물감 오브젝트와 연계하여 color를 변경시켜주는 방식으로 진행해보자. 
        private float width = 0.005f;
        private Color color = Color.black;

        private GameObject currentTrail = null;

        // 누르고 잇는 상태 일때만 나오도록 해야함 (마우스 좌클릭 ) 


        public void StartTrail()
        {
            if (!currentTrail)
            {
                currentTrail = Instantiate(trailPrefab, transform.position, transform.rotation, transform);
                ApplySettings(currentTrail);
            }
        }

        private void ApplySettings(GameObject trailObject)
        {
            TrailRenderer trailRenderer = trailObject.GetComponent<TrailRenderer>();
            trailRenderer.widthMultiplier = width;
            trailRenderer.startColor = color;
            trailRenderer.endColor = color;
        }

        public void EndTrail()
        {
            if (currentTrail)
            {
                currentTrail.transform.parent = null;
                currentTrail = null;
            }
        }

        public void SetWidth(float value)
        {
            width = value;
        }

        public void SetColor(Color value)
        {
            color = value;
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.layer==31)
            {
                Debug.Log("그림판 진입");
                isPainting = true;
            }

            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 31)
            {
                Debug.Log("그림판 벗어남");
                isPainting = false;
            }
        }


    }

}
