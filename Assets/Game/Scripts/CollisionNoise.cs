using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollisionNoise : MonoBehaviour {

	[SerializeField] private List<string> collideWithTags;
	AudioSource thisAudio;
	// Use this for initialization
	void Start () {
		thisAudio = GetComponent<AudioSource> ();
	}


	void OnCollisionEnter(Collision collision) {
		if (collideWithTags.Contains(collision.collider.tag)) {
			thisAudio.Play ();
		}
	}
}
