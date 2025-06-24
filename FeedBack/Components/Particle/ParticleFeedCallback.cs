using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FeedBack
{
    [FeedBack("Particle", 0,TweenType.Callback)]
    public class ParticleFeedCallback : ActionFeedback
    {
        public enum Modes
        {
            Active,
            Instantiate,
        }

        public enum PositionModes
        {
            World,
            Local,
        }

        [SerializeField, GUIColor(0, 1, 0.2f)] protected ParticleSystem[] mParticleSystems;
        // [SerializeField] protected PositionModes mModeType = PositionModes.World;
        //
        // [SerializeField, ShowIf("mModeType", PositionModes.World)]
        // protected Vector3 mWoldPostion;
        //
        // [SerializeField, ShowIf("mModeType", PositionModes.Local), GUIColor(0, 1, 0.2f)]
        // protected Transform mParent;
        //
        // [SerializeField, ShowIf("mModeType", PositionModes.Local)]
        // protected Vector3 mLocalPostion;

        public override void RecordOrginData()
        {
            if (!lastSetingInitFlag)
            {
                lastSetingInitFlag = true;
            }
        }

        public override void Reset()
        {
            foreach (var particle in mParticleSystems)
            {
                particle.gameObject.SetActive(false);
                particle.Stop();
            }
        }

        public override Tween GetTween()
        {
            return null;
        }
        
        public override void GetActionFunc()
        {
            Debug.Log("mParticleSystems");
            foreach (var particle in mParticleSystems)
            {
                particle.gameObject.SetActive(true);
                particle.Play();
            }
        }
        
        public override void TestPlay()
        {
        }


    }
}