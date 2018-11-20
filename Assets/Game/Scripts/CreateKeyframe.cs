using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateKeyframe : MonoBehaviour {

	[SerializeField] private AnimationCurve anim;
	private Keyframe[] ks;
	void Start() {
		ks = new Keyframe[50];
		int i = 0;
		while (i < ks.Length) {
			ks[i] = new Keyframe(i, Mathf.Sin(i));
			i++;
		}
		anim = new AnimationCurve(ks);
	}
	void Update() {
		transform.position = new Vector3(Time.time, anim.Evaluate(Time.time), 0);
	}

}




