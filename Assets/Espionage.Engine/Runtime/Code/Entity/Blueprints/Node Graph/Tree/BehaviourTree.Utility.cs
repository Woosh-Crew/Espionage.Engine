using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Espionage.Engine.Entities;

public static class BehaviourTreeExtensions
{
	public static T Create<T>( this BehaviourTree tree ) where T : Node, new()
	{
		return tree.Create( typeof( T ) ) as T;
	}
}
