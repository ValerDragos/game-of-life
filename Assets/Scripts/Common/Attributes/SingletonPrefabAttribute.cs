using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class SingletonPrefabAttribute : Attribute 
{
	public readonly string Name;
	public readonly bool Persistent;
	
	public SingletonPrefabAttribute(string name, bool persistent) {
		Name = name;
		Persistent = persistent;
	}
	
	public SingletonPrefabAttribute(string name) {
		Name = name;
		Persistent = false;
	}
}
