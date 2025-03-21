using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTimer : BaseTimer
{
	[SerializeField] private float targetTime;
	public float TargetTime { get => targetTime; set => targetTime = value; }

	public virtual float TimeLeft
	{
		get => TargetTime - timer;
	}

	public SwitchTimer(float targetTime, DeltaTimeMode deltaTimeMode = DeltaTimeMode.Fixed) : base(deltaTimeMode)
	{
		this.TargetTime = targetTime;
	}

	public bool PassedTime
	{
		get => timer >= TargetTime;
	}

	public void PassTime()
	{
		timer = TargetTime;
	}

	public static implicit operator bool(SwitchTimer timerObj)
	{
		return timerObj.PassedTime;
	}
}
