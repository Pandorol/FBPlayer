using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FeedBack
{
    [FeedBackAttribute("Transform",0,TweenType.Tween)]
    public class PositionFeedBack : Feedback
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

        [BoxGroup("位移设置")] public PositionMatrixType mpositionMatrix;

        [BoxGroup("位移设置")] public TranslateType mTranslateType;

        [BoxGroup("位移设置")] public float mJumpPower;

        [BoxGroup("位移设置")] public int mNumJumps;

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
                    mLastValues[i] = TargetTransforms[i].position;
                }
            }
        }

        public override void Reset()
        {
            for (int i = 0; i < TargetTransforms.Length; i++)
            {
                TargetTransforms[i].position = mLastValues[i];
            }
        }

        public override Tween GetTween()
        {
            if (TargetTransforms.Length <= 0)
                Debug.LogError($"[PositionFeedBack:] 没有设置目标");

            var tween = DOTween.Sequence();
            foreach (var ta in TargetTransforms)
            {
                tween.Join(GetPositionTween(ta));
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

        #region position

        private Tween GetPositionTween(Transform trans)
        {
            switch (mTranslateType)
            {
                case TranslateType.Move:
                    if (mpositionMatrix == PositionMatrixType.World)
                        return trans.DOMove(TargetVal, Duration);
                    else
                        return trans.DOLocalMove(TargetVal, Duration);
                case TranslateType.Jump:
                    if (mpositionMatrix == PositionMatrixType.World)
                        return trans.DOJump(TargetVal, mJumpPower, mNumJumps, Duration);
                    else
                        return trans.DOLocalJump(TargetVal, mJumpPower, mNumJumps, Duration);
            }

            return null;
        }

        #endregion
    }


    public enum PositionMatrixType
    {
        World,
        Local,
    }

    public enum TranslateType
    {
        Move,
        Jump,
    }
}