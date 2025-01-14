﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(Conditional))]
public class ConditionalEditor : NodeEditor
{

	public override void OnBodyGUI()
	{
		serializedObject.Update();

		Conditional node = target as Conditional;
		SerializedObject so = new SerializedObject(target);

		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("arg1"), GUILayout.MinWidth(0));

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("conditionalType"), GUIContent.none);
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("result"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();

		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("arg2"), GUILayout.MinWidth(0));

		serializedObject.ApplyModifiedProperties();
	}

	public override int GetWidth()
	{
		return 100;
	}
}
