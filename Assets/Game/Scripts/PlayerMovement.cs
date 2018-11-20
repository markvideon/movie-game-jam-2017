using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody rb; 

	[SerializeField] public float accelMultiplier;
	[SerializeField] public float accelRate;
	[SerializeField] public float decelRate;

	[SerializeField] private float angleLowerBound;
	[SerializeField] private float angleUpperBound;
	[SerializeField] private float angularSpeed;

	[SerializeField] private float speedLowerLimit;
	[SerializeField] private float speed;
	[SerializeField] private float speedUpperLimit;
	private float theta = 90f;

	private Vector3 initialPos;
	private Quaternion initialRot;
	private Vector3 moveDir;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();

		moveDir = new Vector3 (0f, 0f, 1f);
		rb.velocity = speed*moveDir;


		initialPos = this.transform.position;
		initialRot = this.transform.rotation;
	}

	// Update is called once per frame
	void Update () {

		if (ResetCheck()) 
		{
			ResetPosition ();
		} else {
			Accelerate();
			Steer ();
		}

			
	}

	private void Accelerate() {
		speed = Vector3.Magnitude (rb.velocity);

		if ((Input.GetKey (KeyCode.W) && speed < speedUpperLimit) || speed < speedLowerLimit) {
			rb.AddForce(accelMultiplier* accelRate*moveDir*Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S) && speed > speedLowerLimit) {
			rb.AddForce(-accelMultiplier*decelRate*moveDir*Time.deltaTime);
		}

	}
		
	private void Steer() {

		if (Input.GetKey (KeyCode.A) && theta < angleUpperBound) {
			theta += angularSpeed*Time.deltaTime;
			moveDir.x = Mathf.Cos(Mathf.Deg2Rad * theta);
			moveDir.z = Mathf.Sin (Mathf.Deg2Rad * theta);
			rb.velocity = speed* moveDir;
		}

		if (Input.GetKey (KeyCode.D) && theta > angleLowerBound) {
			theta -= angularSpeed * Time.deltaTime;
			moveDir.x = Mathf.Cos(Mathf.Deg2Rad * theta);
			moveDir.z = Mathf.Sin (Mathf.Deg2Rad * theta);
			rb.velocity = speed* moveDir;
		}

			
	}

	// Reset iff object has fallen through the floor (world y-axis)
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
