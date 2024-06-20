using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Hands.Gestures;

namespace UnityEngine.XR.Hands.Samples.GestureSample
{
    public class StaticHandGesture : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("왼손 트래킹용")]
        XRHandTrackingEvents m_LeftHandTrackingEvents;

        [SerializeField]
        [Tooltip("오른손 트래킹용")]
        XRHandTrackingEvents m_RightHandTrackingEvents;

        [SerializeField]
        [Tooltip("확인 할 제스처")]
        ScriptableObject m_HandShapeOrPose;

        [SerializeField]
        [Tooltip("손 모양 또는 포즈의 대상 조건에 대한 사용자 변환 대상")]
        Transform m_TargetTransform;

        [SerializeField]
        [Tooltip("왼손 제스처 실행 이벤트")]
        UnityEvent m_LeftGesturePerformed;

        [SerializeField]
        [Tooltip("오른손 제스처 실행 이벤트")]
        UnityEvent m_RightGesturePerformed;

        [SerializeField]
        [Tooltip("왼손 제스처 종료 이벤트")]
        UnityEvent m_LeftGestureEnded;

        [SerializeField]
        [Tooltip("오른손 제스처 종료 이벤트")]
        UnityEvent m_RightGestureEnded;

        [SerializeField]
        [Tooltip("제스처 최소 실행 시간")]
        float m_MinimumHoldTime = 0.2f;

        [SerializeField]
        [Tooltip("제스처 감지가 수행되는 간격")]
        float m_GestureDetectionInterval = 0.1f;


        XRHandShape m_HandShape;
        XRHandPose m_HandPose;
        bool leftWasDetected;
        bool rightWasDetected;
        public bool leftPerformedTriggered;
        public bool rightPerformedTriggered;
        float leftTimeOfLastConditionCheck;
        float rightTimeOfLastConditionCheck;
        float leftHoldStartTime;
        float rightHoldStartTime;


        public XRHandTrackingEvents leftHandTrackingEvents
        {
            get => m_LeftHandTrackingEvents;
            set => m_LeftHandTrackingEvents = value;
        }

        public XRHandTrackingEvents rightHnadTrackingEvents
        {
            get => m_RightHandTrackingEvents;
            set => m_RightHandTrackingEvents = value;
        }
 
        public ScriptableObject handShapeOrPose
        {
            get => m_HandShapeOrPose;
            set => m_HandShapeOrPose = value;
        }

        public Transform targetTransform
        {
            get => m_TargetTransform;
            set => m_TargetTransform = value;
        }

        public UnityEvent leftGesturePerformed
        {
            get => m_LeftGesturePerformed;
            set => m_LeftGesturePerformed = value;
        }
        public UnityEvent rightGesturePerformed
        {
            get => m_RightGesturePerformed;
            set => m_RightGesturePerformed = value;
        }

        public UnityEvent leftGestureEnded
        {
            get => m_LeftGestureEnded;
            set => m_LeftGestureEnded = value;
        }
        public UnityEvent rightGestureEnded
        {
            get => m_RightGestureEnded;
            set => m_RightGestureEnded = value;
        }

        public float minimumHoldTime
        {
            get => m_MinimumHoldTime;
            set => m_MinimumHoldTime = value;
        }

        public float gestureDetectionInterval
        {
            get => m_GestureDetectionInterval;
            set => m_GestureDetectionInterval = value;
        }

        void Awake()
        {

        }

        void OnEnable()
        {
            m_LeftHandTrackingEvents.jointsUpdated.AddListener(OnLeftJointsUpdated);
            m_RightHandTrackingEvents.jointsUpdated.AddListener(OnRightJointsUpdated);

            m_HandShape = m_HandShapeOrPose as XRHandShape;
            m_HandPose = m_HandShapeOrPose as XRHandPose;
            if (m_HandPose != null && m_HandPose.relativeOrientation != null)
                m_HandPose.relativeOrientation.targetTransform = m_TargetTransform;
        }

        void OnDisable()
        {
            m_LeftHandTrackingEvents.jointsUpdated.RemoveListener(OnLeftJointsUpdated);
            m_RightHandTrackingEvents.jointsUpdated.RemoveListener(OnRightJointsUpdated);
        }

        void OnLeftJointsUpdated(XRHandJointsUpdatedEventArgs eventArgs)
        {
            if (!isActiveAndEnabled || Time.timeSinceLevelLoad < leftTimeOfLastConditionCheck + m_GestureDetectionInterval)
                return;

            bool detected =
                (m_LeftHandTrackingEvents.handIsTracked &&
                m_HandShape != null && m_HandShape.CheckConditions(eventArgs) ||
                m_HandPose != null && m_HandPose.CheckConditions(eventArgs));


            //이전 프레임에서 감지되었는지
            if (!leftWasDetected && detected)
            {
                leftHoldStartTime = Time.timeSinceLevelLoad;
            }
            else if (leftWasDetected && !detected)
            {
                leftPerformedTriggered = false;
                m_LeftGestureEnded?.Invoke();
            }

            leftWasDetected = detected;

            //제스처를 실행햇는지
            if (!leftPerformedTriggered && detected)
            {
                float holdTimer = Time.timeSinceLevelLoad - leftHoldStartTime;
                if (holdTimer > m_MinimumHoldTime)
                {
                    m_LeftGesturePerformed?.Invoke();
                    leftPerformedTriggered = true;
                }
            }

            leftTimeOfLastConditionCheck = Time.timeSinceLevelLoad;
        }


        void OnRightJointsUpdated(XRHandJointsUpdatedEventArgs eventArgs)
        {
            if (!isActiveAndEnabled || Time.timeSinceLevelLoad < rightTimeOfLastConditionCheck + m_GestureDetectionInterval)
                return;

            bool detected =
                (m_RightHandTrackingEvents.handIsTracked &&
                m_HandShape != null && m_HandShape.CheckConditions(eventArgs) ||
                m_HandPose != null && m_HandPose.CheckConditions(eventArgs));

            //이전 프레임에서 감지되었는지
            if (!rightWasDetected && detected)
            {
                rightHoldStartTime = Time.timeSinceLevelLoad;
            }
            else if (rightWasDetected && !detected)
            {
                rightPerformedTriggered = false;
                m_RightGestureEnded?.Invoke();
            }

            rightWasDetected = detected;

            //제스처를 실행햇는지
            if (!rightPerformedTriggered && detected)
            {
                float holdTimer = Time.timeSinceLevelLoad - rightHoldStartTime;
                if (holdTimer > m_MinimumHoldTime)
                {
                    m_RightGesturePerformed?.Invoke();
                    rightPerformedTriggered = true;
                }
            }
            //Debug.Log($"{m_HandShapeOrPose.name} : {rightPerformedTriggered}");
            rightTimeOfLastConditionCheck = Time.timeSinceLevelLoad;
        }
    }
}
