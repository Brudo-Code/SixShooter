using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class UIToolkitHelper 
{
	public static void AddElementIfNecessary(VisualElement visualElement, VisualElement parent)
	{
		if (!parent.Contains(visualElement))
		{
			parent.Add(visualElement);
		}
	}

	public static void RemoveElementIfNecessary(VisualElement visualElement, VisualElement parent)
	{
		if (parent.Contains(visualElement))
		{
			parent.Remove(visualElement);
		}
	}

	public static void AddFieldIfNecessary<T>(TextValueField<T> textField, VisualElement parent)
	{
		if (!parent.Contains(textField))
		{
			parent.Add(textField);
		}
	}

	public static void RemoveFieldIfNecessary<T>(TextValueField<T> textField, VisualElement parent)
	{
		if (parent.Contains(textField))
		{
			parent.Remove(textField);
		}
	}
}
