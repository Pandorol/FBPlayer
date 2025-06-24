// using System;
// using System.Collections;
// using DG.Tweening;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace FeedBack
// {
//     public class TransformFeedBack : Feedback
//     {
//         [BoxGroup("基础设置"), LabelText("目标节点"),GUIColor(0, 1, 0.2f)] public Transform[] TargetTransforms;
//         [BoxGroup("基础设置"), LabelText("时间")] public float Duration = 1;
//
//         [BoxGroup("基础设置"), LabelText("Target")]
//         public Vector3 TargetVal = Vector3.zero;
//
//         [BoxGroup("参数设置"), EnumToggleButtons,HideLabel] public ConmandType TransformCommad;
//
//         [BoxGroup("动画设置"),PropertyOrder(20)] public EaseInfo AnimEase;
//
//         private Vector3[] mLastValues;
//
//
//         #region Rotate
//
//         private bool mCheckRotate
//         {
//             get { return TransformCommad == ConmandType.Rotation; }
//         }
//
//         [BoxGroup("旋转设置"), ShowIf("mCheckRotate"), SerializeField]
//         private RotateMode mRotateMode;
//
//         [BoxGroup("旋转设置"), ShowIf("mCheckRotate"), SerializeField]
//         private RotateMatrixType mRotateMatrixType;
//
//         private Tween GetRotateTween(Transform trans)
//         {
//             switch (mRotateMatrixType)
//             {
//                 case RotateMatrixType.World:
//                     return trans.DORotate(TargetVal, Duration, mRotateMode);
//                 case RotateMatrixType.Local:
//                     return trans.DOLocalRotate(TargetVal, Duration, mRotateMode);
//             }
//
//             return null;
//         }
//
//         #endregion
//
//         #region Scale
//
//         private bool mCheckScale
//         {
//             get { return TransformCommad == ConmandType.Scale; }
//         }
//
//         [BoxGroup("大小设置"), ShowIf("mCheckScale"), SerializeField]
//         private ScaleType mScaleType;
//
//         private Tween GetScaleTween(Transform trans)
//         {
//             switch (mScaleType)
//             {
//                 case ScaleType.Full:
//                     return trans.DOScale(TargetVal, Duration);
//                 case ScaleType.OnlyX:
//                     return trans.DOScaleX(TargetVal.x, Duration);
//                 case ScaleType.OnlyY:
//                     return trans.DOScaleY(TargetVal.y, Duration);
//                 case ScaleType.OnlyZ:
//                     return trans.DOScaleZ(TargetVal.z, Duration);
//             }
//
//             return null;
//         }
//
//         #endregion
//
//         #region position
//
//         private bool mCheckPosition
//         {
//             get { return TransformCommad == ConmandType.Position; }
//         }
//
//         [BoxGroup("位移设置"), ShowIf("mCheckPosition")]
//         public PositionMatrixType mpositionMatrix;
//
//         [BoxGroup("位移设置"), ShowIf("mCheckPosition")]
//         public TranslateType mTranslateType;
//
//         [BoxGroup("位移设置"), ShowIf("mCheckPosition")]
//         public float mJumpPower;
//
//         [BoxGroup("位移设置"), ShowIf("mCheckPosition")]
//         public int mNumJumps;
//
//         private Tween GetPositionTween(Transform trans)
//         {
//             switch (mTranslateType)
//             {
//                 case TranslateType.Move:
//                     if (mpositionMatrix == PositionMatrixType.World)
//                         return trans.DOMove(TargetVal, Duration);
//                     else
//                         return trans.DOLocalMove(TargetVal, Duration);
//                 case TranslateType.Jump:
//                     if (mpositionMatrix == PositionMatrixType.World)
//                         return trans.DOJump(TargetVal, mJumpPower, mNumJumps, Duration);
//                     else
//                         return trans.DOLocalJump(TargetVal, mJumpPower, mNumJumps, Duration);
//             }
//
//             return null;
//         }
//
//         #endregion
//
//
//         #region control
//
//         public override void RecordOrginData()
//         {
//             if (!lastSetingInitFlag)
//             {
//                 lastSetingInitFlag = true;
//                 mLastValues = new Vector3[TargetTransforms.Length];
//                 for (int i = 0; i < TargetTransforms.Length; i++)
//                 {
//                     switch (TransformCommad)
//                     {
//                         case ConmandType.Position:
//                             mLastValues[i] = TargetTransforms[i].position;
//                             break;
//                         case ConmandType.Rotation:
//                             mLastValues[i] = TargetTransforms[i].eulerAngles;
//                             break;
//                         case ConmandType.Scale:
//                             mLastValues[i] = TargetTransforms[i].localScale;
//                             break;
//                     }
//                 }
//             }
//         }
//
//         public override void Reset()
//         {
//             for (int i = 0; i < TargetTransforms.Length; i++)
//             {
//                 switch (TransformCommad)
//                 {
//                     case ConmandType.Position:
//                         TargetTransforms[i].position = mLastValues[i];
//                         break;
//                     case ConmandType.Rotation:
//                         TargetTransforms[i].eulerAngles = mLastValues[i];
//                         break;
//                     case ConmandType.Scale:
//                         TargetTransforms[i].localScale = mLastValues[i];
//                         break;
//                 }
//             }
//         }
//
//         public override Tween GetTween()
//         {
//             if(TargetTransforms.Length <= 0)
//                 Debug.LogError($"[TransformFeedBack:] 没有设置目标");
//             
//             var tween = DOTween.Sequence();
//             foreach (var ta in TargetTransforms)
//             {
//                 tween.Join(GetTween(ta));
//             }
//
//             switch (AnimEase.mEaseType)
//             {
//                 case EaseType.Ease:
//                     tween.SetEase(AnimEase.ease);
//                     break;
//                 case EaseType.Curve:
//                     tween.SetEase(AnimEase.curve);
//                     break;
//             }
//
//             return tween;
//         }
//
//         private Tween GetTween(Transform trans)
//         {
//             switch (TransformCommad)
//             {
//                 case ConmandType.Position:
//                     return GetPositionTween(trans);
//                 case ConmandType.Rotation:
//                     return GetRotateTween(trans);
//                 case ConmandType.Scale:
//                     return GetScaleTween(trans);
//             }
//
//             return null;
//         }
//
//
//         // [ButtonGroup("Test"), PropertyOrder(50)]
//         public override void TestPlay()
//         {
//         }
//
//         #endregion
//
//         [OnInspectorGUI]
//         private void OnInspectorGUI()
//         {
//             switch (TransformCommad)
//             {
//                 case ConmandType.Position:
//                     Name = "PositionFeedBack";
//                     break;
//                 case ConmandType.Rotation:
//                     Name = "RotationFeedBack";
//                     break;
//                 case ConmandType.Scale:
//                     Name = "ScaleFeedBack";
//                     break;
//                 default:
//                     Name = "UnKownTransformFeedBack";
//                     break;
//             }
//         }
//
//         public enum ConmandType
//         {
//             Position,
//             Rotation,
//             Scale,
//         }
//
//         public enum PositionMatrixType
//         {
//             World,
//             Local,
//         }
//
//         public enum TranslateType
//         {
//             Move,
//             Jump,
//         }
//
//         public enum RotateMatrixType
//         {
//             World,
//             Local,
//         }
//
//         private enum ScaleType
//         {
//             Full,
//             OnlyX,
//             OnlyY,
//             OnlyZ,
//         }
//     }
// }