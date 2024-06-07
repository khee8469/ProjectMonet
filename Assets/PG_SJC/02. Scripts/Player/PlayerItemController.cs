using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField]
    private InteractObject leftHandItem;    // 왼손 아이템
    [SerializeField]
    private InteractObject rightHandItem;   // 오른손 아이템

    // 플레이어 아이템 Get
    public void GetItem(bool isLeftStick, InteractObject targetObject)
    {
        if (isLeftStick)
            leftHandItem = targetObject;
        else
            rightHandItem = targetObject;
    }
 
    // 플레이어 아이템 Drop
    public void DropItem(bool isLeftStick)
    {
        if (isLeftStick)
            leftHandItem = null;
        else
            rightHandItem = null;

    }

    // 아이템과 상호작용
    public void InteractItem(bool isLeftStick)
    {
        if (isLeftStick && leftHandItem == null) return;
        if (!isLeftStick && rightHandItem == null) return;

        IInteractable itr = isLeftStick ? leftHandItem as IInteractable : rightHandItem as IInteractable;
        if (itr == null) return;

        itr.Interact();
    }
}
