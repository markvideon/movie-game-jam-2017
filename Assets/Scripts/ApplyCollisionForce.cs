using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCollisionForce : MonoBehaviour {

	// Whitelist of objects to apply any additional force with
	[SerializeField] private List<string> collideWithTags;

	// Force to apply for collisions with objects of same type
	[SerializeField] private int collisionSameTypeForce; 

	// Force to apply for collisions with objects of different type
	[SerializeField] private int collisionDiffTypeForce;


	private Vector3 appliedDirection = new Vector3();

	public void OnCollisionEnter(Collision collision) {
		
		appliedDirection = collision.collider.transform.position - this.transform.position;


		if (collideWithTags.Contains(collision.collider.tag)) {
			if ( collision.collider.tag == this.gameObject.tag) {
				collision.collider.attachedRigidbody.AddForce (collisionSameTypeForce * appliedDirection);
			} 	else {
				collision.collider.attachedRigidbody.AddForce (collisionDiffTypeForce * appliedDirection);

			}
		}
	}
}
