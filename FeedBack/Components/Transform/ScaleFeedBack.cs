using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FeedBack
{
    [FeedBackAttribute("Transform", 2,TweenType.Tween)]
    public class ScaleFeedBack : Feedback
    {
        #region property

        [BoxGroup("基础设置"), LabelText("目标节点"), GUIColor(0, 1, 0.2f)]
        public Transform[] TargetTransforms;

        [BoxGroup("基础设置"), LabelText("时间")] public float Duration =
            1;
        [BoxGroup("基础设置")]
        public bool FromValUseAwake = true;

        [BoxGroup("基础设置"), LabelText("StartVal"), HideIf("FromValUseAwake")]
        public Vector3 FromVal =
            Vector3.zero;

        [BoxGroup("基础设置"), LabelText("Target")]
        public Vector3 TargetVal =
            Vector3.zero;

        [BoxGroup("动画设置"), PropertyOrder(20)] public EaseInfo AnimEase;

        private Vector3[] mLastValues;

        [BoxGroup("大小设置"), SerializeField] private ScaleType mScaleType;

        #endregion


        #region contro

        public override void RecordOrginData()
        {
            if (!lastSetingInitFlag)
            {
                lastSetingInitFlag = true;
                if (FromValUseAwake)
                {
                    mLastValues = new Vector3[TargetTransforms.Length];
                    for (int i = 0; i < TargetTransforms.Length; i++)
                    {
                        mLastValues[i] = TargetTransforms[i].localScale;
                    }
                }
                else
                {
                    mLastValues = new Vector3[TargetTransforms.Length];
                    for (int i = 0; i < TargetTransforms.Length; i++)
                    {
                        mLastValues[i] = FromVal;
                    }
                }
            }
        }

        public override void Reset()
        {
            for (int i = 0; i < TargetTransforms.Length; i++)
            {
                TargetTransforms[i].localScale = mLastValues[i];
            }
        }

        public override Tween GetTween()
        {
            if (TargetTransforms.Length <= 0)
                Debug.LogError($"[ScaleFeedBack:] 没有设置目标");

            var tween = DOTween.Sequence();
            foreach (var ta in TargetTransforms)
            {
                tween.Join(GetScaleTween(ta));
            }

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

        #region scale

        private Tween GetScaleTween(Transform trans)
        {
            switch (mScaleType)
            {
                case ScaleType.Full:
                    return trans.DOScale(TargetVal, Duration);
                case ScaleType.OnlyX:
                    return trans.DOScaleX(TargetVal.x, Duration);
                case ScaleType.OnlyY:
                    return trans.DOScaleY(TargetVal.y, Duration);
                case ScaleType.OnlyZ:
                    return trans.DOScaleZ(TargetVal.z, Duration);
            }

            return null;
        }

        #endregion
    }

    enum ScaleType
    {
        Full,
        OnlyX,
        OnlyY,
        OnlyZ,
    }
}