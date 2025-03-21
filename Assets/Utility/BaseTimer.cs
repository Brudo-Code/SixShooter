using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTimer
{
	public enum DeltaTimeMode { Fixed, Scaled, Unscaled };
	[SerializeField] protected DeltaTimeMode deltaTimeMode = DeltaTimeMode.Fixed;
	protected float timer;
	public float LatestTick
	{
		get;
		private set;
	}
	public float LastTickMoment
	{
		get;
		protected set;
	}

	public BaseTimer(DeltaTimeMode deltaTimeMode = DeltaTimeMode.Fixed)
	{
		this.deltaTimeMode = deltaTimeMode;
	}

	public virtual float TimeElapsed
	{
		get => timer;
	}

	public virtual void Tick()
	{
		float tick = GetDeltaTime();
		timer += tick;
		LatestTick = tick;

		switch (deltaTimeMode)
		{
			case DeltaTimeMode.Fixed:
				LastTickMoment = Time.fixedDeltaTime;
				break;

				case DeltaTimeMode.Scaled:
				LastTickMoment = Time.time;
				break;

				case DeltaTimeMode.Unscaled:
				LastTickMoment = Time.unscaledTime;
				break;
		}
	}

	private float GetDeltaTime()
	{
		float deltaTime = 0;
		switch (deltaTimeMode)
		{
			case DeltaTimeMode.Fixed:
				deltaTime = Time.fixedDeltaTime;
				break;

				case DeltaTimeMode.Scaled:
					deltaTime = Time.deltaTime; 
					break;

			case DeltaTimeMode.Unscaled:
				deltaTime = Time.unscaledDeltaTime;
				break;
		}

		return deltaTime;
	}

	public virtual void Reset()
	{
		timer = 0.0f;
	}
}
