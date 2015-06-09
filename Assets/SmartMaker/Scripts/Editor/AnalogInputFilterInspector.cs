using UnityEngine;
using System.Collections;
using UnityEditor;
using SmartMaker;

[CustomEditor(typeof(AnalogInputFilter))]
public class AnalogInputFilterInspector : Editor
{
	SerializedProperty analogInput;
	SerializedProperty minValue;
	SerializedProperty maxValue;
	SerializedProperty noiseFilter;
	SerializedProperty sensitivity;
	
	void OnEnable()
	{
		analogInput = serializedObject.FindProperty("analogInput");
		minValue = serializedObject.FindProperty("minValue");
		maxValue = serializedObject.FindProperty("maxValue");
		noiseFilter = serializedObject.FindProperty("noiseFilter");
		sensitivity = serializedObject.FindProperty("sensitivity");
	}

	public override void OnInspectorGUI()
	{
		this.serializedObject.Update();
		AnalogInputFilter aInputFilter = (AnalogInputFilter)target;

		EditorGUILayout.PropertyField(analogInput, new GUIContent("analogInput"));
		EditorGUILayout.PropertyField(minValue, new GUIContent("Min Value"));
		EditorGUILayout.PropertyField(maxValue, new GUIContent("Max Value"));
		EditorGUILayout.PropertyField(noiseFilter, new GUIContent("Noise Filter"));
		if(noiseFilter.boolValue == true)
			EditorGUILayout.PropertyField(sensitivity, new GUIContent("Sensitivity"));

		if(Application.isPlaying == true)
		{
			if(GUILayout.Button("Reset") == true)
				aInputFilter.Reset();

			DrawCurve(aInputFilter.OriginValues, "Value", 0f, 1f);
			DrawCurve(aInputFilter.Values, "Filtered Value", 0f, 1f);
			DrawCurve(aInputFilter.Momentums, "Momentum", -0.5f, 0.5f);

			EditorUtility.SetDirty(target);
		}

		this.serializedObject.ApplyModifiedProperties();
	}

	public void DrawCurve(float[] values, string label, float min, float max)
	{
		EditorGUILayout.Space();

		AnimationCurve curve = new AnimationCurve();
		float lastValue = 0f;
		if(values.Length > 0)
		{
			curve.AddKey(0f, max);
			curve.AddKey(0.1f, min);
			for(int i=0; i<values.Length; i++)
				curve.AddKey(0.1f * (i + 2), values[i]);
			lastValue = values[values.Length - 1];
		}
		EditorGUILayout.LabelField(label + string.Format(" {0:F2}", lastValue));
		GUI.enabled = false;
		EditorGUILayout.CurveField(curve, GUILayout.Height(100));
		GUI.enabled = true;
	}
}