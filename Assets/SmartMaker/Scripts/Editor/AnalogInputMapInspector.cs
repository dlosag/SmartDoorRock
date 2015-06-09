using UnityEngine;
using System.Collections;
using UnityEditor;
using SmartMaker;


[CustomEditor(typeof(AnalogInputMap))]
public class AnalogInputMapInspector : Editor
{
	bool foldout = true;

	SerializedProperty analogInput;
	SerializedProperty scaler;
	SerializedProperty mapSamples;

	void OnEnable()
	{
		analogInput = serializedObject.FindProperty("analogInput");
		scaler = serializedObject.FindProperty("scaler");
		mapSamples = serializedObject.FindProperty("mapSamples");
	}
	
	public override void OnInspectorGUI()
	{
		this.serializedObject.Update();
		
		AnalogInputMap aInputMap = (AnalogInputMap)target;
		
		EditorGUILayout.PropertyField(analogInput, new GUIContent("analogInput"));
		EditorGUILayout.PropertyField(scaler, new GUIContent("scaler"));
		EditorGUILayout.FloatField("Map Value", aInputMap.Value);

		foldout = EditorGUILayout.Foldout(foldout, "Map Samples");
		if(foldout == true)
		{
			EditorGUI.indentLevel++;

			SerializedProperty sample;

			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Add Sample") == true)
			{
				mapSamples.InsertArrayElementAtIndex(mapSamples.arraySize);
				sample = mapSamples.GetArrayElementAtIndex(mapSamples.arraySize - 1);
			}
			if(mapSamples.arraySize > 0)
			{
				if(GUILayout.Button("Remove All") == true)
					mapSamples.ClearArray();
			}
			EditorGUILayout.EndHorizontal();
					
			for(int i=0; i<mapSamples.arraySize; i++)
			{
				EditorGUILayout.BeginHorizontal();
				sample = mapSamples.GetArrayElementAtIndex(i);
				EditorGUILayout.PropertyField(sample, new GUIContent(string.Format("Sample {0:d}", i)));

				if(GUILayout.Button("+", GUILayout.Width(20)) == true)
				{
					mapSamples.MoveArrayElement(i, i + 1);
				}
				if(i > 0)
					GUI.enabled = true;
				else
					GUI.enabled = false;
				if(GUILayout.Button("-", GUILayout.Width(20)) == true)
				{
					mapSamples.MoveArrayElement(i, i - 1);
				}
				GUI.enabled = true;
				if(GUILayout.Button("x", GUILayout.Width(20)) == true)
				{
					mapSamples.DeleteArrayElementAtIndex(i);
					i--;
				}
				EditorGUILayout.EndHorizontal();

				if (sample.isExpanded)
				{
					foreach (SerializedProperty p in sample)
						EditorGUILayout.PropertyField (p);
				}
			}

			EditorGUI.indentLevel--;
		}
		
		if(Application.isPlaying == true)
			EditorUtility.SetDirty(target);
		
		this.serializedObject.ApplyModifiedProperties();
	}
}
