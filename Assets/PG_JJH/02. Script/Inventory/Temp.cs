using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    // << 인벤토리 슬롯 >> 
    // 부르는 입장에서는 Remove 와 Add 하나로 통일을 시킨다.
    // 즉 add 와 remove 내부에서 분기처리를 진행한다.

    // add 와 remove 시에 slot은 그 내부에 가지고 있는 아이템의 정보를 지니고 있기 때문에.
    // 그 아이템의 정보 (id 나 type 등을 체크해서 내부에서 적절한 처리를 진행한다.)

    // 1. 스택형 아이템인지 아닌지 
    // --> 스택형이 아니면 그냥 기존대로 넣으면 빠지는 아이템이 손으로 오는 식으로 진행한다.
    // --> 스택형이라면 textCount를 늘려줘서 스택형이라는 것을 알 수 있게 한다.
    // --> Remove 시에도 스택형 아이템인지 아닌지 확인해서 count 체크와 아이템 삭제 / 생성 등을 진행한다.
    // 2. 한 손 or 두 손 아이템 ( 특히 remove 시에 잡히는게 아니라 바닥으로 떨어저야 한다. )
    // 아이템의 Type이 Event형이라면 (bool 이든 enum이든 ) --> slot에 직접 닿는 것이 아니라
    // 직접 자신의 인벤토리로 들어가야 하고 그 때 인벤토리를 순회 하면서 같은 ID인 아이템이 있는지 확인하고
    // 만약 스택형이라면 그곳에 스택형 처럼 추가 하고 Id가 없거나 스택형이 아니라면 (아마 id가 겹치지는 않을것 같지만)
    // 그냥 빈 slot을 찾아서 그 곳에 넣어준다. 


    
    // 밖에서 부를 수 있는 것은 public 함수 2가지.
    // inventory slot의 OnSelecetedEnterd 에서 1번 --> 어쨋든 RAY와의 상호작용
    // NPC 이벤트와의 연계에서 1번 -->직접 들어가는 ITEM 
    // 부르는 곳은 2번인듯 하다. 
    public void AddItem(InventoryItem item)
    {

    }
    

    public void RemoveItem(InventoryItem item)
    {

    }

    private void StackItemAdd(InventoryItem item)
    {

    }

    private void StackItemRemove(InventoryItem item)
    {

    }

    private void EventItemAdd(InventoryItem item)
    {

    }

    private void EventItemRemove(InventoryItem item)
    {

    }


}
