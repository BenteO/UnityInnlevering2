using UnityEngine;
using System.Collections;

public class breakBrick: MonoBehaviour {

	BoxCollider2D boxCollider2D;
	public BoxCollider2D pieceBounds;
	public Animator anim;
	public GameObject piece;
	PieceOrigins pieceOrigins;
	GameObject pieceTL;
	GameObject pieceTR;
	GameObject pieceBL;
	GameObject pieceBR;

	void Start() {
		boxCollider2D = GetComponentInParent<BoxCollider2D>();
	}

	void Update() {
		UpdatePieceOrigins();
		if(GameObject.FindWithTag("piece")) {
			pieceTL.transform.position = pieceOrigins.topLeft;
			pieceTR.transform.position = pieceOrigins.topRight;
			pieceBL.transform.position = pieceOrigins.bottomLeft;
			pieceBR.transform.position = pieceOrigins.bottomRight;
		}
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
