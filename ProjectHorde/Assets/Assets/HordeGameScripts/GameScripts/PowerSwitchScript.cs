using UnityEngine;
using System.Collections;

public class PowerSwitchScript : MonoBehaviour {

	GameMangerScript gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameMangerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider c)
	{
		if( c.tag == "Player" && !gameManager.bPowerOn)
		{
			gameManager.EndDisplay();
			gameManager.DisplayInstruction( "Activate to Turn on the Power" , true , (GameMangerScript.BuyDelegate)(TurnOnPower) );
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