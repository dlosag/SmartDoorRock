using UnityEngine;
using System.Collections;
using UnityEditor;
using SmartMaker;


[CustomEditor(typeof(DigitalInput))]
public class DigitalInputInspector : Editor
{
	bool foldout = true;
	SerializedProperty id;
	SerializedProperty pin;
	SerializedProperty pullup;
	SerializedProperty OnStarted;
	SerializedProperty OnExcuted;
	SerializedProperty OnStopped;
	SerializedProperty OnChangedValue;
	
	void OnEnable()
	{
		id = serializedObject.FindProperty("id");
		pin = serializedObject.FindProperty("pin");
		pullup = serializedObject.FindProperty("pullup");
		OnStarted = serializedObject.FindProperty("OnStarted");
		OnExcuted = serializedObject.FindProperty("OnExcuted");
		OnStopped = serializedObject.FindProperty("OnStopped");
		OnChangedValue = serializedObject.FindProperty("OnChangedValue");
	}
	
	public override void OnInspectorGUI()
	{
		this.serializedObject.Update();
		
		DigitalInput dInput = (DigitalInput)target;
		
		foldout = EditorGUILayout.Foldout(foldout, "Sketch Options");
		if(foldout == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(id, new GUIContent("id"));
			EditorGUILayout.PropertyField(pin, new GUIContent("pin"));
			EditorGUILayout.PropertyField(pullup, new GUIContent("pullup"));
			EditorGUI.indentLevel--;
		}
		dInput.autoUpdate = EditorGUILayout.Toggle("Auto update", dInput.autoUpdate);

		int index = 0;
		if(dInput.Value == true)
			index = 1;
		GUILayout.SelectionGrid(index, new string[] {"FALSE", "TRUE"}, 2);

		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(OnStarted);
		EditorGUILayout.PropertyField(OnExcuted);
		EditorGUILayout.PropertyField(OnStopped);
		EditorGUILayout.PropertyField(OnChangedValue);

		if(Application.isPlaying == true && dInput.autoUpdate == true)
			EditorUtility.SetDirty(target);

		this.serializedObject.ApplyModifiedProperties();
	}
}
