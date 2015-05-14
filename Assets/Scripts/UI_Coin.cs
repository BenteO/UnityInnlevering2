using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Coin : MonoBehaviour {

	public GameObject UIText;
	Text thisCoins;

	// Use this for initialization
	void Start () {
		thisCoins = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		thisCoins.text = GameController.gameController.coins.ToString("D2");
	}
}
