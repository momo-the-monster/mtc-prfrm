using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MidiAnimator))]
public class MidiAnimatorEditor : Editor {

    Animator animator;

    SerializedProperty key0Animation;
    SerializedProperty key1Animation;
    SerializedProperty key2Animation;
    SerializedProperty key3Animation;
    SerializedProperty key4Animation;
    SerializedProperty key5Animation;
    SerializedProperty key6Animation;
    SerializedProperty key7Animation;
    SerializedProperty key8Animation;
    SerializedProperty key9Animation;
    SerializedProperty key10Animation;
    SerializedProperty key11Animation;

    private void OnEnable()
    {
        key0Animation = serializedObject.FindProperty("key0Animation");
        key1Animation = serializedObject.FindProperty("key1Animation");
        key2Animation = serializedObject.FindProperty("key2Animation");
        key3Animation = serializedObject.FindProperty("key3Animation");
        key4Animation = serializedObject.FindProperty("key4Animation");
        key5Animation = serializedObject.FindProperty("key5Animation");
        key6Animation = serializedObject.FindProperty("key6Animation");
        key7Animation = serializedObject.FindProperty("key7Animation");
        key8Animation = serializedObject.FindProperty("key8Animation");
        key9Animation = serializedObject.FindProperty("key9Animation");
        key10Animation = serializedObject.FindProperty("key10Animation");
        key11Animation = serializedObject.FindProperty("key11Animation");
    }

    public override void OnInspectorGUI()
    {
        if(animator == null)
        {
            animator = Selection.activeGameObject.GetComponent<Animator>();
        }

         serializedObject.Update();

        int value0 = key0Animation.intValue;
        int value1 = key1Animation.intValue;
        int value2 = key2Animation.intValue;
        int value3 = key3Animation.intValue;
        int value4 = key4Animation.intValue;
        int value5 = key5Animation.intValue;
        int value6 = key6Animation.intValue;
        int value7 = key7Animation.intValue;
        int value8 = key8Animation.intValue;
        int value9 = key9Animation.intValue;
        int value10 = key10Animation.intValue;
        int value11 = key11Animation.intValue;

        var optionsList = new List<string>();
        foreach (var item in animator.runtimeAnimatorController.animationClips)
        {
            optionsList.Add(item.name);
        }

        key0Animation.intValue = EditorGUILayout.Popup("C", value0, optionsList.ToArray());
        key1Animation.intValue = EditorGUILayout.Popup("C#", value1, optionsList.ToArray());
        key2Animation.intValue = EditorGUILayout.Popup("D", value2, optionsList.ToArray());
        key3Animation.intValue = EditorGUILayout.Popup("D#", value3, optionsList.ToArray());
        key4Animation.intValue = EditorGUILayout.Popup("E", value4, optionsList.ToArray());
        key5Animation.intValue = EditorGUILayout.Popup("F", value5, optionsList.ToArray());
        key6Animation.intValue = EditorGUILayout.Popup("F#", value6, optionsList.ToArray());
        key7Animation.intValue = EditorGUILayout.Popup("G", value7, optionsList.ToArray());
        key8Animation.intValue = EditorGUILayout.Popup("G#", value8, optionsList.ToArray());
        key9Animation.intValue = EditorGUILayout.Popup("A", value9, optionsList.ToArray());
        key10Animation.intValue = EditorGUILayout.Popup("A#", value10, optionsList.ToArray());
        key11Animation.intValue = EditorGUILayout.Popup("B", value11, optionsList.ToArray());

        serializedObject.ApplyModifiedProperties();
    }
}
