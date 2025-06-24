using DG.Tweening;
using Sirenix.OdinInspector;

namespace FeedBack
{
    [FeedBack("Internal", 1, TweenType.Interval)]
    public class IntervalFeedBack : DelayFeedback
    {
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
    }
}