using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace FeedBack
{
    [FeedBackAttribute("UI", 101,TweenType.Tween)]
    public class UISliderFeedBack : Feedback
    {
        private float[] mLastValues;

        public override void RecordOrginData()
        {
            if (lastSetingInitFlag) return;
            lastSetingInitFlag = true;


            mLastValues = new float[TargetSliders.Length];
            for (int i = 0; i < TargetSliders.Length; i++)
            {
                if (FromValUseAwake)
                {
                    mLastValues[i] = TargetSliders[i].value;
                }
                else
                {
                    mLastValues[i] = FromValue;
                }
            }
        }

        public override void Reset()
        {
            for (int i = 0; i < TargetSliders.Length; i++)
            {
                TargetSliders[i].value = mLastValues[i];
            }
        }


        public override void TestPlay()
        {
            throw new System.NotImplementedException();
        }

        [BoxGroup("基础设置"), LabelText("目标节点"), GUIColor(0, 1, 0.2f)]
        public Slider[] TargetSliders;

        [BoxGroup("基础设置"), LabelText("时间")] public float Duration = 1;
        [BoxGroup("基础设置")] public bool FromValUseAwake = false;

        [BoxGroup("基础设置"), ShowIf("FromValUseAwake")]
        public float FromValue;

        [BoxGroup("参数设置")] public float ToValue;

        [BoxGroup("参数设置"), SerializeField] private EaseInfo mEaseInfo;


        public override Tween GetTween()
        {
            if (TargetSliders.Length <= 0)
                Debug.LogError($"[PositionFeedBack:] 没有设置目标");


            var sq = DOTween.Sequence();
            foreach (var sr in TargetSliders)
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

        private Tween GetTween(Slider sr)
        {
            return sr.DOValue(ToValue, Duration);
        }
    }
}