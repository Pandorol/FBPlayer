using DG.Tweening;

using Sirenix.OdinInspector;
using UnityEngine;

namespace FeedBack
{
    [FeedBack("Audio", 1, TweenType.Callback)]
    public class AudioFeedBack: ActionFeedback
    {
        [BoxGroup("基础设置"), SerializeField, GUIColor(0, 1, 0.2f)]
        public string AudioName = "click";

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
            
        }
    }
}