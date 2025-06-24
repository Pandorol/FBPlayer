using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FeedBack
{
    [FeedBack("UI", 0, TweenType.Tween)]
    public class CanvasFeedBack : Feedback
    {
        private float[] mLastAlphas;

        public override void RecordOrginData()
        {
            if (lastSetingInitFlag) return;
            lastSetingInitFlag = true;

            if (!mCustomInitailVal)
            {
                InitailVal = TargetCanvas.alpha;
            }
        }

        public override void Reset()
        {
            TargetCanvas.alpha = InitailVal;
        }


        public override void TestPlay()
        {
            throw new System.NotImplementedException();
        }

        [BoxGroup("基础设置"), LabelText("目标节点"), GUIColor(0, 1, 0.2f)]
        public CanvasGroup TargetCanvas;

        [BoxGroup("基础设置"), LabelText("时间")] public float Duration = 1;

        [SerializeField, BoxGroup("参数设置"), LabelText("初始化是否自定义")]
        private bool mCustomInitailVal = false;

        [BoxGroup("参数设置"), LabelText("InitailVal"), ShowIf("mCustomInitailVal")]
        public float InitailVal = 0;

        [BoxGroup("参数设置")] public float ToAlpha;

        [BoxGroup("参数设置"), SerializeField] private EaseInfo mEaseInfo;


        public override Tween GetTween()
        {
            if (TargetCanvas == null)
                Debug.LogError($"[PositionFeedBack:] 没有设置目标");


            var sq = GetTween(TargetCanvas);

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

        private Tween GetTween(CanvasGroup sr)
        {
            return sr.DOFade(ToAlpha, Duration);
        }
    }
}