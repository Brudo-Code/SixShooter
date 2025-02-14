using System.Collections;
using UnityEngine;

public class MechanicalAction
{
	public virtual void OnStart() { }
	public virtual IEnumerator Step() { yield return null; }
	public virtual void OnStopped() { }
}
