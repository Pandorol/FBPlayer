using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FeedBack
{
    [FeedBackAttribute("Transform",1,TweenType.Tween)]
    public class RotationFeedBack : Feedback
    {
        #region property

        [BoxGroup("基础设置"), LabelText("目标节点"), GUIColor(0, 1, 0.2f)]
        public Transform[] TargetTransforms;

        [BoxGroup("基础设置"), LabelText("时间")] public float Duration =
            1;

        [BoxGroup("基础设置"), LabelText("Target")]
        public Vector3 TargetVal =
            Vector3.zero;

        [BoxGroup("动画设置"), PropertyOrder(20)] public EaseInfo AnimEase;

        private Vector3[] mLastValues;

        [BoxGroup("旋转设置"), SerializeField]
        private RotateMode mRotateMode;

        [BoxGroup("旋转设置"),  SerializeField]
        private RotateMatrixType mRotateMatrixType;
        
        #endregion
        
        #region contro

        public override void RecordOrginData()
        {
            if (!lastSetingInitFlag)
            {
                lastSetingInitFlag = true;
                mLastValues = new Vector3[TargetTransforms.Length];
                for (int i = 0; i < TargetTransforms.Length; i++)
                {
                    mLastValues[i] = TargetTransforms[i].eulerAngles;
                }
            }
        }

        public override void Reset()
        {
            for (int i = 0; i < TargetTransforms.Length; i++)
            {
                TargetTransforms[i].eulerAngles = mLastValues[i];
            }
        }

        public override Tween GetTween()
        {
            if (TargetTransforms.Length <= 0)
                Debug.LogError($"[RotationFeedBack:] 没有设置目标");

            var tween = DOTween.Sequence();
            foreach (var ta in TargetTransforms)
            {
                tween.Join(GetRotateTween(ta));
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

        #region Rotation

        private Tween GetRotateTween(Transform trans)
        {
            switch (mRotateMatrixType)
            {
                case RotateMatrixType.World:
                    return trans.DORotate(TargetVal, Duration, mRotateMode);
                case RotateMatrixType.Local:
                    return trans.DOLocalRotate(TargetVal, Duration, mRotateMode);
            }

            return null;
        }

        #endregion
        
        public override void TestPlay()
        {
            throw new System.NotImplementedException();
        }
    }

    public enum RotateMatrixType
    {
        World,
        Local,
    }
}