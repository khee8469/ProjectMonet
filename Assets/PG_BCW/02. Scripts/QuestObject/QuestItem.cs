using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class QuestItem : ItemObject
{
    [Header("오브젝트 정보")]
    [SerializeField]
    Rigidbody rb;

    [Tooltip("아이템을 주려는 NPC")]
    [SerializeField]
    int npcID;

    //ItemID 이거 써서 할 수 잇나
    [Tooltip("클리어하려는 퀘스트")]
    [SerializeField]
    int clearQuestID;

    //ItemID 이거 써서 할 수 잇나
    [Tooltip("아이템 전달 범위")]
    [SerializeField]
    int itemDeliveryRange;



    protected override void Awake()
    {
        base.Awake();

        if (rb == null) rb = GetComponent<Rigidbody>();

    }

    //잡았을때 중력과은 키고, 키메마틱은 끄고
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        CheckClearItem();
        rb.useGravity = true;
        rb.isKinematic = false;

    }

    Collider[] hitColliders = new Collider[20];
    private void CheckClearItem()
    {
        // 현재 위치를 중심으로 overlapRadius 반지름 내의 콜라이더를 감지
        int size = Physics.OverlapSphereNonAlloc(transform.position, itemDeliveryRange, hitColliders);

        if (size <= 0)
            return;
        // 감지된 콜라이더들을 순회
        for(int i =0; i< size; i++)
        {
            // NPC 컴포넌트를 가지고 있는지 확인
            NPC npcComponent = hitColliders[i].GetComponent<NPC>();
            if (npcComponent != null)
            {
                // NPC ID가 일치하는지 확인
                if (npcComponent.ID == npcID)
                {
                    // 퀘스트를 클리어 상태로 변경
                    Quest quest = Manager.Quest.GetQuest(clearQuestID);
                    if (quest != null)
                    {
                        quest.ChangeState(QuestState.Clear);
                        Debug.Log($"퀘스트 클리어: {clearQuestID}");
                        //퀘스트 완료시키고 삭제
                        Destroy(gameObject);
                        Debug.Log($"{gameObject.name} : 삭제");
                    }
                    else
                    {
                        Debug.LogError($"퀘스트를 찾을 수 없습니다: {clearQuestID}");
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, itemDeliveryRange);
    }
}
