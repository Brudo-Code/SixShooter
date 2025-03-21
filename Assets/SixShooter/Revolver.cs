using System;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
	[Serializable]
	public class Chamber
	{
		private bool hasCartridge;
		public bool HasCartridge => hasCartridge;

		private Cartridge cartridge;
		public Cartridge Cartridge => cartridge;

		public void Empty()
		{
			this.hasCartridge = false;
			this.cartridge = null;
		}

		public void Insert(Cartridge newCartridge)
		{
			this.hasCartridge = true;
			this.cartridge = newCartridge;
		}
	}

	private bool isTriggerDown = false;

	private bool isEjectorRodDown = false;

	private Chamber[] chambers = new Chamber[6] {new Chamber(), new Chamber(), new Chamber(), new Chamber(), new Chamber(), new Chamber()};
	
	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private Transform bulletOrigin;

	[SerializeField]
	private float muzzleVelocity = 237.0f;

	private float cylinderRotation;
	public float CylinderRotation
	{
		get => cylinderRotation;
		set
		{
			cylinderRotation = Mathf.Repeat(value, 360);
		}
	}

	public int FiringChamberIndex => Mathf.FloorToInt(CylinderRotation / 60);
	public int LoadingChamberIndex => MathHelper.Mod(FiringChamberIndex + 1, 6);
	public Chamber FiringChamber => chambers[FiringChamberIndex];
	public Chamber LoadingChamber => chambers[LoadingChamberIndex];
	public bool IsTriggerDown => isTriggerDown;

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

	public bool GetChamber(int index, out Chamber chamber)
	{
		chamber = new Chamber();

		if (index < 0 || index >= chambers.Length)
		{
			return false;
		}

		chamber = chambers[index];
		return true;
	}

	public void Trigger()
	{
		// Play trigger pull sound

		isTriggerDown = true;

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

	public void ReleaseTrigger()
	{
		isTriggerDown = false;
	}

	public void PullEjectorRod()
	{
		if (!isEjectorRodDown)
		{
			OnEjectorRodDown();
		}
		isEjectorRodDown = true;
	}

	public void PushEjectorRod()
	{
		isEjectorRodDown = false;
	}

	private void OnEjectorRodDown()
	{
		if (!LoadingChamber.HasCartridge)
		{
			return;
		}
		
		EjectCartridge();
	}

	private void EjectCartridge()
	{
		LoadingChamber.Empty();
	}

	public void TryLoadCartridge(Cartridge cartridge)
	{
		if (LoadingChamber.HasCartridge)
		{
			return;
		}

		LoadCartridge(cartridge);
	}

	private void LoadCartridge(Cartridge cartridge)
	{
		LoadingChamber.Insert(cartridge);
	}

	public void PullHammer(float delta)
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

	public void ReleaseHammer()
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
		GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin);
		newBullet.GetComponent<Rigidbody>().AddForce(bulletOrigin.forward * muzzleVelocity, ForceMode.VelocityChange);
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
