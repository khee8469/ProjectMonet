using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    // 임시
    // 저장된 데이터 삭제 후 다시 로드
    public void OnClickResetButton()
    {
        // 세이브 폴더 내 파일 삭제
        CSVHelper.Remove(SystemPath.GetPath(DataPath.LocalQuestData));
        Manager.Scene.LoadScene("Lobby");
    }
}
