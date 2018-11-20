using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

/* 
 * SwarmList contains functions for the components of the
 * common model for flocking behaviours: Seperation, Cohesion and
 * Alignment, as well as a data structure containing members of the flock
 * 
 * SwarmList is coupled with SwarmMovement to produce the AI behaviour 
 */

public class SwarmList {

	private static List<Transform> members;

	private static int length = 0;

	static SwarmList() {
		members = new List<Transform>();
	}

	public static ReadOnlyCollection<Transform> getList() {
		return members.AsReadOnly();
	}

	public static void addMember(Transform input) {
		members.Add (input);
		length++;
	}

	public static Vector3 getCohesion(Vector3 inputPosition, float neighbourhoodRadius) {
		
		Vector3 result = new Vector3 (0f, 0f, 0f);
		int neighbours = 0;

		foreach (Transform t in members) {
			if (Vector3.Distance (inputPosition, t.position) < neighbourhoodRadius) {
				result += t.position;
				neighbours++;
			}
		}

		if (neighbours > 0) {
			// Result is average position of neighbours
			result /= neighbours;

			// Result is now vector from current position to that average position
			result -= inputPosition;

			result = Vector3.Normalize (result);
		}
			
		return result;
	}

	public static Vector3 getAlignment(Vector3 inputPosition, float neighbourhoodRadius) {
		Vector3 result = new Vector3 (0f, 0f, 0f);

		int neighbours = 0;

		foreach (Transform t in members) {

			float radius = Vector3.Distance (inputPosition, t.position);

			/* Represents the number of members of the list where 
			 *  inputPosition == t.position
			 */
			int duplicateCount = 0;

			if (radius < neighbourhoodRadius) {
				Rigidbody rb = t.GetComponent<Rigidbody> ();

				if (rb) {
					
					if (radius > 0) {
						result += rb.velocity;
						neighbours++;
					} 
					else {
						duplicateCount++;
						if (duplicateCount > 1 ) {
							result += rb.velocity;
							neighbours++;
						}
					}
				
				} else {
					Debug.Log ("Expected rigidbody when calculating alignment component!");
				}
			}
		}

		if (neighbours > 0) {
			result /= neighbours;
			result = Vector3.Normalize (result);
		}

		return result;
	}

	public static Vector3 getSeperation(Vector3 inputPosition, float neighbourhoodRadius) {
		Vector3 result = new Vector3 (0f, 0f, 0f);
		int neighbours = 0;

		foreach (Transform t in members) {

			float distance = Vector3.Distance (inputPosition, t.position);

			if (distance < neighbourhoodRadius && distance > 0f) {

				// Note the sign convention. Collecting the vector *away* from members[i]
				// To seperate.
				result += (inputPosition - t.position);
				neighbours++;
			}
		}

		if (neighbours > 0) {
			result /= neighbours;
			result = Vector3.Normalize (result);
		}

		return result;
	}
		
	public static int getLength() {
		return length;
	}

}
