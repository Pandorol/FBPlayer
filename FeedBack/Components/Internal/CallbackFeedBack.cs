using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FeedBack
{
    [FeedBack("Internal", 1, TweenType.Callback)]
    public class CallbackFeedBack : ActionFeedback
    {
        [BoxGroup("基础设置"), SerializeField, GUIColor(0, 1, 0.2f)]
        public UnityEvent ActionCallback;

        public override void RecordOrginData()
        {
        }

        public override void Reset()
        {
        }

        public override Tween GetTween()
        {
            return null;
        }

        public override void TestPlay()
        {
        }

        public override void GetActionFunc()
        {
            ActionCallback.Invoke();
        }
    }
}