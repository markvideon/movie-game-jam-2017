using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TranslateWorldContext : MonoBehaviour {

	public float xRadius = 100f;
	public float zRadius = 100f;

	private GameObject playerReference;
	private GameObject[] enemyReference;

	private Vector3 initialPlayerPos;


	// Use this for initialization
	void Start () {
		playerReference = GameObject.FindGameObjectWithTag("Player");
		enemyReference = GameObject.FindGameObjectsWithTag("Enemy");

		initialPlayerPos = Vector3.zero;

	}

	// Update is called once per frame
	void Update () {
		float xMeasure = Mathf.Abs (playerReference.transform.position.x);
		float zMeasure = Mathf.Abs (playerReference.transform.position.z);

		if (xMeasure > xRadius || zMeasure > zRadius) {
			Vector3 difference = initialPlayerPos - playerReference.transform.position;

			//Translate player
			playerReference.transform.Translate(difference,Space.World);

			// Translate all enemies
			foreach(GameObject e in enemyReference) {
				e.transform.Translate(difference,Space.World);
			}


		}
	}
}
