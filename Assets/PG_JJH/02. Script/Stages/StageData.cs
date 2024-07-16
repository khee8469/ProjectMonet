using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="DoorS",menuName = "ScriptableObjects/StageObjectable")]
public class StageData :ScriptableObject
{
    public List<bool> stageUnlockStatus = new List<bool>();
    
    // 이거 어차피 이 상태 리스트 그대로 들고 json 으로 바꿔서 
    // 저장하면 아무 문제 없을듯. 

}







