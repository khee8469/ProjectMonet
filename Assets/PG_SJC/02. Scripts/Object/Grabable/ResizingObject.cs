using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    /// <summary>
    /// 오브젝트 크기 착시 시스템
    /// </summary>

    // 최초 오브젝트가 잡혔을 때 (Grab 되었을 때)
    // 1. 플레이어 -> 오브젝트 방향으로 레이를 발사
    // 2. 레이에 닿은 벽과 오브젝트 사이 거리를 할당 (최초 거리)
    // 3. Update에서 지속적으로 레이를 발사하여 최초 거리에 비례해 오브젝트의 크기 변경

    public class ResizingObject : InteractObject
    {
        // 값 형식 트랜스폼
        public struct V_Transform
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
            public V_Transform(Vector3 position, Quaternion rotation, Vector3 scale)
            {
                this.position = position;
                this.rotation = rotation;
                this.scale = scale;
            }
            public V_Transform(Transform transform)
            {
                this.position = transform.position;
                this.rotation = transform.rotation;
                this.scale = transform.localScale;
            }
        }

        [Header("에디터 세팅")]
        [SerializeField]
        private float limitScale;   // 크기의 한계치 (넘어서면 원복)

        [SerializeField]
        private Vector3 maxScale;
        [SerializeField]
        private Vector3 minScale;

        [Space(5)]
        [Header("밸런싱")]
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private V_Transform resetTransform;

        [SerializeField]
        private float originScaleX;     // 최초 크기
        [SerializeField]
        private float originDist = -1f;       // 최초 거리 (sqr)
        [SerializeField]
        private float originRatio = -1f;      // 최대 크기에 비례한 최초 비율

        public bool IsGrabbed   // isGrabbed 프로퍼티
        {
            get { return isGrabbed; }
            set { isGrabbed = value; }
        }
        public float scaleRatio;

        //float ognDist;
        //float ognScale;
        Vector3 targetScale;
        protected override void OnEnable()
        {
            base.OnEnable();
            mainCamera = Camera.main;
            resetTransform = new V_Transform(transform);
        }

        private void Update()
        {
            if (!IsGrabbed) return;

            SetPosition();
            //Resize();
        }

        private void Resize()
        {
            //float scaleRatio = 0f;

            Vector3 rayDir = mainCamera.transform.forward;
            Ray ray = new Ray(transform.position, rayDir);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, Manager.Layer.wallLM))
            {
                if (originDist < 0f) // 최초 거리가 할당되지 않은 경우
                {
                    originDist = (hitInfo.point - transform.position).sqrMagnitude;         // 벽과 거리계산
                    originScaleX = transform.localScale.x;
                    scaleRatio = originRatio; // 최초 비율 설정
                }
                else // 최초 거리와 비례해 크기 재설정
                {
                    float targetDist = (hitInfo.point - transform.position).sqrMagnitude;   // 벽과 거리계산
                    scaleRatio = targetDist / originDist * originRatio;       // 비율 설정
                    scaleRatio = Mathf.Clamp(scaleRatio, 0.01f, 1f);           // 최소 비율, 최대 비율 설정 
                }
            }
            else // 레이가 닿지 않은 경우
            {
                originScaleX = transform.localScale.x;
                scaleRatio = originRatio;   // 최초 비율 설정
            }

            transform.localScale = Vector3.Lerp(minScale, maxScale, scaleRatio);
        }

        // 오브젝트의 위치값 고정 (메인 카메라 기준)
        private void SetPosition()
        {
            Vector3 rayDir = mainCamera.transform.forward;
            Ray ray = new Ray(mainCamera.transform.position, rayDir);

            // 구체 레이
            //if (Physics.SphereCast(ray, transform.localScale.x, out RaycastHit hitInfo, Mathf.Infinity, Manager.Layer.wallLM))
            //{
            //    // 닿은 벽을 기준으로 오브젝트의 위치설정
            //    transform.position = hitInfo.point + hitInfo.normal * targetScale.x;

            //    if (originDist == -1f)
            //    {
            //        originDist = (mainCamera.transform.position - transform.position).sqrMagnitude;
            //    }

            //    Debug.DrawRay(mainCamera.transform.position, rayDir * 500f, Color.yellow);

            //    float curDist = (mainCamera.transform.position - transform.position).sqrMagnitude;
            //    float ratio = curDist / originDist;
            //    targetScale.x = targetScale.y = targetScale.z = ratio;

            //    // 최소 스케일 지정
            //    if (ratio * originScaleX < 0.1f)
            //        transform.localScale = minScale;
            //    else
            //        transform.localScale = targetScale * originScaleX;
            //}
            //else
            //{
            //    Debug.Log("레이가 닿지 않습니다.");
            //    // 레이가 닿지 않은 경우 강제로 Detach 
            //    //ForceDetach();
            //}

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, Manager.Layer.wallLM))
            {
                // 닿은 벽을 기준으로 오브젝트의 위치설정
                transform.position = hitInfo.point - rayDir * targetScale.x;

                if (originDist == -1f)
                {
                    originDist = (mainCamera.transform.position - transform.position).sqrMagnitude;
                }

                Debug.DrawRay(mainCamera.transform.position, hitInfo.point, Color.yellow);

                float curDist = (mainCamera.transform.position - transform.position).sqrMagnitude;
                float ratio = curDist / originDist;
                targetScale.x = targetScale.y = targetScale.z = ratio;

                // 최소 스케일 지정
                if (ratio * originScaleX < 0.1f)
                    transform.localScale = minScale;
                else
                    transform.localScale = targetScale * originScaleX;
            }
            else
            {
                if (!isSelected) return;
                // 레이가 닿지 않은 경우 강제로 Detach
                //ForceDetach();
            }

            //transform.position = mainCamera.transform.position + mainCamera.transform.forward * grabDistance;
        }
        private void SetTransform()
        {
            originScaleX = transform.localScale.x;
            targetScale = transform.localScale;
            originDist = -1f;

        }

        // 인터렉터에 SelectExit 호출
        private void ForceDetach()
        {
            foreach (var interactor in interactorsSelecting)
            {
                interactionManager.SelectExit(interactor, this);
            }
        }
        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);
            transform.GetComponent<Rigidbody>().isKinematic = true;

            SetTransform();

            IsGrabbed = true;
        }
        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);
            transform.GetComponent<Rigidbody>().isKinematic = false;

            IsGrabbed = false;

            // 오브젝트의 크기가 최대치 이상이 되면 원상복구
            if (transform.localScale.x >= limitScale)
            {
                StartCoroutine(ResizeRoutine());
            }
        }

        IEnumerator ResizeRoutine()
        {
            float rate = 0f;
            Vector3 startScale = transform.localScale;

            while (rate < 1f)
            {
                rate += Time.deltaTime * 2f;
                //transform.position = Vector3.Lerp(startPos, resetTransform.position, rate);
                ///transform.rotation = Quaternion.Lerp(startRot, resetTransform.rotation, rate);
                transform.localScale = Vector3.Lerp(startScale, resetTransform.scale, rate);
                yield return null;
            }

            //transform.position = resetTransform.position;
            //transform.rotation = resetTransform.rotation;
            transform.localScale = resetTransform.scale;

        }
    }
}