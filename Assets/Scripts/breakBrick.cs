using UnityEngine;
using System.Collections;

public class breakBrick: MonoBehaviour {

	// Components
	BoxCollider2D boxCollider2D;
	public BoxCollider2D pieceBounds;
	public Animator anim;

	// Piece after break
	public GameObject piece;

	// Where to spawn pieces
	PieceOrigins pieceOrigins;
	// Spawn piece positioning
	GameObject pieceTL;
	GameObject pieceTR;
	GameObject pieceBL;
	GameObject pieceBR;

	// Audio Clips
	public AudioClip shatter;

	// Gets the boxCollider2D
	void Start() {
		boxCollider2D = GetComponent<BoxCollider2D>();
	}

	void Update() {
		// Updates the position of the broken pieces
		UpdatePieceOrigins();
		// Changes positions of the broken pieces
		if(GameObject.FindGameObjectWithTag("piece")) {
			if(pieceTL != null) {
				pieceTL.transform.position = pieceOrigins.topLeft;
			}
			if(pieceTR != null) {
				pieceTR.transform.position = pieceOrigins.topRight;
			}
			if(pieceBL != null) {
				pieceBL.transform.position = pieceOrigins.bottomLeft;
			}
			if(pieceBR != null) {
				pieceBR.transform.position = pieceOrigins.bottomRight;
			}
		}
	}

	// Increase score; for animator
	public void increaseScoreBricks() {
		AudioManager.audioManager.PlayFX(shatter);
		GainPoints.gainPoints.increaseScoreFixed(50);
	}

	public void disableCollider() {
		boxCollider2D.enabled = !boxCollider2D.enabled;
		anim.enabled = !anim.enabled;
		pieceTL = (GameObject) Instantiate(piece, new Vector3(pieceOrigins.topLeft.x, pieceOrigins.topLeft.y, 0), Quaternion.identity);
		pieceTR = (GameObject) Instantiate(piece, new Vector3(pieceOrigins.topRight.x, pieceOrigins.topRight.y, 0), Quaternion.identity);
		pieceBL = (GameObject) Instantiate(piece, new Vector3(pieceOrigins.bottomLeft.x, pieceOrigins.bottomLeft.y, 0), Quaternion.identity);
		pieceBR = (GameObject) Instantiate(piece, new Vector3(pieceOrigins.bottomRight.x, pieceOrigins.bottomRight.y, 0), Quaternion.identity);
	}

	void UpdatePieceOrigins() {
		Bounds bounds = pieceBounds.bounds;

		pieceOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		pieceOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		pieceOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		pieceOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	struct PieceOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
