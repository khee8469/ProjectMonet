using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Plate : MonoBehaviour
{
    //몇개의 소켓이 사용중인지
    private int hasSockets;
    //성공확인용
    private bool isSuccess;
    public bool IsSuccess { get { return isSuccess; } set { isSuccess = value; } }

    
    public void SelectSocket()
    {
        hasSockets++;
        if (hasSockets > 2)
        {
            isSuccess = true;
            Debug.Log("접시 담기 성공");
        }
    }
}
