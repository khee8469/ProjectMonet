using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureMove : MonoBehaviour
{
    
    [SerializeField] Npc cube;  //동기화되는 오브젝트
    public Npc Cube { get { return cube; } set { cube = value; } }
    [SerializeField] float multiple;  //실제크기와 몇배차이인지
    Vector3 startPos;
    Vector3 endPos;


    public void GetPosition()
    {
        startPos = transform.position;
    }

    public void SetPosition()
    {
        endPos = transform.position;
        //미니어처 놓았을 때 높이와 회전 고정
        transform.position = new Vector3(endPos.x, 0.5f, endPos.z);
        transform.rotation = Quaternion.identity;

        //위치 동기화
        Vector3 b = new Vector3(endPos.x - startPos.x,0, endPos.z - startPos.z);
        cube.transform.position = cube.transform.position + (multiple * b);
    }
}
