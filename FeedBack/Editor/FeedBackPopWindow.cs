

namespace FeedBack
{
#if UNITY_EDITOR
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    
    public class FeedBackPopWindow : PopupWindowContent
    {
        public struct FeedbackTypePair
        {
            public Type fbType;
            public string fbName;
        }

        List<FeedbackTypePair> mTtypesAndNames = new List<FeedbackTypePair>();
        private List<string> typeNames = new List<string>();
        private int typeIndex = 0;
        private static GameObject CurGameObject;


        public override void OnOpen()
        {
            var types = (from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where assemblyType.IsSubclassOf(typeof(Feedback))
                select assemblyType).ToList();
            mTtypesAndNames.Clear();
            typeNames.Clear();
            for (int i = 0; i < types.Count; i++)
            {
                FeedbackTypePair newtype = new FeedbackTypePair();
                newtype.fbType = types[i];
                newtype.fbName = types[i].Name;
                mTtypesAndNames.Add(newtype);
                typeNames.Add(newtype.fbName);
            }
        }

        public override void OnGUI(Rect rect)
        {
            for (int i = 0; i < mTtypesAndNames.Count; i++)
            {
                if (GUILayout.Button(mTtypesAndNames[i].fbName))
                {
                    Debug.Log(mTtypesAndNames[i].fbName);
                    Debug.Log(CurGameObject);
                    if (CurGameObject && CurGameObject.GetComponent<FBPlayer>() != null)
                    {
                        CurGameObject.GetComponent<FBPlayer>()
                            .AddFeedBack(mTtypesAndNames[i].fbType, mTtypesAndNames[i].fbName);
                    }
                }
            }
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 500);
        }

        public static void Show(Rect buttonRect, GameObject selectGameObject)
        {
            CurGameObject = selectGameObject;
            PopupWindow.Show(buttonRect, new FeedBackPopWindow());
        }
    }
#endif
}