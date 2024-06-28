using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

namespace Jc
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("유저 메인 캔버스")]
        [SerializeField]
        private Canvas mainCanavas;

        [Header("페이드 이미지")]
        [SerializeField]
        private Image fadeImage;

        [Header("팝업 그룹 (인벤토리/퀘스트)")]
        [SerializeField]
        private GameObject infoGruop;

        [Header("퀘스트 엔트리 프리팹")]
        [SerializeField]
        private QuestEntry questEntryPrefab;
        public QuestEntry QuestEntryPrefab { get { return questEntryPrefab; } }

        // 퀘스트 엔트리 리스트
        private List<QuestEntry> questEntryList = new List<QuestEntry>();

        [Header("퀘스트 엔트리 그룹 트랜스폼")]
        [SerializeField]
        private RectTransform questEntryTr;

        private void OnEnable()
        {
            Debug.Log("UIManager Enable");

            Camera renderCamera = Camera.main;
            if (renderCamera == null) return;

            mainCanavas.worldCamera = renderCamera;
        }

        // 페이드 인
        public IEnumerator FadeInRoutine(float fadeTime = 0f)
        {
            float rate = 0f;
            Color fadeInColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
            Color fadeOutColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
            
            while (rate < 1f)
            {
                rate += Time.deltaTime / fadeTime;
                fadeImage.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
                yield return null;
            }
        }
        
        // 페이드 아웃
        public IEnumerator FadeOutRoutine(float fadeTime = 0f)
        {
            float rate = 0f;
            Color fadeInColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
            Color fadeOutColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);

            while (rate < 1f)
            {
                rate += Time.deltaTime / fadeTime;
                fadeImage.color = Color.Lerp(fadeInColor, fadeOutColor, rate);
                yield return null;
            }
        }

        // 인벤토리/퀘스트 창 열기
        public void OpenInfoGroup()
        {
            infoGruop.transform.parent = Camera.main.transform;
            infoGruop.transform.localPosition = Vector3.zero;
            infoGruop.transform.localRotation = Quaternion.identity;

            // 새롭게 add or remove 되면 켜줄 때 한 번 데이터를 로드한다. 
            if(Manager.Inventory.is_AddRemoveItem == true)
            {
                Manager.Inventory.LoadSlot();
                Manager.Inventory.is_AddRemoveItem = false; 
            }


            infoGruop.SetActive(true);
            Manager.Inventory.isEnable = true;
        }
        // 인벤토리/퀘스트 창 닫기
        public void CloseInfoGroup()
        {
            Manager.PlableData.SaveSlotData();
            infoGruop.transform.parent = this.transform;
            Manager.Inventory.isEnable = false;
            infoGruop.SetActive(false);
        }

        public void CreateEntry(Quest quest)
        {
            // 퀘스트 엔트리 생성
            QuestEntry entry = Instantiate(questEntryPrefab, questEntryTr);
            entry.OwnerQuest = quest;
            entry.InitSetting();

            questEntryList.Add(entry);
        }
        public void RemoveEntry(Quest quest)
        {
            int removeIndex = -1;
            for (int i = 0; i < questEntryList.Count; i++)
            {
                if (questEntryList[i].OwnerQuest != quest) continue;

                Destroy(questEntryList[i].gameObject);
                removeIndex = i;
                break;
            }

            if (removeIndex >= 0)
                questEntryList.RemoveAt(removeIndex);
        }
    }
}