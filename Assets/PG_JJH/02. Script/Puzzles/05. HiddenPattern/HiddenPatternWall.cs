using Jc;
using JJH;
using System.Collections;
using UnityEngine;

public class HiddenPatternWall : MonoBehaviour , IPuzzleable
{

    // 이게 마테리얼이 알파값을 낮출 수 있는 마테리얼이 아니라서
    // 그냥 setActive 해야 할듯한대? 

    [SerializeField] private Material material;

    [SerializeField] HiddenPatternPuzzle puzzle;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    Coroutine coroutine; // 이거 어차피 같은 스크립트여도 자기 변수는 따로 쓰니까 괜찮아! 

    private void AlphaUp(Material mat)
    {
        if (mat.HasProperty("_BaseColor"))
        {
            /*Color baseColor = mat.GetColor("_BaseColor");
            baseColor.a = 1f;  // 알파값 1로 변경 
            mat.SetColor("_BaseColor", baseColor); // 컬러 값 설정*/
            if (coroutine != null)
            {
                StopCoroutine(coroutine);   
            }
            coroutine = StartCoroutine(AlphaUpRoutine(material));
        }
    }

    private IEnumerator AlphaUpRoutine(Material mat)
    {
        Color baseColor = mat.GetColor("_BaseColor");
        float duration = 1.5f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            baseColor.a = alpha;  // 알파값 1로 변경 
            Debug.Log($"알파 값 증가 중->{alpha}");
            mat.SetColor("_BaseColor", baseColor); // 컬러 값 설정
            yield return null;
        }


        baseColor.a = 1f;  // 알파값 1로 변경 
        mat.SetColor("_BaseColor", baseColor); // 컬러 값 설정

    }

    private void AlphaDown(Material mat)
    {
        /*Color baseColor = mat.GetColor("_BaseColor");
        baseColor.a = 0f;  // 알파값 1로 변경 
        mat.SetColor("_BaseColor", baseColor); // 컬러 값 설정*/

        if (coroutine != null)
        {
            StopCoroutine(coroutine);          
        }
        coroutine = StartCoroutine(AlphaDownRoutine(material));
    }

    private IEnumerator AlphaDownRoutine(Material mat)
    {
        Color baseColor = mat.GetColor("_BaseColor");
        float duration = 1.5f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / duration));
            baseColor.a = alpha; // 알파값 감소 시키기.
            Debug.Log($"알파 값 감소 중->{alpha}");
            mat.SetColor("_BaseColor", baseColor); // 컬러 값 설정
            yield return null;
        }

        baseColor.a = 0f;  // 알파값 1로 변경 
        mat.SetColor("_BaseColor", baseColor); // 컬러 값 설정

    }


    // 그냥 태그 한 번 만 쓰자.
    private void OnTriggerEnter(Collider other) // 트리거 시작 시 알파업 
    {
        if (other.transform.GetComponent<HiddenPatternflashlight>()?.IsGrabed == true)
        {
            Debug.Log("트리거 엔터");
            AlphaDown(material);
        }

    }
    private void OnTriggerExit(Collider other)  // 트리거 밖으로 나가면 알파 다운 
    {
        if (other.transform.GetComponent<HiddenPatternflashlight>()?.IsGrabed == true)
        {
            Debug.Log("트리거 엑시트");
            AlphaUp(material);
        }
    }



    // 약간 굳이 싶기는 한대 그래도 클리어 하면 없애주는게 나을 것 같기는 함. 
    public void RegistObject(PuzzleManager puzzle)
    {
        
    }

    public void UpdatePuzzleManager(PuzzleManager puzzle, int index = -1)
    {
        
    }

    public void ActiveSetting()
    {
        
    }

    public void DisActiveSetting()
    {
        
    }

    public void CompleteSetting()
    {
        gameObject.SetActive(false); //완료 상태면 그냥 자기 자신 꺼주기. 
    }
}
