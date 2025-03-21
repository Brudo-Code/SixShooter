using Unity.FPS.Gameplay;
using UnityEngine;

public class SixShooterGunArm : MonoBehaviour
{
	private const float DEFAULT_LOOK_DISTANCE = 100f;

	public bool isPlayer = true;

	public enum Grip {Hip, Reload};
	private Grip previousGrip;
	private Grip currentGrip;

	[SerializeField]
	private SixShooterInputHandler inputHandler;
	[SerializeField]
	private Transform lookOrigin;
	[SerializeField]
	private Transform gunTransform;
	[SerializeField]
	private Revolver revolver;

	[SerializeField]
	private LayerMask lookLayerMask;

	[Header("Gun Handling")]
	[SerializeField]
	private float hammerPullDuration = 0.25f;
	[SerializeField]
	private float cylinderRotateSpeed = 30.0f;

	[Header("Gun position")]
	[SerializeField]
	private Transform hipPosition;
	[SerializeField]
	private Transform reloadPosition;
	[SerializeField]
	private float gripChangeDuration = 0.25f;
	[SerializeField]
	private AnimationCurve hipToReloadGripCurve;
	[SerializeField]
	private AnimationCurve reloadToDefaultGripCurve;

	private Vector3 lookPoint;
	private SwitchTimer gripChangeTimer = new SwitchTimer(0.0f, BaseTimer.DeltaTimeMode.Scaled);
	private Vector3 switchStartPosition;
	private Quaternion switchStartRotation;
	private AnimationCurve currentGripChangeCurve;

	[SerializeField]
	private bool drawRevolverStateOnGui = true;

	private Vector3 DefaultLookPoint => lookOrigin.forward * DEFAULT_LOOK_DISTANCE;
	private bool CanManipulateGun => gripChangeTimer.PassedTime;
	private float NormalizedGripChangeTime =>  currentGripChangeCurve.Evaluate(gripChangeTimer.TimeElapsed / gripChangeTimer.TargetTime);
	private Transform GripChangeTarget
	{
		get
		{
			switch (currentGrip)
			{
				case Grip.Hip:
					return hipPosition;

				case Grip.Reload:
					return reloadPosition;
			}

			return hipPosition;
		}
	}

	private void Update()
    {
		FindLookPoint();
		MoveIntoGrip();

		if (currentGrip == Grip.Hip && CanManipulateGun)
		{
			gunTransform.LookAt(lookPoint);
		}

		if (isPlayer)
		{
			HandleArmInput();
			HandleGunInput();
		}
    }

	private void MoveIntoGrip()
	{
		if (CanManipulateGun)
		{
			return;
		}

		gripChangeTimer.Tick();
		gunTransform.position = Vector3.LerpUnclamped(switchStartPosition, GripChangeTarget.position, NormalizedGripChangeTime);
		gunTransform.rotation = Quaternion.LerpUnclamped(switchStartRotation, GripChangeTarget.rotation, NormalizedGripChangeTime);
	}

	private void HandleArmInput()
	{
		if (inputHandler.GetReloadButtonDown())
		{
			if (currentGrip == Grip.Hip)
			{
				ChangeGrip(Grip.Reload);
			}
			else if(currentGrip == Grip.Reload)
			{
				ChangeGrip(Grip.Hip);
			}
		}
	}

	private void HandleGunInput()
	{
		if (!CanManipulateGun)
		{
			return;
		}

		if (inputHandler.GetFireInputDown())
		{
			if (currentGrip == Grip.Hip)
			{
				PullTrigger();
			}
			else if (currentGrip == Grip.Reload)
			{
				revolver.TryLoadCartridge(new Cartridge());
			}
		}

		if (inputHandler.GetFireInputReleased())
		{
			revolver.ReleaseTrigger();
		}

		if (inputHandler.GetHammerPullInputHeld())
		{
			PullHammer();
		}
		else if (inputHandler.GetHammerReleased())
		{
			ReleaseHammer();
		}

		revolver.TryRotateCylinder(inputHandler.GetCylinderInput() * cylinderRotateSpeed);
	}

	private void ChangeGrip(Grip newGrip)
	{
		float cancelTime = newGrip == previousGrip ? gripChangeTimer.TimeLeft : 0;
		gripChangeTimer.TargetTime = gripChangeDuration - cancelTime;
		switchStartPosition = gunTransform.position;
		switchStartRotation = gunTransform.rotation;

		switch (newGrip)
		{
			case Grip.Hip:
				currentGripChangeCurve = reloadToDefaultGripCurve;
				break;

			case Grip.Reload:
				currentGripChangeCurve = hipToReloadGripCurve;
				break;

			default:
				break;
		}

		previousGrip = currentGrip;
		currentGrip = newGrip;
		gripChangeTimer.Reset();
	}

	public void PullHammer()
	{
		revolver.PullHammer(1 / hammerPullDuration * Time.deltaTime);
	}

	public void ReleaseHammer()
	{
		revolver.ReleaseHammer();
	}

	public void PullTrigger()
	{
		revolver.Trigger();
	}

	public void FindLookPoint()
	{
		if (Physics.Raycast(lookOrigin.position, lookOrigin.forward, out RaycastHit hit, Mathf.Infinity, lookLayerMask))
		{
			lookPoint = hit.point;
			return;
		}

		lookPoint = DefaultLookPoint;
	}

	private void OnGUI()
	{
		if (!drawRevolverStateOnGui)
		{
			return;
		}

		string revolverState = $"Revolver\n" +
			$"Hammer: {revolver.CurrentHammerState}\n" +
			$"Cylinder: {revolver.CylinderRotation}\n" +
			$"Chambers: ";
		for (int i = 0; i < 6; i++)
		{
			if (i > 0)
			{
				revolverState += "-";
			}
			revolverState += $"{(revolver.FiringChamberIndex == i ? "F" : "")}";
			revolverState += $"{(revolver.LoadingChamberIndex == i ? "L" : "")}";

			if (!revolver.GetChamber(i, out Revolver.Chamber chamber))
			{
				revolverState += "ERROR";
				continue;
			}
			if (!chamber.HasCartridge)
			{
				revolverState += "0";
				continue;
			}
			revolverState += $"{(chamber.Cartridge.IsSpent ? "X" : "I")}";
		}
		GUI.Label(new Rect(10, 10, 200, 100), revolverState);
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		float crossScale = 0.25f;
		Gizmos.DrawLine(lookPoint - lookOrigin.right * crossScale, lookPoint + lookOrigin.right * crossScale);
		Gizmos.DrawLine(lookPoint - lookOrigin.up * crossScale, lookPoint + lookOrigin.up * crossScale);
	}
}
