using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel; // For use of ReadOnlyCollection
using UnityEngine;
using UnityEngine.UI;

/*
 * Draws a radar system onto a new texture which is set to the 
 * texture used by the Raw Image Component. 
 * 
 * Radar system functionality is currently coupled with
 * SwarmList (and hence SwarmMovement) in order to draw enemies.
 */

[RequireComponent(typeof(RawImage))]
public class DrawRadar : MonoBehaviour {

	[SerializeField]
	private int scanRadius = 10; 

	//Component level data
	private RawImage RawComponent;
	private Texture2D radarTexture;
	private GameObject playerObjectReference;

	// Entity nav colours
	private Color[] backgroundColour;
	private Color[] enemyColour;
	private Color[] playerColour;

	//Co-ordinate system information
	private int origin;
	private int blockHeight;
	private int blockWidth;

	void Start () {
		RawComponent = GetComponent<RawImage> ();
		radarTexture = new Texture2D (256, 256);

		// Component level settings
		Color ComponentColour = new Color(1f,1f,1f,1f); 
		RawComponent.color = ComponentColour;
		RawComponent.texture = radarTexture;

		// Co-ordinate system
		origin = radarTexture.width / 2;
		blockWidth = (int)radarTexture.width/10;
		blockHeight = blockWidth;

		playerObjectReference = GameObject.FindGameObjectWithTag("Player");

		SetUpColourArrays ();
		DrawPlayerAtOrigin ();
	}

	// Update is called once per frame
	void Update () {
		ClearTexture ();
		DrawPlayerAtOrigin ();
		DrawEnemies ();
	}

	private void SetUpColourArrays() {
		
		// Set-up background colour array
		backgroundColour = new Color[radarTexture.width * radarTexture.height];

		for (int i = 0; i < backgroundColour.Length; i++) {
			backgroundColour [i] = new Color(1f,1f,1f,0.5f);
		}

		// Enemy colour block
		enemyColour = new Color[blockWidth * blockHeight];

		for (int i = 0; i < enemyColour.Length; i++) {
			enemyColour [i] = Color.red;
		}

		//Player colour block
		playerColour = new Color[blockWidth * blockHeight];

		for (int i = 0; i < playerColour.Length; i++) {
			playerColour [i] = Color.white;
		}

	}

	// Sets every pixel in the texture to backgroundColour
	private void ClearTexture() {

		radarTexture.SetPixels (0, 0, radarTexture.width,radarTexture.height, backgroundColour);
		radarTexture.Apply ();
	}

	// Offsets are required as blocks are drawn to the right and 
	// up from the starting position.
	private void DrawPlayerAtOrigin() {

		radarTexture.SetPixels (origin - blockWidth/2, origin - blockHeight/2, blockWidth, blockHeight, playerColour);
		radarTexture.Apply ();
	}

	private void DrawEnemies() {

		// Read-only version of list of swarming enemies
		ReadOnlyCollection<Transform> enemyList = SwarmList.getList();

		// Co-ordinate system with player at origin 
		foreach (Transform enemyTransform in enemyList) {

			float deltaX = enemyTransform.position.x- playerObjectReference.transform.position.x;
			float deltaZ = enemyTransform.position.z - playerObjectReference.transform.position.z;

			// Assuming x-z plane movement
			if (deltaX < scanRadius && deltaZ < scanRadius) {

				int xLoc = Mathf.FloorToInt ((origin - blockWidth / 2) + (deltaX / scanRadius) * (radarTexture.width / 2));
				int zLoc = Mathf.FloorToInt ((origin - blockHeight/2) + (deltaZ/scanRadius) * (radarTexture.height/2));

				if ((xLoc > 0 && xLoc < radarTexture.width-blockWidth) && 
					(zLoc > 0 && zLoc < radarTexture.height-blockHeight)) {
					radarTexture.SetPixels (xLoc, zLoc, blockWidth, blockHeight, enemyColour);
				}
			}
		}

		radarTexture.Apply ();
	}

	private void DrawObstacle() {
	}
}
