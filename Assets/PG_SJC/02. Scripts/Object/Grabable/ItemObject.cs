using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class ItemObject : InteractObject
    {
        [Header("아이템 오브젝트 세팅")]
        [SerializeField]
        private int itemID;
        public int ItemID { get { return itemID; }}

        [SerializeField]
        private float scalingSpeed = 4f;

        [SerializeField]
        private Vector3 inventoryScale;

        [Space(10)]
        [Header("밸런싱")]
        [Space(5)]
        [SerializeField]
        private Vector3 originScale;

        private Coroutine setScaleRoutine;

        protected override void Awake()
        {
            base.Awake();
            trackScale = false;
            originScale = transform.localScale; 
        }
        protected override void OnDisable()
        {
            base.OnDisable();

            if (setScaleRoutine != null)
            {
                StopCoroutine(setScaleRoutine);
                setScaleRoutine = null;
            }
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            if (Manager.UI.OnPopup)
                SetScale();
        }

        public void ResetScale()
        {
            if (setScaleRoutine != null)
            {
                StopCoroutine(setScaleRoutine);
                setScaleRoutine = null;
            }
            transform.localScale = originScale;
        }
        public void ResetScaleWithLerp()
        {
            if(setScaleRoutine != null)
            {
                StopCoroutine(setScaleRoutine);
                setScaleRoutine = null;
            }

            setScaleRoutine = StartCoroutine(SetScaleRoutine(originScale));
        }
        public void SetScale()
        {
            if (setScaleRoutine != null)
            {
                StopCoroutine(setScaleRoutine);
                setScaleRoutine = null;
            }
            transform.localScale = inventoryScale;
        }
        public void SetScaleWithLerp()
        {
            if (setScaleRoutine != null)
            {
                StopCoroutine(setScaleRoutine);
                setScaleRoutine = null;
            }

            setScaleRoutine = StartCoroutine(SetScaleRoutine(inventoryScale));
        }

        IEnumerator SetScaleRoutine(Vector3 targetScale)
        {
            float rate = 0f;
            Vector3 startScale = transform.localScale;
            Vector3 endScale = targetScale;
            while(rate < 1f)
            {
                rate += Time.deltaTime * scalingSpeed;
                transform.localScale = Vector3.Lerp(startScale, endScale, rate);
                yield return null;
            }
            setScaleRoutine = null;
            yield return null;
        }
    }
}
