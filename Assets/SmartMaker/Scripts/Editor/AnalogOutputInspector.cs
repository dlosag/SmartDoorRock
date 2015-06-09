using UnityEngine;
using System.Collections;
using UnityEditor;
using SmartMaker;


[CustomEditor(typeof(AnalogOutput))]
public class AnalogOutputInspector : Editor
{
	bool foldout = true;
	SerializedProperty id;
	SerializedProperty pin;
	SerializedProperty OnStarted;
	SerializedProperty OnStopped;
	
	void OnEnable()
	{
		id = serializedObject.FindProperty("id");
		pin = serializedObject.FindProperty("pin");
		OnStarted = serializedObject.FindProperty("OnStarted");
		OnStopped = serializedObject.FindProperty("OnStopped");
	}
	
	public override void OnInspectorGUI()
	{
		this.serializedObject.Update();
		
		AnalogOutput aOut = (AnalogOutput)target;
		
		foldout = EditorGUILayout.Foldout(foldout, "Sketch Options");
		if(foldout == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(id, new GUIContent("id"));
			EditorGUILayout.PropertyField(pin, new GUIContent("pin"));
			EditorGUI.indentLevel--;
		}

		aOut.value = EditorGUILayout.Slider("Analog Value", aOut.value, 0f, 1f);

		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(OnStarted);
		EditorGUILayout.PropertyField(OnStopped);

		this.serializedObject.ApplyModifiedProperties();
	}
}
