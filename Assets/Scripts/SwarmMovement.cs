using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * SwarmMovement applies the various components of swarm motion to 
 * the rigidbody of each entity as well as addition a new component,
 * Seeking.
 * 
*/
[RequireComponent(typeof(Rigidbody))]
public class SwarmMovement : MonoBehaviour {
	
	public float scaleMultiplier = 1f;
	public float neighbourRadius = 1f;

	public float cohesionStrength = 1f;
	public float seperationStrength = 1f;
	public float alignmentStrength = 1f; 
	public float seekingStrength = 1f;

	public Transform goal;
	private float goalDistance;

	private Rigidbody rb; 
	private Vector3 initialPos;
	private Quaternion initialRot;

	private Vector3 cohesion = new Vector3();
	private Vector3 seperation = new Vector3();
	private Vector3 alignment = new Vector3();
	private Vector3 seeking = new Vector3();

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		initialPos = this.transform.position;
		initialRot = this.transform.rotation;

		// Warning: Use script execution order to ensure this functions as implied.
		if (goal) {
			Rigidbody rb_goal = goal.GetComponent<Rigidbody>();
			if (rb_goal) {
				rb.velocity = rb_goal.velocity;
			}
		}

	}

	void Update () {

		if (ResetCheck()) {
			ResetPosition ();
		}
		else 
		{
			cohesion = Flocking.getCohesion (this.transform, neighbourRadius);
			seperation = Flocking.getSeperation(this.transform, neighbourRadius);
			alignment = Flocking.getAlignment(this.transform, neighbourRadius); 
		
			// For future use cases where a goal does not fit the application context
			if (goal) {
				seeking += goal.position - this.transform.position;
				goalDistance = Vector3.Magnitude (seeking);
			
				seeking = Vector3.Normalize (seeking);

				if (goalDistance < 5) {
					seeking *= 10;
				} else if (goalDistance > 25) {
					seeking *= 20;
				} else if (goalDistance > 50) {
					seeking *= 60;

				}
			
			}

			Vector3 result = cohesionStrength *cohesion + seperationStrength* seperation + 
							alignmentStrength* alignment + seekingStrength*seeking;

			result *= scaleMultiplier*Time.deltaTime;
			rb.AddForce(result);
		}

	}

	private bool ResetCheck() {
		if (this.transform.position.y < 0) {
			return true;
		}
	
		return false;
	}

	private void ResetPosition() {
		this.transform.position = initialPos;
		this.transform.rotation = initialRot;
	}


}
