﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Event/FlagNode")]
public class FlagNode : BaseNode
{
	public FlagBank.Flags throwFlag;

	// Use this for initialization
	protected override void Init()
	{
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return throwFlag; // Replace this
	}
}