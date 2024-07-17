using System;
using JJH;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace JJH
{
    [System.Serializable]
    public class CanvasData  // 캔버스 데이터 이름 수정 할 것. --> Manager 도 이름 수정 필요.
        // 채색 기능은 json 으로 씬 전환 시 및 다시 시작 할 시 저장 되도록 해야 한다. 
    {

        [Tooltip("로비 그림 --> 이게 둘 중 에 하나만 있어도 아마 될 듯 한대 일단 그냥 진행함 나중에 수정하자 ")]
        // 그림 조각 저장용
        public bool[] myDrawPartCheckArr = new bool[20];

        [Tooltip("포스트 프로세싱 컬러 / 흑백 전환 bool 변수 -> 0 1 2 3 각 스테이지 마다 체크 / 로딩루틴 ")]


        // 씬의 포스트 프로세싱 상태 저장용 
        public bool[] isColoredCheckArr = new bool[4];  // 이게 포스트 프로세싱 상태 체크 용

        // 이게 0번이 활성화 되면 -> 첫 번째 1챕터 포스트프로세싱 컬러 상태 --> 이게 즉 결국 2챕터 해금 상태. 


        // 챕터 해금 상태 저장이랑 포스트 프로세싱 이랑 한 번에 좀 열거형으로 저장해주면 어떻게 잘되지 않을까
        /*public enum CanvasState
        {
            // 비 해금 , 흑백으로 해금됨 , 컬러로 해금 됨 
            DisActive , Proceed , Clear 
        }
*/

        [Tooltip("스테이지 해금 상태 저장용 배열 변수")]
        public bool[] stageUnlockStatus = new bool[4]; // 어차피 스테이지 4개니까 그냥 4로 하자. 



    }


    

}

