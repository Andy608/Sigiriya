﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(46, 217, 125)]
[CreateNodeMenu("Logic/Not")]
public class Not : Node
{
	[Input(typeConstraint = TypeConstraint.Strict)] public bool arg1;
	[Output] public bool result;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		bool arg1 = GetInputValue<bool>("arg1", this.arg1);

		result = !arg1;

		return result;
	}
}