using UnityEngine;

public static class MathHelper
{
	public static int Mod(int a, int n) => (a % n + n) % n;
}
