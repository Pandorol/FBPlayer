using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FeedBack
{
    [FeedBackAttribute("UI",100,TweenType.Tween)]
    public class ImageFeedBack: Feedback
    {
         private Color[] mLastCorlors;

        public override void RecordOrginData()
        {
            if (lastSetingInitFlag) return;
            lastSetingInitFlag = true;
            
            mLastCorlors = new Color[TargetImages.Length];
            for (int i = 0; i < TargetImages.Length; i++)
            {
                mLastCorlors[i] = TargetImages[i].color;
            }
        }

        public override void Reset()
        {
            
            for (int i = 0; i < TargetImages.Length; i++)
            {
                TargetImages[i].color = mLastCorlors[i];
            }

            
        }


        public override void TestPlay()
        {
            throw new System.NotImplementedException();
        }

        [BoxGroup("基础设置"), LabelText("目标节点"), GUIColor(0, 1, 0.2f)]
        public Image[] TargetImages;

        [BoxGroup("基础设置"), LabelText("目标节点")] public RendererConmmand TargetCommad;
        [BoxGroup("基础设置"), LabelText("时间")] public float Duration = 1;

        [BoxGroup("参数设置"), HideIf("HideColor")]
        public Color color;

        [BoxGroup("参数设置"), HideIf("HideTo")] public float to;

        [BoxGroup("参数设置"), HideIf("HideGradient")]
        public Gradient gradient;

        [BoxGroup("参数设置"), SerializeField] private EaseInfo mEaseInfo;


        public override Tween GetTween()
        {
            if (TargetImages.Length <= 0)
                Debug.LogError($"[PositionFeedBack:] 没有设置目标");

            
            var sq = DOTween.Sequence();
            foreach (var sr in TargetImages)
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

        private Tween GetTween(Image sr)
        {
            switch (TargetCommad)
            {
                case RendererConmmand.Color:
                    return sr.DOColor(color, Duration);
                case RendererConmmand.Fade:
                    return sr.DOFade(to, Duration);
                case RendererConmmand.GradientColor:
                    return sr.DOGradientColor(gradient, Duration);
                case RendererConmmand.BlendableColor:
                    return sr.DOBlendableColor(color, Duration);
            }

            return null;
        }

        private bool HideColor()
        {
            return !TargetCommad.ToString().Contains("Color") || TargetCommad == RendererConmmand.GradientColor;
        }

        private bool HideTo()
        {
            return TargetCommad != RendererConmmand.Fade;
        }

        private bool HideGradient()
        {
            return TargetCommad != RendererConmmand.GradientColor;
        }


    }
}