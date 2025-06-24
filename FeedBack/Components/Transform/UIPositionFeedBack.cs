using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FeedBack
{
    [FeedBackAttribute("UI", 0, TweenType.Tween)]
    public class UIPositionFeedBack : Feedback
    {
        #region property

        [BoxGroup("基础设置"), LabelText("目标节点"), GUIColor(0, 1, 0.2f)]
        public RectTransform TargetTransform;

        [BoxGroup("基础设置"), LabelText("时间")] public float Duration =
            1;

        [SerializeField, BoxGroup("基础设置"), LabelText("初始化是否自定义")]
        private bool mCustomInitailVal = false;

        [BoxGroup("基础设置"), LabelText("InitailVal"), ShowIf("mCustomInitailVal")]
        public Vector3 InitailVal =
            Vector3.zero;

        [BoxGroup("基础设置"), LabelText("Target")]
        public Vector3 TargetVal =
            Vector3.zero;

        [BoxGroup("动画设置"), PropertyOrder(20)] public EaseInfo AnimEase;

        #endregion


        #region contro

        public override void RecordOrginData()
        {
            if (!lastSetingInitFlag)
            {
                lastSetingInitFlag = true;
                if (!mCustomInitailVal)
                {
                    InitailVal = TargetTransform.anchoredPosition;
                }
            }
        }

        public override void Reset()
        {
            TargetTransform.anchoredPosition = InitailVal;
        }

        public override Tween GetTween()
        {
            if (TargetTransform == null)
                Debug.LogError($"[PositionFeedBack:] 没有设置目标");

            var tween = GetPositionTween();

            switch (AnimEase.mEaseType)
            {
                case EaseType.Ease:
                    tween.SetEase(AnimEase.ease);
                    break;
                case EaseType.Curve:
                    tween.SetEase(AnimEase.curve);
                    break;
            }

            return tween;
        }

        #endregion


        public override void TestPlay()
        {
            throw new System.NotImplementedException();
        }

        #region position

        private Tween GetPositionTween()
        {
            return TargetTransform.DOAnchorPos(TargetVal, Duration);
        }

        #endregion
    }
}