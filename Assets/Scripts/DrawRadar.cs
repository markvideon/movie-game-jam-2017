using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Draws a radar system onto a new texture which is set to the 
 * texture used by the Raw Image Component. 
 * 
 */

[RequireComponent(typeof(RawImage))]
public class DrawRadar : MonoBehaviour {


    [SerializeField] private Transform enemyTransformParent;

    // Entity nav colours
    [SerializeField] Color backgroundColour = new Vector4(1f,1f,1f,0.4f);
    [SerializeField] Color playerColour = new Vector4(1f, 1f, 1f, 1f);
    [SerializeField] Color enemyColour = new Vector4(1f, 0f, 0f, 1f);

    //Component level data
    private RawImage RawComponent;
    private Texture2D radarTexture;
    [SerializeField] private GameObject playerObjectReference;

    private Color[] backgroundColoursArray;
	private Color[] enemyColoursArray;
	private Color[] playerColoursArray;

	//Co-ordinate system information
	private int origin;
	private int blockHeight;
	private int blockWidth;

    // Image size
    [SerializeField] private int texWidth = 256;
    [SerializeField] private int texHeight = 256;
    [SerializeField] private int scanRadius = 30;
    [SerializeField] private int iconSize = 10;

    void Start () {
		RawComponent = GetComponent<RawImage> ();
        radarTexture = new Texture2D (texWidth, texHeight);

		// Component level settings
		Color ComponentColour = new Color(1f,1f,1f,1f); 
		RawComponent.color = ComponentColour;
		RawComponent.texture = radarTexture;

		// Co-ordinate system
        origin = texWidth / 2;
        blockWidth = iconSize;
		blockHeight = blockWidth;

		SetUpPrimitiveColourArrays ();
		DrawPlayerAtOrigin ();
	}

	// Update is called once per frame
	void Update () {
        ClearTexture ();
        DrawPlayerAtOrigin ();

        // Draw enemies
        DrawMarker (enemyTransformParent, enemyColoursArray, blockWidth, blockHeight);
	}

    #region Colours

    private void SetUpPrimitiveColourArrays() {
		
		// Set-up background colour array
		backgroundColoursArray = new Color[radarTexture.width * radarTexture.height];

		for (int i = 0; i < backgroundColoursArray.Length; i++) {
            backgroundColoursArray [i] = backgroundColour;
		}

		// Enemy colour block
		enemyColoursArray = new Color[blockWidth * blockHeight];

		for (int i = 0; i < enemyColoursArray.Length; i++) {
            enemyColoursArray [i] = enemyColour;
		}

		//Player colour block
		playerColoursArray = new Color[blockWidth * blockHeight];

		for (int i = 0; i < playerColoursArray.Length; i++) {
            playerColoursArray [i] = playerColour;
		}

	}

    // For possible future use
    private void SetUpPrimitiveColourArrays(Color[] extBackgroundColourArray, Color[] extEnemyColourArray, Color[] extPlayerColourArray)
    {

        // Set-up background colour array
        for (int i = 0; i < backgroundColoursArray.Length; i++)
        {
            backgroundColoursArray[i] = extBackgroundColourArray[i];
        }

        for (int i = 0; i < enemyColoursArray.Length; i++)
        {
            enemyColoursArray[i] = extEnemyColourArray[i];
        }

        for (int i = 0; i < playerColoursArray.Length; i++)
        {
            playerColoursArray[i] = extPlayerColourArray[i];
        }

    }
    #endregion

    // Sets every pixel in the texture to backgroundColour
    private void ClearTexture() {

        radarTexture.SetPixels (0, 0, radarTexture.width,radarTexture.height, backgroundColoursArray);
        radarTexture.Apply ();
	}

	// Offsets are required as blocks are drawn to the right and 
	// up from the starting position.
	private void DrawPlayerAtOrigin() {

        radarTexture.SetPixels (origin - blockWidth/2, origin - blockHeight/2, blockWidth, blockHeight, playerColoursArray);
        radarTexture.Apply ();
	}

    /* Will draw the child transforms of markerParent using markerColours array */
	private void DrawMarker(Transform markerParent, Color[] markerColour, int markerWidth, int markerHeight) {
    

        // Co-ordinate system with player at origin 
        for (int i = 0; i < markerParent.childCount; i++) {

            float deltaX = markerParent.GetChild(i).position.x- playerObjectReference.transform.position.x;
			float deltaZ = markerParent.GetChild(i).position.z - playerObjectReference.transform.position.z;

			// Assuming x-z plane movement
			if (deltaX < scanRadius && deltaZ < scanRadius) {

				int xLoc = Mathf.FloorToInt ((origin - blockWidth / 2) + (deltaX / scanRadius) * (radarTexture.width / 2));
				int zLoc = Mathf.FloorToInt ((origin - blockHeight/ 2) + (deltaZ/scanRadius) * (radarTexture.height/2));

				if ((xLoc > 0 && xLoc < radarTexture.width-blockWidth) && 
					(zLoc > 0 && zLoc < radarTexture.height-blockHeight)) {
                    radarTexture.SetPixels (xLoc, zLoc, markerWidth, markerHeight, markerColour);
				}
			}
		}

		radarTexture.Apply ();
	}


}
