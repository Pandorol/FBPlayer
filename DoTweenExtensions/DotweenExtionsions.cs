using DG.Tweening;

namespace DOTweenExtensions
{
    public static class DotweenExtionsions
    {
        public static void SafeKill(this Tween tween, bool complete = false)
        {
            if (tween != null && tween.IsActive() && tween.IsPlaying())
                tween.Kill(complete);
        }
        
        public static void SafeKill(this Tweener tween, bool complete = false)
        {
            if (tween != null && tween.IsActive() && tween.IsPlaying())
                tween.Kill(complete);
        }
        
        public static void SafeKill(this Sequence tween, bool complete = false)
        {
            if (tween != null && tween.IsActive() && tween.IsPlaying())
                tween.Kill(complete);
        }
    }
}