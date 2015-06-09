using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using UnityEditor.Events;
using SmartMaker;

[CustomEditor(typeof(ArduinoApp))]
public class ArduinoAppInspector : Editor
{
	bool foldout = true;

	SerializedProperty commObject;
	SerializedProperty timeoutSec;
	SerializedProperty uartNum;
	SerializedProperty uartBaudrate;
	SerializedProperty OnConnected;
	SerializedProperty OnConnectionFailed;
	SerializedProperty OnDisconnected;

	void OnEnable()
	{
		commObject = serializedObject.FindProperty("commObject");
		timeoutSec = serializedObject.FindProperty("timeoutSec");
		uartNum = serializedObject.FindProperty("uartNum");
		uartBaudrate = serializedObject.FindProperty("uartBaudrate");
		OnConnected = serializedObject.FindProperty("OnConnected");
		OnConnectionFailed = serializedObject.FindProperty("OnConnectionFailed");
		OnDisconnected = serializedObject.FindProperty("OnDisconnected");
	}

	public override void OnInspectorGUI()
	{
		this.serializedObject.Update();

		ArduinoApp arduino = (ArduinoApp)target;

		if(Application.isPlaying == false)
		{
			EditorGUILayout.HelpBox("To connect the board is only possible in Play mode.", MessageType.Info);
			if(GUILayout.Button("Create Sketch") == true)
				CreateSketch(EditorUtility.SaveFilePanel("Create Sketch", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "", "ino"));

			foldout = EditorGUILayout.Foldout(foldout, "Sketch Options");
			if(foldout == true)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(uartNum, new GUIContent("UART (Serial_)"));
				EditorGUILayout.PropertyField(uartBaudrate, new GUIContent("UART Baudrate"));
				EditorGUI.indentLevel--;
			}
		}
		else
		{
			if(arduino.commObject != null)
			{
				if(arduino.Connected == true)
				{
					if(GUILayout.Button("Disconnect") == true)
						arduino.Disconnect();
				}
				else
				{
					if(GUILayout.Button("Connect") == true)
						arduino.Connect();
				}
				
				EditorUtility.SetDirty(target);
			}
			else
			{
				EditorGUILayout.HelpBox("CommObject is Null!", MessageType.Error);
			}
		}

		EditorGUILayout.PropertyField(commObject, new GUIContent("CommObject"));
		EditorGUILayout.PropertyField(timeoutSec, new GUIContent("Timeout(sec)"));

		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(OnConnected);
		EditorGUILayout.PropertyField(OnConnectionFailed);
		EditorGUILayout.PropertyField(OnDisconnected);

		this.serializedObject.ApplyModifiedProperties();
	}

	private void CreateSketch(string file)
	{
		if(file == null)
			return;

		if(file.Length == 0)
			return;

		ArduinoApp arduino = (ArduinoApp)target;
		AppAction[] actions = arduino.appActions;
		StringBuilder source = new StringBuilder();

		// Check id duplications
		for(int i=0; i<actions.Length; i++)
		{
			for(int j=i+1; j<actions.Length; j++)
			{
				if(actions[i].id == actions[j].id)
				{
					Debug.LogError("AppAction ID is duplicated!");
					return;
				}
			}
		}

		// #Includes
		List<Type> types = new List<Type>();
		List<string> exIncludes = new List<string>();
		foreach(AppAction action in actions)
		{
			Type type = action.GetType();
			if(types.IndexOf(type) < 0)
			{
				types.Add(type);
				string[] includes = action.SketchIncludes();
				if(includes != null)
				{
					foreach(string include in includes)
					{
						if(exIncludes.IndexOf(include) < 0)
							exIncludes.Add(include);
					}
				}
			}
		}
		foreach(string include in exIncludes)
			source.AppendLine(include);
		source.AppendLine("#include \"UnityApp.h\"");
		foreach(Type type in types)
			source.AppendLine(string.Format("#include \"{0}.h\"", type.Name));
		source.AppendLine();

		// Declarations
		foreach(AppAction action in actions)
			source.AppendLine(action.SketchDeclaration());
		source.AppendLine();

		// void Setup()
		source.AppendLine("void setup()");
		source.AppendLine("{");
		foreach(AppAction action in actions)
			source.AppendLine(string.Format("  UnityApp.attachAction((AppAction*)&{0});", action.SketchVarName));
		if(arduino.uartNum > 0)
			source.AppendLine(string.Format("  UnityApp.attachSerial(&Serial{0:d});", arduino.uartNum));
		source.AppendLine(string.Format("  UnityApp.begin({0:d});", arduino.uartBaudrate));
		source.AppendLine("}");
		source.AppendLine();

		// void loop()
		source.AppendLine("void loop()");
		source.AppendLine("{");
		source.AppendLine("  UnityApp.process();");
		source.AppendLine("}");

		// Create source
		string path = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
		Directory.CreateDirectory(path);
		StreamWriter sw = new StreamWriter(Path.Combine(path, Path.GetFileName(file)));
		sw.Write(source.ToString());
		sw.Close();

		string srcPath = "Assets/SmartMaker/Arduino";
		try
		{
			CopyLibrary("UnityApp", srcPath, path);
			CopyLibrary("AppAction", srcPath, path);
			foreach(Type type in types)
			{
				CopyLibrary(type.Name, srcPath, path);
				string subPath = Path.Combine(srcPath, type.Name);
				if(Directory.Exists(subPath) == true)
				{
					string[] subFiles = Directory.GetFiles(subPath);
					foreach(string subFile in subFiles)
						File.Copy(subFile, Path.Combine(path, Path.GetFileName(subFile)), true);
				}
			}
		}
		catch(Exception)
		{
			Debug.LogError("Copy error! It must be in the \"Assetes/SmartMaker/Arduino\" path.");
		}
	}

	private void CopyLibrary(string name, string srcPath, string destPath)
	{
		File.Copy(Path.Combine(srcPath, name + ".h"), Path.Combine(destPath, name + ".h"), true);
		File.Copy(Path.Combine(srcPath, name + ".cpp"), Path.Combine(destPath, name + ".cpp"), true);
	}
}
