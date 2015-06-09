using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using SmartMaker;

[CustomEditor(typeof(ColorLED))]
public class ColorLEDInspector : Editor
{
	SerializedProperty analogRed;
	SerializedProperty analogGreen;
	SerializedProperty analogBlue;

	void OnEnable()
	{
		analogRed = serializedObject.FindProperty("analogRed");
		analogGreen = serializedObject.FindProperty("analogGreen");
		analogBlue = serializedObject.FindProperty("analogBlue");
	}

	public override void OnInspectorGUI()
	{
		this.serializedObject.Update();

		EditorGUILayout.PropertyField(analogRed, new GUIContent("Analog Red"));
		EditorGUILayout.PropertyField(analogGreen, new GUIContent("Analog Green"));
		EditorGUILayout.PropertyField(analogBlue, new GUIContent("Analog Blue"));

		this.serializedObject.ApplyModifiedProperties();

		ColorLED colorLED = (ColorLED)target;
		colorLED.color = EditorGUILayout.ColorField("Color", colorLED.color);
		colorLED.calibrationRed = EditorGUILayout.Slider("Calibration Red", colorLED.calibrationRed, -1f, 1f);
		colorLED.calibrationGreen = EditorGUILayout.Slider("Calibration Green",colorLED.calibrationGreen, -1f, 1f);
		colorLED.calibrationBlue = EditorGUILayout.Slider("Calibration Blue",colorLED.calibrationBlue, -1f, 1f);
	}
}
