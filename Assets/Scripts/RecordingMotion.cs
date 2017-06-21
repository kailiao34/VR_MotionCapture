using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class RecordingMotion : MonoBehaviour {

    //[HideInInspector]
    public string[] bonePaths;
    public Transform[] boneReferences;
    public SteamVR_TrackedController lCtrler, rCtrler;
    public float timeStep = 0.01666f;
    public bool captureMode = false;
    
    AnimationClip newClip;
    AnimationCurve curve;
    Vector3[] curPos;
    Quaternion[] curRot;
    List<Keyframe>[] kfPosX, kfPosY, kfPosZ, kfRotX, kfRotY, kfRotZ, kfRotW;
    int cBones;
    //float tCount = 0;
    bool recordTrigger = false;

    void Awake () {
        cBones = boneReferences.Length;

        kfPosX = new List<Keyframe>[cBones]; kfPosY = new List<Keyframe>[cBones]; kfPosZ = new List<Keyframe>[cBones];
        kfRotX = new List<Keyframe>[cBones]; kfRotY = new List<Keyframe>[cBones]; kfRotZ = new List<Keyframe>[cBones];
        kfRotW = new List<Keyframe>[cBones];

        curPos = new Vector3[cBones];
        curRot = new Quaternion[cBones];

        for (int i=0; i<cBones; i++) {
            kfPosX[i] = new List<Keyframe>();
            kfPosY[i] = new List<Keyframe>();
            kfPosZ[i] = new List<Keyframe>();
            kfRotX[i] = new List<Keyframe>();
            kfRotY[i] = new List<Keyframe>();
            kfRotZ[i] = new List<Keyframe>();
            kfRotW[i] = new List<Keyframe>();
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");
        if (!AssetDatabase.IsValidFolder("Assets/Resources/MotionCapAnims"))
            AssetDatabase.CreateFolder("Assets/Resources", "MotionCapAnims");

        // 註冊按鈕事件
        lCtrler.TriggerClicked += TriggerRecording;
        rCtrler.TriggerClicked += TriggerRecording;

        //recordTrigger = true;
        //StartCoroutine(Rec());
    }

    //IEnumerator Rec() {
    //    while (true) {
            
    //        float t = Time.time - tCount;

    //        for (int i = 0; i < cBones; i++) {                  // i 為哪一個Bone

    //            //print(rot.ToString("F5"));

    //            kfPosX[i].Add(new Keyframe(t, curPos[i].x));
    //            kfPosY[i].Add(new Keyframe(t, curPos[i].y));
    //            kfPosZ[i].Add(new Keyframe(t, curPos[i].z));
    //            kfRotX[i].Add(new Keyframe(t, curRot[i].x));
    //            kfRotY[i].Add(new Keyframe(t, curRot[i].y));
    //            kfRotZ[i].Add(new Keyframe(t, curRot[i].z));
    //            kfRotW[i].Add(new Keyframe(t, curRot[i].w));
    //        }

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    private void Update() {
        for (int i = 0; i < cBones; i++) {
            curPos[i] = boneReferences[i].localPosition;
            curRot[i] = boneReferences[i].localRotation;
        }
    }

    //public Transform w;


    // 開始與結束錄製
    void TriggerRecording(object sender, ClickedEventArgs e) {
        if (recordTrigger) {                // 結束錄製並儲存
            #region ========= Stop recording and save the animations =========
            recordTrigger = false;
            print("Stop Recording Motion");

            newClip = new AnimationClip();
            for (int i = 0; i < cBones; i++) {                         // i 為哪一個Bone

                curve = new AnimationCurve(kfPosX[i].ToArray());
                newClip.SetCurve(bonePaths[i], typeof(Transform), "localPosition.x", curve);
                curve = new AnimationCurve(kfPosY[i].ToArray());
                newClip.SetCurve(bonePaths[i], typeof(Transform), "localPosition.y", curve);
                curve = new AnimationCurve(kfPosZ[i].ToArray());
                newClip.SetCurve(bonePaths[i], typeof(Transform), "localPosition.z", curve);
                curve = new AnimationCurve(kfRotX[i].ToArray());
                newClip.SetCurve(bonePaths[i], typeof(Transform), "localRotation.x", curve);
                curve = new AnimationCurve(kfRotY[i].ToArray());
                newClip.SetCurve(bonePaths[i], typeof(Transform), "localRotation.y", curve);
                curve = new AnimationCurve(kfRotZ[i].ToArray());
                newClip.SetCurve(bonePaths[i], typeof(Transform), "localRotation.z", curve);
                curve = new AnimationCurve(kfRotW[i].ToArray());
                newClip.SetCurve(bonePaths[i], typeof(Transform), "localRotation.w", curve);
                
            }
            
            AssetDatabase.CreateAsset(newClip, "Assets/Resources/MotionCapAnims/animTest.anim");
            AssetDatabase.SaveAssets();
            #endregion ==============================================

        } else {                            // 開始錄製
            //tCount = Time.time;
            recordTrigger = true;
            StartCoroutine(RecordPos());
            print("Start Recording Motion...");
        }
    }

    // 每個timeStep執行一次的執行緒
    IEnumerator RecordPos() {

        float tCount = 0;
        //Vector3 pos;
        //Quaternion rot;

        while (recordTrigger) {
            //print(curRot[2].eulerAngles);
            for (int i = 0; i < cBones; i++) {                  // i 為哪一個Bone
                //pos = boneReferences[i].position;
                //rot = boneReferences[i].rotation;

                //print(rot.ToString("F5"));

                kfPosX[i].Add(new Keyframe(tCount, curPos[i].x));
                kfPosY[i].Add(new Keyframe(tCount, curPos[i].y));
                kfPosZ[i].Add(new Keyframe(tCount, curPos[i].z));
                kfRotX[i].Add(new Keyframe(tCount, curRot[i].x));
                kfRotY[i].Add(new Keyframe(tCount, curRot[i].y));
                kfRotZ[i].Add(new Keyframe(tCount, curRot[i].z));
                kfRotW[i].Add(new Keyframe(tCount, curRot[i].w));
            }

            tCount += timeStep;
            yield return new WaitForSeconds(timeStep);
        }
    }
}
