//Created by: Steven Hoover
//All Rights Reserved 2014
//TODO:
//add animation controls
//prevent ready owned gun from poping up

using UnityEngine;
using System.Collections;

public class RandomBoxScript : MonoBehaviour {

	enum RandomBoxStates { Closed, OpenAndPicking , OpenAndChoose , Closing };
	RandomBoxStates state;
	GameManagerScript gameManager;

	GameManagerScript.GunType gunType;

	//private basic variables
	bool bGunFound = false;

	int iTotalNumOfGunTypes;
    int iTimesLooped = 0;

	float fCurrentOpenTime;

	//public variables
	public bool bFireSale = false;

	public int iCost;
	public int iFireSaleCost;

	public float fPickingTime;
	public float fTotalOpenTime;
	public float fClosingTime;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManagerScript> ();

		iTotalNumOfGunTypes = System.Enum.GetNames (typeof( GameManagerScript.GunType)).Length;
	}
	
	// Update is called once per frame
	void Update () 
	{
		fCurrentOpenTime += Time.deltaTime;
		switch(state)
		{
		case RandomBoxStates.Closed:
			fCurrentOpenTime = 0;
			bGunFound = false;
			break;
		case RandomBoxStates.OpenAndPicking:
			if( !bGunFound )
			{
                do
                {
                    iTimesLooped++;
                    gunType = (GameManagerScript.GunType)(Random.Range(1, iTotalNumOfGunTypes));
                } while (gameManager.PlayerHaveGun(gunType.ToString()) || iTimesLooped > iTotalNumOfGunTypes);
                iTimesLooped = 0;
				bGunFound = true;
                if (iTimesLooped > iTotalNumOfGunTypes)
                    gunType = GameManagerScript.GunType.ErrorGun;
			}

			if( fCurrentOpenTime >= fPickingTime )
				state = RandomBoxStates.OpenAndChoose;
			break;
		case RandomBoxStates.OpenAndChoose:
			if( fCurrentOpenTime >= fTotalOpenTime )
			{
				state = RandomBoxStates.Closing;
				fCurrentOpenTime = 0;
				//TODO: disable buying
			}
			break;
		case RandomBoxStates.Closing:
			if( fCurrentOpenTime >= fClosingTime )
			{
				state = RandomBoxStates.Closed;
				fCurrentOpenTime = 0;
			}
			break;
		default:
			Debug.LogError("RandomBoxScript - update - unhandled case - state = " + state.ToString() );
			break;
		}
	}

	void BuyGun( PlayerInventoryScript a_inventory )
	{
		if( !gameManager.Player1Buy( iCost ) || state != RandomBoxStates.OpenAndChoose)//tell gameManager to purchase, if it tells us player cant, return
			return;
		//gameManager.DisplayInstruction( SelectInstruction( gameManager.player.GetComponent<PlayerInventoryScript>() ) , (state == BuyBoardState.Ammo ? (GameMangerScript.BuyDelegate)BuyAmmo : (GameMangerScript.BuyDelegate)BuyGun) );
		a_inventory.AddGun (GunClass.InitFromString(gunType.ToString() ) );
        state = RandomBoxStates.Closing;
		gameManager.EndDisplay ();
	}

	void ActivateBox( PlayerInventoryScript a_inventory)
	{
		if( !gameManager.Player1Buy( iCost ) )//tell gameManager to purchase, if it tells us player cant, return
			return;
		if( state == RandomBoxStates.Closed)
			state = RandomBoxStates.OpenAndPicking;
	}

	public string SelectInstruction( PlayerInventoryScript inventory )
	{
		switch(state)
		{
		case RandomBoxStates.Closed:
			if( gameManager.Player1Buy( iCost , false ) )
				return "Press E to Activate Box";
			else
				return "Not Enough Points";

		case RandomBoxStates.OpenAndChoose:
			return "Press E to Pickup " + gunType.ToString ();
		default:
			return "";
		}
	}

	void OnTriggerStay(Collider c)
	{
		if( c.tag == "Player" )
		{
			gameManager.EndDisplay();
			gameManager.DisplayInstruction( SelectInstruction( c.gameObject.GetComponent<PlayerInventoryScript>() ) , true , state == RandomBoxStates.OpenAndChoose?  (GameManagerScript.BuyDelegate)BuyGun : (GameManagerScript.BuyDelegate)ActivateBox ) ;
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if(c.tag == "Player" )
		{
			gameManager.EndDisplay();
		}
	}
}
