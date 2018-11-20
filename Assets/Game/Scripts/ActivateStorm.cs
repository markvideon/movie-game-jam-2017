using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ActivateStorm : MonoBehaviour {

	private MeshRenderer myMesh;
	private Rigidbody myRb; 
	private Vector3 someplace_far_away;

	[SerializeField] public static bool noStorm;
	[SerializeField] private Light clearLight;
	[SerializeField] private Light stormLight;
	[SerializeField] private GameObject lightDust;
	[SerializeField] private GameObject darkDust;
	[SerializeField] private GameObject lightning;
	[SerializeField] private GameObject tornado;




	void Start () {
		someplace_far_away = new Vector3 (0f,0f,2500f);
		this.transform.position = someplace_far_away;

		noStorm = true;
		myRb = GetComponent<Rigidbody> ();
		myRb.velocity = new Vector3 (0f,0f,-100f);
	}


	void ResetStorm() {
		this.transform.position = someplace_far_away;
		myRb.velocity = Vector3.zero;
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Player")) {
			ToggleStorm ();
			UpdateSkybox ();
			UpdateDirectionalLights ();
			UpdateLightning ();
			UpdateDust ();
			UpdateTornado ();
			this.transform.position = someplace_far_away;

		}
	}

	void UpdateTornado() {
		if (noStorm) {
			tornado.gameObject.SetActive (false); 
		} else {
			tornado.gameObject.SetActive (true); 
		}
	}

	void UpdateSkybox() {
		if (noStorm) {
			Camera.main.clearFlags = CameraClearFlags.Skybox; 
		} else {
			Camera.main.clearFlags = CameraClearFlags.SolidColor;
		}
	}

	void UpdateDirectionalLights() {
		if (noStorm) {
			stormLight.gameObject.SetActive (false);
			clearLight.gameObject.SetActive (true);
		} else {
			clearLight.gameObject.SetActive (false);
			stormLight.gameObject.SetActive (true);
		}

	}

	void UpdateLightning() {
		if (noStorm) {
			lightning.gameObject.SetActive (false);

		} else {
			lightning.gameObject.SetActive (true);
		}
	}

	void UpdateDust() {
		if (noStorm) {
			lightDust.gameObject.SetActive (true);
			darkDust.gameObject.SetActive (false);

		} else {
			lightDust.gameObject.SetActive (false);
			darkDust.gameObject.SetActive (true);		
		}

	}


	void ToggleStorm() {
		if (noStorm) {
			noStorm = false;
		} else {
			noStorm = true;
		}
	}
}
