using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

/* 
 * Flocking contains functions for the components of the
 * common model for flocking behaviours: Seperation, Cohesion and
 * Alignment, Seeking and Avoidance
 * 
 * Note: Alignment component currently requires use of RigidBody velocities
 */

public static class Flocking {

   	public static Vector3 getCohesion(Transform queryingTransform, float neighbourhoodRadius, bool normalise) {
		
		Vector3 result = new Vector3 (0f, 0f, 0f);
        int neighbourCount = queryingTransform.parent.childCount - 1;

        if (neighbourCount < 1) {
            return result;
        }

        for (int i = 0; i < neighbourCount + 1; i++) {
            if (Vector3.Distance (queryingTransform.position, queryingTransform.parent.GetChild(i).position) < neighbourhoodRadius) {
				result += queryingTransform.parent.GetChild(i).position;
			}
		}

		// Result is average position of neighbours
        result /= neighbourCount;

	    // Result is now vector from current position to that average position
        result -= queryingTransform.position;

        if (normalise) {
            result = Vector3.Normalize(result);
        }
		
		return result;
	}

	public static Vector3 getAlignment(Transform queryingTransform, float neighbourhoodRadius, bool normalise, Vector3[] flockVelocities) {
		
        Vector3 result = new Vector3 (0f, 0f, 0f);

        int neighbourCount = queryingTransform.parent.childCount -1;

        if (flockVelocities.Length != (neighbourCount + 1)) {

            #if UNITY_EDITOR
                Debug.Log("Velocity list length did not flock member count!");
            #endif

            return result;
        }

        // Proceed through all the children, no guarantee of sorted order
        for (int i = 0; i < neighbourCount + 1; i++)
        {

            float radius = Vector3.Distance (queryingTransform.position, queryingTransform.parent.GetChild(i).position);

			if (radius < neighbourhoodRadius && queryingTransform != queryingTransform.parent.GetChild(i)) 
            {
                result += flockVelocities[i];
            } 
			
		}

		result /= neighbourCount;

        if (normalise) {
            result = Vector3.Normalize(result);
        }

		return result;
	}

	public static Vector3 getSeperation(Transform queryingTransform, float neighbourhoodRadius, bool normalise) {
		Vector3 result = new Vector3 (0f, 0f, 0f);
		
        int neighbourCount = queryingTransform.parent.childCount -1;

        if (neighbourCount < 1) {
            return result;
        }
        
        for (int i = 0; i < neighbourCount + 1; i++) {

            float distance = Vector3.Distance (queryingTransform.position, queryingTransform.parent.GetChild(i).position);

            // No need to check if the transform being evaluated is the current
            // Transform as for Seperation Force computation would result as 0f.
            if (distance < neighbourhoodRadius && distance > 0f) {

				// Collecting the vector *away* from members[i] to seperate.
				result += (queryingTransform.position - queryingTransform.parent.GetChild(i).position);
			}
		}

		result /= neighbourCount;

        if (normalise)
        {
            result = Vector3.Normalize (result);
		}

		return result;
	}

    public static Vector3 getSeeking(Transform queryingTransform, Transform goal, bool normalise) {
        Vector3 result = new Vector3(0f, 0f, 0f);
        result = goal.position - queryingTransform.position;

        if (normalise) {
            result = Vector3.Normalize(result);
        }

        return result;
    }
	
    public static Vector3 getAvoidance(Transform queryingTransform, Transform goal, bool normalise) {
        return -1f * getSeeking(queryingTransform,goal, normalise);
    }
}
