using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
 * Rotates a rigidbody to align their forward direction with
 * their velocity direction
 * 
 */
[RequireComponent(typeof(Rigidbody))]
public class RigidEntityRotation : MonoBehaviour {

	[SerializeField]
	private int rotSpeed;

	private Quaternion toRotation;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		toRotation = Quaternion.LookRotation(rb.velocity);
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, toRotation , rotSpeed);
	
	}
}
