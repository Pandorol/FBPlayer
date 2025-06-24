namespace FeedBack
{
#if UNITY_EDITOR
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEngine;

    public class FeedBackWindow : OdinEditorWindow
    {
        public struct FeedbackTypePair
        {
            public Type fbType;
            public string fbName;
            public string fbPath;
            public int Order;
        }

        private class SpliteFeedBack
        {
            public string fbPath;
            public List<FeedbackTypePair> TypePairs;

            public SpliteFeedBack(string fbPath)
            {
                this.fbPath = fbPath;
                TypePairs = new List<FeedbackTypePair>();
            }
        }

        private GUIStyle mGUIStyle;
        private Dictionary<string, SpliteFeedBack> mFeedBacks;
        List<FeedbackTypePair> mTtypesAndNames = new List<FeedbackTypePair>();
        private List<string> typeNames = new List<string>();
        private int typeIndex = 0;
        private static GameObject CurGameObject;

        public static void OpenWindow(GameObject selectGameObject)
        {
            var window = GetWindow<FeedBackWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("FeedBack Select Window");

            CurGameObject = selectGameObject;
        }

        protected override void OnEnable()
        {
            mGUIStyle = new GUIStyle();
            mGUIStyle.alignment = TextAnchor.MiddleCenter;
            mGUIStyle.normal.textColor = Color.white;

            mFeedBacks = new Dictionary<string, SpliteFeedBack>();
            mTtypesAndNames = new List<FeedbackTypePair>();
            typeNames = new List<string>();

            var types = (from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where assemblyType.IsSubclassOf(typeof(Feedback)) && !assemblyType.IsAbstract
                select assemblyType).ToList();

            for (int i = 0; i < types.Count; i++)
            {
                FeedbackTypePair newtype = new FeedbackTypePair();
                newtype.fbType = types[i];
                newtype.fbName = types[i].Name;
                FeedBackAttribute attribute =
                    Attribute.GetCustomAttribute(newtype.fbType, typeof(FeedBackAttribute)) as FeedBackAttribute;
                newtype.fbPath = attribute.Split;
                newtype.Order = attribute.Order;

                if (!mFeedBacks.ContainsKey(newtype.fbPath))
                {
                    mFeedBacks[newtype.fbPath] = new SpliteFeedBack(newtype.fbPath);
                }

                mFeedBacks[newtype.fbPath].TypePairs.Add(newtype);
            }

            foreach (var pair in mFeedBacks)
            {
                pair.Value.TypePairs.Sort((x, y) => { return x.Order.CompareTo(y.Order); });
            }

            foreach (var pair in mFeedBacks)
            {
                mTtypesAndNames.AddRange(pair.Value.TypePairs);
                foreach (var t in pair.Value.TypePairs)
                {
                    typeNames.Add(t.fbName);
                }
            }
        }


        protected override void DrawEditors()
        {
            EditorGUILayout.BeginVertical();
            string lastPath = "";
            foreach (var tn in mTtypesAndNames)
            {
                if (tn.fbPath != lastPath)
                {
                    lastPath = tn.fbPath;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(tn.fbPath, mGUIStyle);
                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button(tn.fbName))
                {
                    if (CurGameObject && CurGameObject.GetComponent<FBPlayer>() != null)
                    {
                        CurGameObject.GetComponent<FBPlayer>()
                            .AddFeedBack(tn.fbType, tn.fbName);

                        Close();
                    }
                }
            }

            EditorGUILayout.EndVertical();

            //     for (int i = 0; i < mTtypesAndNames.Count; i++)
            //     {
            //         if (GUILayout.Button(mTtypesAndNames[i].fbName))
            //         {
            //             if (CurGameObject && CurGameObject.GetComponent<FBPlayer>() != null)
            //             {
            //                 CurGameObject.GetComponent<FBPlayer>()
            //                     .AddFeedBack(mTtypesAndNames[i].fbType, mTtypesAndNames[i].fbName);
            //
            //                 Close();
            //             }
            //         }
            //     }
        }
    }
#endif
}