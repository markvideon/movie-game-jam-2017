using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpolateCoroutine : MonoBehaviour {

	MeshRenderer renderer;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<MeshRenderer> ();
		StartCoroutine("Fade");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Fade(float ivalue, float fvalue, float runtime) {

		float elapsed = 0;


		while (elapsed / runtime <= 0.99*fvalue )  {
			Color c = renderer.material.color;
			elapsed += Time.deltaTime;
			renderer.material.color = c;
			Debug.Log (f);
			yield return null;
		}

	}
}
