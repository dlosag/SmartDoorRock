using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using SmartMaker;

[CustomEditor(typeof(MPU9150))]
public class MPU9150Inspector : Editor
{
	bool foldout = true;
	SerializedProperty id;
	SerializedProperty targetObject;
	SerializedProperty offsetAngles;
	SerializedProperty OnCalibrated;

	void OnEnable()
	{
		id = serializedObject.FindProperty("id");
		targetObject = serializedObject.FindProperty("target");
		offsetAngles = serializedObject.FindProperty("offsetAngles");
		OnCalibrated = serializedObject.FindProperty("OnCalibrated");
	}

	public override void OnInspectorGUI()
	{
		this.serializedObject.Update();

		MPU9150 imu = (MPU9150)target;

		foldout = EditorGUILayout.Foldout(foldout, "Sketch Options");
		if(foldout == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(id, new GUIContent("id"));
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(targetObject, new GUIContent("Target"));
		EditorGUILayout.PropertyField(offsetAngles, new GUIContent("Offset"));

		if(Application.isPlaying == true)
		{
			if(GUILayout.Button("Calibration") == true)
				imu.Calibration();
		}

		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(OnCalibrated);

		this.serializedObject.ApplyModifiedProperties();
	}
}
