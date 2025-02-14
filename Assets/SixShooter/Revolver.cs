using System;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
	public class Chamber
	{
		public bool HasCartridge;
		public Cartridge Cartridge;
	}

	private bool isTriggerDown = false;
	private bool isEjectorRodDown = false;

	private Chamber[] chambers = new Chamber[6];

	private float cylinderRotation;
	public float CylinderRotation
	{
		get => cylinderRotation;
		set
		{
			cylinderRotation = value % 360;
		}
	}

	public int FiringChamberIndex => Mathf.FloorToInt(CylinderRotation / 60);
	public int LoadingChamberIndex => (FiringChamberIndex + 1) % 6;
	public Chamber FiringChamber => chambers[FiringChamberIndex];
	public Chamber LoadingChamber => chambers[LoadingChamberIndex];

	private float hammerDistance;
	public float HammerDistance
	{
		get => hammerDistance;
		set
		{
			hammerDistance = Mathf.Clamp01(value);
		}
	}

	public enum HammerState { Uncocked, Halfcock, Cocked };
	public HammerState CurrentHammerState
	{
		get
		{
			if (HammerDistance < 0.5f)
			{
				return HammerState.Uncocked;
			}
			else if (HammerDistance <= 0.99f)
			{
				return HammerState.Halfcock;
			}
			else
			{
				return HammerState.Cocked;
			}
		}
	}

	private void OnTriggerDown()
	{
		// Play trigger pull sound

		switch (CurrentHammerState)
		{
			case HammerState.Uncocked:
					// Return
					break;

			case HammerState.Halfcock:
				// Play half-cock hammer return sound
				break;

			case HammerState.Cocked:
				// Play cocked hammer return sound
				HammerDistance = 0;
				StrikeChamber();
				break;

		}

		HammerDistance = 0;
	}

	private void OnEjectorRodDown()
	{
		if (LoadingChamber.HasCartridge)
		{
			return;
		}
		
		EjectCartridge();
	}

	private void EjectCartridge()
	{
		LoadingChamber.HasCartridge = false;
		LoadingChamber.Cartridge = null;
	}

	private void PullHammer(float delta)
	{
		delta = Mathf.Clamp01(delta);

		HammerState previousHammerState = CurrentHammerState;

		HammerDistance += delta;

		if (previousHammerState != CurrentHammerState)
		{
			// Play click sound depending on hammer state
			// Rotate barrel
		}
	}

	private void ReleaseHammer()
	{
		if (!isTriggerDown)
		{
			HammerDistance = Mathf.Floor(HammerDistance / 0.5f) * 0.5f;
			return;
		}

		if (CurrentHammerState == HammerState.Cocked)
		{
			StrikeChamber();
		}
		HammerDistance = 0;		
	}

	private void StrikeChamber()
	{
		if (!FiringChamber.HasCartridge)
		{
			return;
		}

		if (FiringChamber.Cartridge.IsSpent)
		{
			return;
		}

		Discharge();
	}

	private void Discharge()
	{
		FiringChamber.Cartridge.IsSpent = true;
	}

	public bool TryRotateCylinder(float delta)
	{
		if (isEjectorRodDown)
		{
			return false;
		}

		RotateCylinder(delta);
		return true;
	}

	private void RotateCylinder(float delta)
	{
		int previousFiringChamberIndex = FiringChamberIndex;

		CylinderRotation += delta;

		if (previousFiringChamberIndex != FiringChamberIndex)
		{
			// Play click sound
		}
	}

	private void ReleaseCylinder()
	{
		CylinderRotation = Mathf.Floor(CylinderRotation / 60) * 60 + 30;
	}
}
