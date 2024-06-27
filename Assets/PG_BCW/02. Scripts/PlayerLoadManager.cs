using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadManager : MonoBehaviour
{
    private static PlayerLoadManager instance;
    public static PlayerLoadManager Instance {  get { return instance; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 이 오브젝트를 파괴하지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 파괴
        }
    }
}
