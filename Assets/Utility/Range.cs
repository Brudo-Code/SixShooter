using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunchButton.Utility
{
	[System.Serializable]
	public struct Range
	{
		[SerializeField] private float low, high;

		public float Low { get => low; set => low = value; }
		public float High { get => high; set => high = value; }

		public float Center
		{
			get
			{
				return Lerp(0.5f);
			}
		}

		public Range(float low, float high)
		{
			this.low = low;
			this.high = high;
		}

		public float Lerp(float time)
		{
			return Mathf.Lerp(Low, High, time);
		}

		public float GetRandom()
		{
			return Random.Range(Low, High);
		}

		public bool Contains(float value)
		{
			return Low <= value && value <= High;
		}
	}
}
