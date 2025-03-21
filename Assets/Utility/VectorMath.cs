using UnityEngine;

namespace PunchButton.Utility
{
	public static class VectorMath
	{
		public static Vector2 Round(this Vector2 v)
		{
			return new Vector2
			(
				Mathf.Round(v.x),
				Mathf.Round(v.y)
			);
		}

		public static Vector2 GetDirectionFromAngle(float degrees)
		{
			float radians = degrees * Mathf.Deg2Rad;
			return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
		}

		public static float GetAngleFromDirection(Vector2 direction)
		{
			direction.Normalize();
			float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
			if (angle < 0)
			{
				angle += 360;
			}
			angle %= 360;
			return angle;
		}

		public static Vector2 GetRandomDirection()
		{
			return GetDirectionFromAngle(Random.Range(0, 360));
		}
	}
}
