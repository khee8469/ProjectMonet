using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishedDraw : MonoBehaviour
{
    // 얘는 완성된 그림 오브젝트임 --> 

    public int drawID; // 0 부터 해서 그냥 한번에 함수에 같이 같은 값으로 붙여주자. 
    SpriteRenderer spriteRenderer;
    private float alphaValue;
    
    public static UnityEvent <int> FinishAlphaUp = new UnityEvent<int> ();


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        alphaValue = spriteRenderer.color.a;
        

    }

    void Start()
    {
        Color color = spriteRenderer.color;

        if (ChapterManager.is_Colored[drawID]==true)
        {
            alphaValue = 1f; //켜져있으면.
            Debug.Log("원본 그림 켜짐");
        }
        else
        {
            alphaValue = 0f; // 꺼져있으면 
            
        }

        color.a = alphaValue;
        spriteRenderer.color = color; // 변경된 알파 값을 반영합니다.

        FinishAlphaUp.AddListener(finishedPaint_AlphaUp);


    }


    public void finishedPaint_AlphaUp(int number)
    {
        if(number== drawID)
        {
            Color color = spriteRenderer.color;

            color.a = 1f; // 1로 돌려주기. 
            spriteRenderer.color = color; //구조체라 다시 대입 필요 

            
        }
    }


    
}
