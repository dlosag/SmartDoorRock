using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using SmartMaker;

[CustomEditor(typeof(AnalogInputDrag))]
public class AnalogInputDragInspector : Editor
{
	SerializedProperty analogInput;
	SerializedProperty dragMinRatio;
	SerializedProperty dragMaxRatio;
	SerializedProperty dragForceScaler;
	SerializedProperty OnDragStart;
	SerializedProperty OnDragMove;
	SerializedProperty OnDragEnd;
	
	void OnEnable()
	{
		analogInput = serializedObject.FindProperty("analogInput");
		dragMinRatio = serializedObject.FindProperty("dragMinRatio");
		dragMaxRatio = serializedObject.FindProperty("dragMaxRatio");
		dragForceScaler = serializedObject.FindProperty("dragForceScaler");
		OnDragStart = serializedObject.FindProperty("OnDragStart");
		OnDragMove = serializedObject.FindProperty("OnDragMove");
		OnDragEnd = serializedObject.FindProperty("OnDragEnd");
	}

	public override void OnInspectorGUI()
	{
		this.serializedObject.Update();

		AnalogInputDrag aInputDrag = (AnalogInputDrag)target;

		EditorGUILayout.PropertyField(analogInput, new GUIContent("analogInput"));
		EditorGUILayout.PropertyField(dragMinRatio, new GUIContent("dragMinRatio"));
		EditorGUILayout.PropertyField(dragMaxRatio, new GUIContent("dragMaxRatio"));
		EditorGUILayout.PropertyField(dragForceScaler, new GUIContent("dragForceScaler"));

		EditorGUILayout.LabelField("Is Dragging", aInputDrag.isDragging.ToString());
		EditorGUILayout.LabelField("Value", aInputDrag.Value.ToString("F2"));
		EditorGUILayout.LabelField("DragForce", aInputDrag.DragForce.ToString("F2"));

		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(OnDragStart);
		EditorGUILayout.PropertyField(OnDragMove);
		EditorGUILayout.PropertyField(OnDragEnd);
		
		if(Application.isPlaying == true)
			EditorUtility.SetDirty(target);

		this.serializedObject.ApplyModifiedProperties();
	}
}
