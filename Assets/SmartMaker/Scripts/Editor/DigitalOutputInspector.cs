using UnityEngine;
using System.Collections;
using UnityEditor;
using SmartMaker;


[CustomEditor(typeof(DigitalOutput))]
public class DigitalOutputInspector : Editor
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
		
		DigitalOutput dOut = (DigitalOutput)target;

		foldout = EditorGUILayout.Foldout(foldout, "Sketch Options");
		if(foldout == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(id, new GUIContent("id"));
			EditorGUILayout.PropertyField(pin, new GUIContent("pin"));
			EditorGUI.indentLevel--;
		}

		int index = 0;
		if(dOut.value == true)
			index = 1;
		int newIndex = GUILayout.SelectionGrid(index, new string[] {"FALSE", "TRUE"}, 2);
		if(index != newIndex)
		{
			if(newIndex == 0)
				dOut.value = false;
			else
				dOut.value = true;
		}

		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(OnStarted);
		EditorGUILayout.PropertyField(OnStopped);
		
		this.serializedObject.ApplyModifiedProperties();
	}
}
