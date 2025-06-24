using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FeedBack
{
    [Serializable]
    public abstract class Feedback
    {
        protected bool lastSetingInitFlag;

        [ReadOnly, PropertyOrder(-10), SerializeField]
        public string Name;

        [ReadOnly, PropertyOrder(-9), SerializeField]
        public int UniID;

        [SerializeField, PropertyOrder(-8)] public OperationType Operation;

        public string Title
        {
            get { return $"{Name}"; }
        }

        //记录原始数据
        public abstract void RecordOrginData();
        public abstract void Reset();

        public abstract Tween GetTween();
        public abstract void TestPlay();
    }

    public abstract class DelayFeedback : Feedback
    {
        [SerializeField, GUIColor(0, 1, 0.2f), PropertyOrder(-7)]
        public float Interval;
    }
    
    public abstract class ActionFeedback : Feedback
    {
        // [SerializeField, GUIColor(0, 1, 0.2f), PropertyOrder(-7)]
        public abstract void GetActionFunc();
    }

    [HideLabel, Serializable]
    public struct EaseInfo
    {
        public EaseType mEaseType;
        [ShowIf("mEaseType", EaseType.Ease)] public Ease ease;

        [ShowIf("mEaseType", EaseType.Curve), HideReferenceObjectPicker]
        public AnimationCurve curve;
    }

    public enum OperationType
    {
        Append,
        Join,
    }

    public enum EaseType
    {
        Ease,
        Curve,
    }
}