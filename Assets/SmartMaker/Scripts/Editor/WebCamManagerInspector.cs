using UnityEngine;
using System.Collections;
using UnityEditor;
using SmartMaker;

[CustomEditor(typeof(WebCamManager))]
public class WebCamManagerInspector : Editor
{
	bool foldout = true;

	SerializedProperty deviceNames;
	SerializedProperty deviceName;
	SerializedProperty capWidth;
	SerializedProperty capHeight;
	SerializedProperty capFPS;
	SerializedProperty material;
	SerializedProperty uiImage;
	SerializedProperty uiText;
	SerializedProperty uiPanel;
	SerializedProperty uiItem;

	void OnEnable()
	{
		deviceNames = serializedObject.FindProperty("deviceNames");
		deviceName = serializedObject.FindProperty("deviceName");
		capWidth = serializedObject.FindProperty("capWidth");
		capHeight = serializedObject.FindProperty("capHeight");
		capFPS = serializedObject.FindProperty("capFPS");
		material = serializedObject.FindProperty("material");
		uiImage = serializedObject.FindProperty("uiImage");
		uiText = serializedObject.FindProperty("uiText");
		uiPanel = serializedObject.FindProperty("uiPanel");
		uiItem = serializedObject.FindProperty("uiItem");
	}
	
	public override void OnInspectorGUI()
	{
#if UNITY_WEBPLAYER
		EditorGUILayout.HelpBox("This component does not work on web player platform", MessageType.Error);
#endif
		this.serializedObject.Update();
		
		WebCamManager webcam = (WebCamManager)target;
		
		GUI.enabled = !webcam.isPlaying;
		
		int index = -1;
		string[] list = new string[deviceNames.arraySize];
		for(int i=0; i<list.Length; i++)
		{
			list[i] = deviceNames.GetArrayElementAtIndex(i).stringValue;
			if(deviceName.stringValue.Equals(list[i]) == true)
				index = i;
		}
		index = EditorGUILayout.Popup("Device Name", index, list);
		if(index >= 0)
			deviceName.stringValue = list[index];
		else
			deviceName.stringValue = "";
		if(GUILayout.Button("Search") == true)
			webcam.DeviceSearch();

		EditorGUILayout.PropertyField(capWidth, new GUIContent("capture Width"));
		EditorGUILayout.PropertyField(capHeight, new GUIContent("capture Height"));
		EditorGUILayout.PropertyField(capFPS, new GUIContent("capture FPS"));
		EditorGUILayout.PropertyField(material, new GUIContent("Render Material"));
		EditorGUILayout.PropertyField(uiImage, new GUIContent("Render UIImage"));

		if(webcam.isPlaying == true)
		{
			EditorGUILayout.LabelField(string.Format("Image Width:{0:d}", webcam.currentWidth));
			EditorGUILayout.LabelField(string.Format("Image Height:{0:d}", webcam.currentHeight));
		}

		GUI.enabled = true;
		if(webcam.isPlaying == true)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Pause") == true)
				webcam.Pause();
			if(GUILayout.Button("Stop") == true)
				webcam.Stop();
			EditorGUILayout.EndHorizontal();
		}
		else
		{
			if(Application.isPlaying == true)
			{
				if(GUILayout.Button("Play") == true)
					webcam.Play();
			}
		}

		foldout = EditorGUILayout.Foldout(foldout, "UI objects");
		if(foldout == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(uiText, new GUIContent("UI Text"));
			EditorGUILayout.PropertyField(uiPanel, new GUIContent("UI Panel"));
			EditorGUILayout.PropertyField(uiItem, new GUIContent("UI Item"));
			EditorGUI.indentLevel--;
		}
		
		this.serializedObject.ApplyModifiedProperties();
	}
}
