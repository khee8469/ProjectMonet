using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class PictureBook : MonoBehaviour
{
    [Header("에디터 세팅")]
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private List<BookPage> pageGroup;  // 페이지 그룹
    private BookPage currentDepth;     // 현재 페이지

    [SerializeField]
    private int maxPage;
    [SerializeField]
    private int currentPage = -1;
    [SerializeField]
    private int prevPage = -1;

    private int id_NextPage;
    private int id_PrevPage;
    private int id_OpenBook;
    private int id_CloseBook;

    private void Awake()
    {
        // 애니메이터 파라미터 캐싱
        id_NextPage = Animator.StringToHash("NextPage");
        id_PrevPage = Animator.StringToHash("PrevPage");
        id_OpenBook = Animator.StringToHash("OpenBook");
        id_CloseBook = Animator.StringToHash("CloseBook");
    }

    public void NextPage()
    {
        prevPage = currentPage;
        currentPage++;
        if (currentPage == -1)
            anim.SetTrigger(id_OpenBook);
        // 다음 페이지가 존재한다면
        else if (currentPage < maxPage)
            anim.SetTrigger(id_NextPage);
        // 다음 페이지가 없을 경우
        else
            currentPage = maxPage;
        prevPage = -1;
    }
    public void PrevPage()
    {
        prevPage = currentPage;
        currentPage--;
        // 이전 페이지가 존재한다면
        if (currentPage >= 0)
        {
            anim.SetTrigger(id_PrevPage);
        }
        // 이전 페이지가 없을 경우
        else
        {
            currentPage = 0;
        }
        prevPage = -1;
    }

    #region 애니메이션 액션
    // 이전 페이지 비활성화
    public void DisActivePageDepth()
    {
        if(prevPage >= 0 && prevPage < pageGroup.Count)
        {
            pageGroup[prevPage].gameObject.SetActive(false);
        }
    }

    // 현재 페이지 활성화
    public void ActivePageDepth()
    {
        if(currentPage >=0 && currentPage < pageGroup.Count)
        {
            currentDepth = pageGroup[currentPage];
            currentDepth.gameObject.SetActive(true);
        }
    }
    #endregion
}
