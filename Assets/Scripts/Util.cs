using UnityEngine;
using System.Collections;

public class Util {
	public static Vector3 ForceTowards(Vector3 origin, Vector3 target, float intensity) {
        return (target - origin).normalized * intensity;
    }

    // checks if a is in front of b, in the forward direction
    public static bool IsInFront(Vector3 a, Vector3 b, Vector3 forward) {
        Vector3 heading = b - a;
        heading.Normalize();
        float dot = Vector3.Dot(heading, forward);
        return dot < 0;
    }
}
