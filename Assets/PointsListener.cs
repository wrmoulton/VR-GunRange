using UnityEngine;
using BNG;

public class PointsListener : MonoBehaviour {
    public MoveableTargets target; // May be unneeded but needs further testing
    public int pointsValue = 10; 

    // Add to on damaged of Damageable
    public void AddPoints(float damage) {
        // Debug.Log($"Adding points: {pointsValue}");
        // Adds pointsValue, not damage, to target
		if(target == null) {
			Debug.LogWarning("No target assigned in PointsListener!!");
			return;
		}
       	target.HitTarget(pointsValue);
    }
}
