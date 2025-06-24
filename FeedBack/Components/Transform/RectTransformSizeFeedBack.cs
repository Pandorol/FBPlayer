using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FeedBack
{
    
    [FeedBackAttribute("Transform",10,TweenType.Tween)]
    public class RectTransformSizeFeedBack: Feedback
    {
        private Vector2[] mLastSizes;

        public override void RecordOrginData()
        {
            if (lastSetingInitFlag) return;
            lastSetingInitFlag = true;


            mLastSizes = new Vector2[TargetRectTransforms.Length];
            for (int i = 0; i < TargetRectTransforms.Length; i++)
            {
                if (FromValUseAwake)
                {
                    mLastSizes[i] = TargetRectTransforms[i].sizeDelta;
                }
                else
                {
                    mLastSizes[i] = FromSize;
                }
            }
        }

        public override void Reset()
        {
            for (int i = 0; i < TargetRectTransforms.Length; i++)
            {
                TargetRectTransforms[i].sizeDelta = mLastSizes[i];
            }
        }


        public override void TestPlay()
        {
            throw new System.NotImplementedException();
        }

        [BoxGroup("基础设置"), LabelText("目标节点"), GUIColor(0, 1, 0.2f)]
        public RectTransform[] TargetRectTransforms;

        [BoxGroup("基础设置"), LabelText("时间")] public float Duration = 1;
        
        [BoxGroup("参数设置")] public bool FromValUseAwake = false;
        [BoxGroup("参数设置"), HideIf("FromValUseAwake")]
        public Vector2 FromSize;

        [BoxGroup("参数设置")] public Vector2 ToSize;

        [BoxGroup("参数设置"), SerializeField] private EaseInfo mEaseInfo;


        public override Tween GetTween()
        {
            if (TargetRectTransforms.Length <= 0)
                Debug.LogError($"[PositionFeedBack:] 没有设置目标");


            var sq = DOTween.Sequence();
            foreach (var sr in TargetRectTransforms)
            {
                sq.Join(GetTween(sr));
            }

            switch (mEaseInfo.mEaseType)
            {
                case EaseType.Ease:
                    sq.SetEase(mEaseInfo.ease);
                    break;
                case EaseType.Curve:
                    sq.SetEase(mEaseInfo.curve);
                    break;
            }

            return sq;
        }

        private Tween GetTween(RectTransform sr)
        {
            return sr.DOSizeDelta(ToSize, Duration);
        }
    }
}