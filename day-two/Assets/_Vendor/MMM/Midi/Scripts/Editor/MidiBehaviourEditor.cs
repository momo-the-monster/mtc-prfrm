using UnityEngine;
using UnityEditor;
using MMM.Midi;
using System.Collections.Generic;

[CustomEditor(typeof(MidiBehaviour),true)]
[CanEditMultipleObjects]
public class MidiBehaviourEditor : Editor {

    // Properties serialized from MidiBehaviour
    SerializedProperty durationIn;
    SerializedProperty durationOut;
    SerializedProperty magnitude;

    // Internal Curve used to visualize duration + magnitude
    AnimationCurve curve;

    void OnEnable()
    {
        durationIn = serializedObject.FindProperty("durationIn");
        durationOut = serializedObject.FindProperty("durationOut");
        magnitude = serializedObject.FindProperty("magnitude");
        curve = new AnimationCurve(GetKeyframesFromParams());
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        // Build Curve visualization
        curve.keys = GetKeyframesFromParams();
        Color curveColor = Color.HSVToRGB(magnitude.floatValue % 10 / 10, 0.7f, 1);
        Rect curveRect = new Rect(0, 0, durationIn.floatValue + durationOut.floatValue, magnitude.floatValue);

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 30;
        EditorGUILayout.PropertyField(durationIn, new GUIContent("In"));
        EditorGUILayout.CurveField(curve, curveColor, curveRect);
        EditorGUILayout.PropertyField(durationOut, new GUIContent("Out"));
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(magnitude);
        EditorGUILayout.Space();
        serializedObject.ApplyModifiedProperties();
    }

    public Keyframe[] GetKeyframesFromParams()
    {
        float durationInValue = durationIn.floatValue;
        float durationOutValue = durationOut.floatValue;
        float magnitudeValue = magnitude.floatValue;
        float curveTime = 0;
        float curveValue = 0;

        var resultList = new List<Keyframe>();
        resultList.Add(new Keyframe(curveTime, curveValue));
        curveTime += durationInValue;
        curveValue = magnitudeValue;
        resultList.Add(new Keyframe(curveTime, curveValue));
        resultList.Add(new Keyframe(curveTime, curveValue));
        curveTime += durationOutValue;
        curveValue = 0;
        resultList.Add(new Keyframe(curveTime, curveValue));
        return resultList.ToArray();
    }

    void UpdateKeyframesFromParams()
    {
        curve.MoveKey(1, new Keyframe(durationIn.floatValue, magnitude.floatValue));
        curve.MoveKey(2, new Keyframe(durationOut.floatValue, 0));
    }
}
