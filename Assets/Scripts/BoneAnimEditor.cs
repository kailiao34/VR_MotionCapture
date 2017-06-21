using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(RecordingMotion))]
public class BoneAnimEditor : Editor {
    
    //Assets/Modles/Robot Kyle/Model/Robot Kyle.fbx

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        RecordingMotion rm = target as RecordingMotion;

        //GUIStyle style = new GUIStyle();

        //// Horizontal Line
        //EditorGUILayout.Space();
        //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //EditorGUILayout.Space();

        //// Path Field
        //bat.path = EditorGUILayout.TextField("Model Path: ", bat.path);
        //// Avatar Field
        //bat.avatar = (Avatar)EditorGUILayout.ObjectField("Avatar: ", bat.avatar, typeof(Avatar), true);
        // Excute Button
        if (GUILayout.Button("Get Bones")) {

            Animator animator = rm.gameObject.GetComponent<Animator>();
            List<Transform> boneList = new List<Transform>();
            Transform tmpTrans;
            Transform root = rm.transform;

            foreach(HumanBodyBones bi in System.Enum.GetValues(typeof(HumanBodyBones))) {
                tmpTrans = animator.GetBoneTransform(bi);
                if (tmpTrans != null)
                    boneList.Add(tmpTrans);
            }

            rm.boneReferences = boneList.ToArray();
            rm.bonePaths = new string[boneList.Count];
            for (int i=0; i<boneList.Count; i++) {
                rm.bonePaths[i] = GetChildPath(root, boneList[i]);
            }

            //int bi = 0;
            //while (true) {
            //    Transform t = animator.GetBoneTransform((HumanBodyBones)bi);
            //    if (t == null)
            //        break;
                
            //    Debug.Log();
            //    bi++
            //}

            #region ========== Assigning Bone Treansforms ===========
            //bat.boneReferences.pelvis = animator.GetBoneTransform(HumanBodyBones.Hips);
            //bat.boneReferences.spine = animator.GetBoneTransform(HumanBodyBones.Spine);
            //bat.boneReferences.chest = animator.GetBoneTransform(HumanBodyBones.Chest);
            //bat.boneReferences.neck = animator.GetBoneTransform(HumanBodyBones.Neck);
            //bat.boneReferences.head = animator.GetBoneTransform(HumanBodyBones.Head);
            //bat.boneReferences.leftShoulder = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
            //bat.boneReferences.leftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            //bat.boneReferences.leftForearm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            //bat.boneReferences.leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            //bat.boneReferences.rightShoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
            //bat.boneReferences.rightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            //bat.boneReferences.rightForearm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
            //bat.boneReferences.rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
            //bat.boneReferences.leftThigh = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            //bat.boneReferences.leftCalf = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
            //bat.boneReferences.leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            //bat.boneReferences.leftToes = animator.GetBoneTransform(HumanBodyBones.LeftToes);
            //bat.boneReferences.rightThigh = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
            //bat.boneReferences.rightCalf = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
            //bat.boneReferences.rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            //bat.boneReferences.rightToes = animator.GetBoneTransform(HumanBodyBones.RightToes);
            #endregion ==================================================

            

            // if there was no Animator component, then delete the one we created
            //if (deleteAnimator)
            //    DestroyImmediate(animator);

            // Set Model's animation type to Generic
            //ModelImporter model = (ModelImporter)AssetImporter.GetAtPath(bat.path);
            //model.animationType = ModelImporterAnimationType.Generic;

        }

    }

    string GetChildPath(Transform root, Transform t) {

        string s = "";

        while (t != null & t != root) {
            s = t.name + "/" + s;
            t = t.parent;
        }

        s = s.Substring(0, s.Length - 1);

        return s;
    }
}
