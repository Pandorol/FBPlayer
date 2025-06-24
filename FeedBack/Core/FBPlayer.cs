using System;
using System.Collections.Generic;
using DG.Tweening;
using DOTweenExtensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.Utilities.Editor;
#endif

namespace FeedBack
{
    public class FBPlayer : MonoBehaviour
    {
        [BoxGroup("Settings")] public float InitialDelay = 0;

        //循环设置
        [BoxGroup("Settings")] public bool EnableLoop = false;

        [BoxGroup("Settings"), ShowIf("EnableLoop")]
        public int Loops = 0;

        [BoxGroup("Settings"), ShowIf("EnableLoop")]
        public LoopType AnimLoopType = LoopType.Restart;

        //time
        [BoxGroup("Settings")] public float TimeScale = 1;

        [BoxGroup("Settings")] public UpdateType UpdateTypeTime = UpdateType.Normal;

        //生命周期
        [BoxGroup("Settings")] public bool InitOnAwake = false;
        [BoxGroup("Settings")] public bool PlayOnEnable = false;
        [BoxGroup("Settings")] public bool KillOnDisable = false;

        [SerializeReference]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(ShowIndexLabels = false,
            ListElementLabelName = "Title", HideAddButton = true)]
        public List<Feedback> feedBacks = new List<Feedback>();

        [Header("Event")]
        public bool UseEvent;
        [ShowIf("UseEvent")] public UnityEvent PlayCompletEvent;
        [ShowIf("UseEvent")] public UnityEvent PlayStartEvent;


        private Sequence lastTween;

        private void Awake()
        {
            if (InitOnAwake)
            {
                Initialization();
            }
        }

        private void OnDestroy()
        {
            Kill();
        }

        private void OnEnable()
        {
            if (PlayOnEnable)
            {
                Play();
            }
        }

        private void OnDisable()
        {
            if (KillOnDisable)
            {
                Kill(false);
            }
        }

        public void Kill(bool callComplet = false)
        {
            lastTween?.SafeKill(callComplet);
            lastTween = null;
        }

        public void Initialization()
        {
            if (!EnableLoop)
            {
                Loops = 0;
            }

            foreach (var feedback in feedBacks)
            {
                feedback.RecordOrginData();
            }
        }

        public void Reset()
        {
            Kill(false);
            for (int i = feedBacks.Count - 1; i >= 0; i--)
            {
                feedBacks[i].Reset();
            }
        }

        public void Play()
        {
            Reset();

            lastTween = DOTween.Sequence();
            lastTween.SetDelay(InitialDelay);
            foreach (var feedback in feedBacks)
            {
                var attr = GetAttribute(feedback);
                if (attr.tType == TweenType.Tween)
                {
                    if (feedback.Operation == OperationType.Append)
                        lastTween.Append(feedback.GetTween());
                    else if (feedback.Operation == OperationType.Join)
                        lastTween.Join(feedback.GetTween());
                }
                else if (attr.tType == TweenType.Interval)
                {
                    lastTween.AppendInterval((feedback as IntervalFeedBack).Interval);
                }
                else if (attr.tType == TweenType.Callback)
                {
                    lastTween.AppendCallback((feedback as ActionFeedback).GetActionFunc);
                }
            }

            lastTween.SetLoops(Loops, AnimLoopType);
            lastTween.SetUpdate(UpdateTypeTime);
            lastTween.timeScale = TimeScale;
            lastTween.OnStart(OnStart);
            lastTween.OnComplete(OnComplet);
            lastTween.Play();
        }

        private FeedBackAttribute GetAttribute(Feedback feedback)
        {
            return
                Attribute.GetCustomAttribute(feedback.GetType(), typeof(FeedBackAttribute)) as FeedBackAttribute;
        }

        private void OnStart()
        {
            if (UseEvent)
            {
                PlayStartEvent.Invoke();
            }
        }
        private void OnComplet()
        {
            if (UseEvent)
            {
                PlayCompletEvent.Invoke();
            }
        }

#if UNITY_EDITOR
        Rect buttonRect;

        // private void OnTitleBarGUI()
        // {
        //     GUILayout.TextField("xxx");
        // }

        private void OnBeginListElementGUI(int index)
        {
            // SirenixEditorGUI.BeginBox(this.feedBacks[index].Name + " " + this.feedBacks[index].UniID);
        }

        private void OnEndListElementGUI(int index)
        {
            // SirenixEditorGUI.BeginBox();
        }

        [OnInspectorGUI]
        private void RepaintConstantly()
        {
            // typeIndex = UnityEditor.EditorGUILayout.Popup(typeIndex, typeNames.ToArray());
            if (UnityEngine.GUILayout.Button("Add"))
            {
                var rect = GUILayoutUtility.GetRect(new GUIContent("Show"), EditorStyles.toolbarButton);
                // UnityEditor.PopupWindow.Show(buttonRect, new FeedBackPopWindow());
                var assembly = System.Reflection.Assembly.Load("FBPlayer");
                var type = assembly.GetType("FeedBack.FeedBackWindow");
                var mainMethod = type.GetMethod("OpenWindow");
                var parameters = new object[] { gameObject };
                mainMethod.Invoke(null, parameters);
                // if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
            }
        }

        public void AddFeedBack(Type type, string typeName)
        {
            var ftype = (Feedback)Activator.CreateInstance(type);
            ftype.Name = typeName;
            feedBacks.Add(ftype);

            for (int i = 0; i < feedBacks.Count; i++)
            {
                feedBacks[i].UniID = i;
            }
        }
#endif


        [Button]
        private void TestPlay()
        {
            Play();
        }
    }
}