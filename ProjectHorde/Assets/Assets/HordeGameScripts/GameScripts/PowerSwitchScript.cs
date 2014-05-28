using UnityEngine;
using System.Collections;

public class PowerSwitchScript : MonoBehaviour {

	GameManagerScript gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManagerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider c)
	{
		if( c.tag == "Player" && !gameManager.bPowerOn)
		{
			gameManager.EndDisplay();
			gameManager.DisplayInstruction( "Activate to Turn on the Power" , true , (GameManagerScript.BuyDelegate)(TurnOnPower) );
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if(c.tag == "Player" )
		{
			gameManager.EndDisplay();
		}
	}
	
	public void TurnOnPower( PlayerInventoryScript a_inventory )
	{
		gameManager.TurnOnPower();
		gameManager.EndDisplay ();
	}

}