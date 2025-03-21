using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntervalTimer : BaseTimer
{

	public IntervalTimer(DeltaTimeMode deltaTimeMode = DeltaTimeMode.Fixed)
	{
		this.deltaTimeMode = deltaTimeMode;
	}

	public bool PassedInterval(float interval)
	{
		return (timer % interval) < ((timer - LatestTick) % interval);
	}

}
