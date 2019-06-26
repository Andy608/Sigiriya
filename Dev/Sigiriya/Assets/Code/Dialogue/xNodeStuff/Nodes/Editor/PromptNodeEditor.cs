﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(PromptNode))]
public class PromptNodeEditor : NodeEditor
{
	public bool openFoldout;
	Sprite m_sprite;
	[SerializeField] public UnityEngine.UI.Image pic;


	public override void OnBodyGUI()
	{
		serializedObject.Update();

		PromptNode node = target as PromptNode;

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();



		GUILayout.BeginVertical();
		openFoldout = EditorGUILayout.Foldout(openFoldout, "Extra Connections:", EditorStyles.boldFont);
		if (openFoldout)
		{
			//TODO: maybe try to color these?
			NodeEditorGUILayout.PortField(new GUIContent("Interrupt Connection"), target.GetOutputPort("interruptConnection"), GUILayout.MinWidth(0));
			NodeEditorGUILayout.PortField(new GUIContent("Checkpoint Connection"), target.GetOutputPort("checkpointConnection"), GUILayout.MinWidth(0));
			NodeEditorGUILayout.PortField(new GUIContent("Exit Connection"), target.GetOutputPort("exitConnection"), GUILayout.MinWidth(0));
			//TODO: call this in window editor instead of node editor class
			//NodeEditorWindow.current.Repaint();
		}
		GUILayout.EndVertical();

		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("prompt"), GUIContent.none);
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("time"), new GUIContent("Time"));

		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("speakerPic"), new GUIContent("Speaker Image"));
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("usedSprite"), new GUIContent("Speaker Image"));

		NodeEditorGUILayout.InstancePortList("responses", typeof(ResponseNode), serializedObject, XNode.NodePort.IO.Output, XNode.Node.ConnectionType.Override);
		//NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("responses"));

		serializedObject.ApplyModifiedProperties();
	}

	public override int GetWidth()
	{
		return 300;
	}
}