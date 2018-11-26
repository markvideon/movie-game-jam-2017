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

   	public static Vector3 getCohesion(Transform queryingTransform, float neighbourhoodRadius) {
		
		Vector3 result = new Vector3 (0f, 0f, 0f);
        int neighbourCount = queryingTransform.parent.childCount - 1;

        for (int i = 0; i < neighbourCount + 1; i++) {
            if (Vector3.Distance (queryingTransform.position, queryingTransform.parent.GetChild(i).position) < neighbourhoodRadius) {
				result += queryingTransform.parent.GetChild(i).position;
			}
		}

        if (neighbourCount > 0) {
			// Result is average position of neighbours
            result /= neighbourCount;

			// Result is now vector from current position to that average position
            result -= queryingTransform.position;

			result = Vector3.Normalize (result);
		}
			
		return result;
	}

	public static Vector3 getAlignment(Transform queryingTransform, float neighbourhoodRadius) {
		Vector3 result = new Vector3 (0f, 0f, 0f);

        int neighbourCount = queryingTransform.parent.childCount -1;

        // Proceed through all the children, no guarantee of sorted order
        for (int i = 0; i < neighbourCount + 1; i++)
        {

            float radius = Vector3.Distance (queryingTransform.position, queryingTransform.parent.GetChild(i).position);

			if (radius < neighbourhoodRadius) 
            {
                if (queryingTransform != queryingTransform.parent.GetChild(i)) 
                {
                    Rigidbody rb = queryingTransform.parent.GetChild(i).GetComponent<Rigidbody>();

                    if (rb) {
                        result += rb.velocity;
                    } else {
                        Debug.Log("Expected rigidbody when calculating alignment component!");

                        // Definition when not using RBs? Require some means of acquiring velocities
                    }
                } 
			}
		}

		if (neighbourCount > 0) {
			result /= neighbourCount;
			result = Vector3.Normalize (result);
		}

		return result;
	}

	public static Vector3 getSeperation(Transform queryingTransform, float neighbourhoodRadius) {
		Vector3 result = new Vector3 (0f, 0f, 0f);
		
        int neighbourCount = queryingTransform.parent.childCount -1;
        
        for (int i = 0; i < neighbourCount + 1; i++) {

            float distance = Vector3.Distance (queryingTransform.position, queryingTransform.parent.GetChild(i).position);

            // No need to check if the transform being evaluated is the current
            // Transform as for Seperation Force computation would result as 0f.
            if (distance < neighbourhoodRadius && distance > 0f) {

				// Collecting the vector *away* from members[i] to seperate.
				result += (queryingTransform.position - queryingTransform.parent.GetChild(i).position);
			}
		}

		if (neighbourCount > 0) {
			result /= neighbourCount;
			result = Vector3.Normalize (result);
		}

		return result;
	}

    public static Vector3 getSeeking(Transform queryingTransform, Transform goal) {
        Vector3 result = new Vector3(0f, 0f, 0f);
        result = goal.position - queryingTransform.position;
        result = Vector3.Normalize(result);

        return result;
    }
	
    public static Vector3 getAvoidance(Transform queryingTransform, Transform goal) {
        return -1f * getSeeking(queryingTransform,goal);
    }
}
