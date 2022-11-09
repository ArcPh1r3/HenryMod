using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditoRecordAndSetTransforms {

    public static List<Transform> _transforms;

    public static Vector3[] _storedPositions;
    public static Quaternion[] _storedRotations;
    public static Vector3[] _storedScales;

    [MenuItem("CONTEXT/Transform/Record All Child Transforms")]
    public static void getAllTransformPositions() {

        FillTransformList();

        _storedPositions = new Vector3[_transforms.Count];
        _storedRotations = new Quaternion[_transforms.Count];
        _storedScales = new Vector3[_transforms.Count];

        int aye = 0;
        for (int i = 0; i < _transforms.Count; i++) {

            if (_transforms[i]) {
                _storedPositions[i] = _transforms[i].position;
                _storedRotations[i] = _transforms[i].rotation;
                _storedScales[i] = _transforms[i].localScale;

                aye++;
            }
        }

        Debug.Log($"{aye}/{_transforms.Count} transforms have been recorded. Turn off animation Preview and click 'Set ALL Recorded Transforms'");
    }

    private static void FillTransformList() {

        if (_transforms == null)
            _transforms = new List<Transform>();

        Transform[] children;
        for (int select = 0; select < Selection.transforms.Length; select++) {

            //NEVER do GetComponentsInChildren at runtime, unless you don't care about FPS in which case you're not a true gamer -ts
            children = Selection.transforms[select].GetComponentsInChildren<Transform>(true);

            //holy shit i'm doing the SRM thing kinda
            for (int i = 0; i < children.Length || i < _transforms.Count; i++) {;

                if (i < children.Length) {

                    if (_transforms.Count <= i) {
                        _transforms.Add(children[i]);
                    } else {
                        _transforms[i] = children[i];
                    }
                } else {
                    _transforms[i] = null;
                }
            }
        }
    }

    [CanEditMultipleObjects]
    [MenuItem("CONTEXT/Transform/Set All Recorded Transforms")]
    public static void setAllTransformPositions() {

        if (_transforms == null) {
            Debug.LogError("no transforms are recorded. use Record Transforms first");
            return;
        }

        //Undo.RecordObjects(_transforms.ToArray(), "setting transforms");

        for (int i = 0; i < _transforms.Count; i++) {

            if (_transforms[i] != null) {
                Undo.RecordObject(_transforms[i], "setting transforms");
                _transforms[i].position = _storedPositions[i];
                _transforms[i].rotation = _storedRotations[i];
                _transforms[i].localScale = _storedScales[i];
            }
        }
    }

    [MenuItem("CONTEXT/Transform/Copy Recorded To Children")]
    public static void SetAllTransformPositionsRelative() {

        Transform[] newTransforms = Selection.activeTransform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < newTransforms.Length; i++) {

            string newPath = GetTransformPath(newTransforms[i], newTransforms[0]);
            for (int j = 0; j < _transforms.Count; j++) {

                if (newPath == GetTransformPath(_transforms[j], _transforms[0])) {

                    newTransforms[i].localPosition = _transforms[j].localPosition;
                    newTransforms[i].localRotation = _transforms[j].localRotation;
                    newTransforms[i].localScale = _transforms[j].localScale;
                    continue;
                }
            }
        }
    }

    public static string GetTransformPath(Transform endTransform, Transform stopTransform = null) {

        string path = endTransform.name;
        while(endTransform.parent != null && endTransform.parent != stopTransform) {

            path = $"{endTransform.parent.name}/{path}";
            endTransform = endTransform.parent;
        }

        Debug.Log(path);
        return path;
    }
}
