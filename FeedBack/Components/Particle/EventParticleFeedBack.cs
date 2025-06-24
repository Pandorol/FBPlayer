using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FeedBack
{
    [FeedBack("Particle", 2, TweenType.Callback)]
    public class EventParticleFeedBack: ActionFeedback
    {

        [Serializable]
        public class ParticleInfo
        {
            public string mLocation;
            public ParticleFeedCallback.PositionModes mModeType;
            public Vector3 mPosition;

            [ShowIf("mModeType", ParticleFeedCallback.PositionModes.Local), GUIColor(0, 0.6f, 0.2f)]
            public Transform mParent;

            [HideInInspector] public ParticleSystem mParticleSystem;
        }

        [SerializeField, GUIColor(0, 1, 0.2f)] protected ParticleInfo[] mParticleInfos;

        public override void RecordOrginData()
        {
            if (!lastSetingInitFlag)
            {
                lastSetingInitFlag = true;
                foreach (var info in mParticleInfos)
                {
                    if (info.mModeType == ParticleFeedCallback.PositionModes.Local)
                    {
                        if (info.mParent == null)
                        {
                            Debug.LogError("【InstantParticleFeedBack】 本地模式未指定父节点");
                        }
                    }
                    else
                    {
                        // var obj = GameObject.Instantiate(info.mPrefab, info.mPosition, Quaternion.identity);
                        // info.mParticleSystem = obj.GetComponent<ParticleSystem>();
                    }
                }
            }
        }

        public override void Reset()
        {
            foreach (var info in mParticleInfos)
            {
                info?.mParticleSystem.gameObject.SetActive(false);
                info?.mParticleSystem.Stop();
            }
        }

        public override Tween GetTween()
        {
            return null;
        }

        public override void GetActionFunc()
        {
            foreach (var info in mParticleInfos)
            {
                info?.mParticleSystem.gameObject.SetActive(true);
                info?.mParticleSystem.Stop();
            }
        }

        public override void TestPlay()
        {
        }
    }
}