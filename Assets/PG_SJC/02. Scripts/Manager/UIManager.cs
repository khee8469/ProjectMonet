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

        // 퀘스트 엔트리 딕셔너리
        private Dictionary<int, QuestEntry> questEntryDic = new Dictionary<int, QuestEntry>();

        [Header("퀘스트 엔트리 그룹 트랜스폼")]
        [SerializeField]
        private RectTransform questEntryTr;

        private void Start()
        {
            infoGruop.SetActive(false);
        }

        public void CameraInit()
        {
            Camera renderCamera = Camera.main;
            Debug.Log(renderCamera);
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

            infoGruop.SetActive(true);
        }
        // 인벤토리/퀘스트 창 닫기
        public void CloseInfoGroup()
        {
            //Manager.PlableData.SaveSlotData();
            infoGruop.transform.parent = this.transform;
            infoGruop.SetActive(false);
        }

        public void CreateEntry(Quest quest)
        {
            if (questEntryDic.ContainsKey(quest.QuestID))
                return;

            // 퀘스트 엔트리 생성
            QuestEntry entry = Instantiate(questEntryPrefab, questEntryTr);
            entry.OwnerQuest = quest;
            entry.entryID = quest.QuestID;
            entry.InitSetting();

            questEntryDic.Add(quest.QuestID, entry);
        }
        public void RemoveEntry(Quest quest)
        {
            if (!questEntryDic.ContainsKey(quest.QuestID))
                return;

            Destroy(questEntryDic[quest.QuestID]);
            questEntryDic.Remove(quest.QuestID);
        }
    }
}