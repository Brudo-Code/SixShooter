using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[System.Serializable]
public struct Optional<T>
{
	[SerializeField] private bool enabled;
	[SerializeField] private T value;

	public Optional(T initialValue)
	{
		enabled = true;
		value = initialValue;
	}

	public bool Enabled 
	{
		get => enabled; 
		set => enabled = value;
	}
	public T Value
	{
		get => value;
		set => this.value = value;
	}
}




