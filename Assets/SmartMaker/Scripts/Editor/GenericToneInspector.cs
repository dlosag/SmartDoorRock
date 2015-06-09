using UnityEngine;
using System.Collections;
using UnityEditor;
using SmartMaker;


[CustomEditor(typeof(GenericTone))]
public class GenericToneInspector : Editor
{
	bool foldout = true;
	ToneFrequency toneFrequency = ToneFrequency.MUTE;

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
		
		GenericTone tone = (GenericTone)target;
		
		foldout = EditorGUILayout.Foldout(foldout, "Sketch Options");
		if(foldout == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(id, new GUIContent("id"));
			EditorGUILayout.PropertyField(pin, new GUIContent("pin"));
			EditorGUI.indentLevel--;
		}

		GUILayout.BeginHorizontal();
		toneFrequency = (ToneFrequency)EditorGUILayout.EnumPopup("Tone Frequency", toneFrequency);
		int frequency = (int)toneFrequency;
		EditorGUILayout.LabelField(frequency.ToString());
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Play") == true)
			tone.toneFrequency = toneFrequency;
		if(GUILayout.Button("Mute") == true)
			tone.toneFrequency = ToneFrequency.MUTE;
		GUILayout.EndHorizontal();

		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(OnStarted);
		EditorGUILayout.PropertyField(OnStopped);

		this.serializedObject.ApplyModifiedProperties();
	}
}
