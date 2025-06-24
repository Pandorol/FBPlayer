using System;

namespace FeedBack
{
    public enum TweenType
    {
        Tween,
        Interval,
        Callback,
    } 
    
    [AttributeUsage(AttributeTargets.Class)]
    public class FeedBackAttribute : Attribute
    {
        public string Split;
        public int Order;
        public TweenType tType;

        public FeedBackAttribute(string split, int order,TweenType tweenType)
        {
            Split = split;
            Order = order;
            tType = tweenType;
        }
    }
}