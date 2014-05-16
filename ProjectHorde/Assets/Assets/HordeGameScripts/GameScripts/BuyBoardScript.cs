using UnityEngine;
using System.Collections;

public class BuyBoardScript : MonoBehaviour {

	enum BuyBoardState { Ammo , Gun };
	BuyBoardState state;
	GameMangerScript gameManager;

	public enum ReplenishType { Flat , Percentage };

	public enum GunType { p9i8 , p0k6 , rt96 , DoubleBarrel };

	public GunType gunType;

	public int iCostGunPurchase;
	public int iCostAmmoPurchase;

	public ReplenishType replenishType;
	public int iAmmoReplenished;
	public float fPercentageReplenished;

	// Use this for initialization
	void Start () 
	{
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameMangerScript>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public string SelectInstruction( PlayerInventoryScript inventory )
	{

		for( int i = 0; i < inventory.iMaxIndex ; i++ )
		{
			if( inventory.gunInventory[i] != null )
			{
				if( inventory.gunInventory[i].sName == gunType.ToString() )
				{
					state = BuyBoardState.Ammo;
					return "Press E to Buy Ammo ( " + iCostAmmoPurchase.ToString() + " )";
				}
			}
		}
		state = BuyBoardState.Gun;
		return "Press E to Buy " + gunType.ToString () + " ( " + iCostGunPurchase.ToString () + " ) ";
	}

	void BuyAmmo(PlayerInventoryScript a_inventory)
	{
		if( !gameManager.Player1Buy( iCostAmmoPurchase ) )//tell gameManager to purchase, if it tells us player cant, return
			return;

		bool bSuccess = false;
		for( int i = 0; i < a_inventory.iMaxIndex ; i++ )//find our gun
		{
			if( a_inventory.gunInventory[i] != null )
			{
				if( a_inventory.gunInventory[i].sName == gunType.ToString() )
				{
					a_inventory.gunInventory[i].RefillAmmo( iAmmoReplenished ); //add ammo to gun
					bSuccess = true;
				}
			}
		}
		if( !bSuccess )
			Debug.LogError( "BuyBoardScript - BuyAmmo - Gun not found in inventory");
	}

	void BuyGun( PlayerInventoryScript a_inventory )
	{
		if( !gameManager.Player1Buy( iCostGunPurchase ) )//tell gameManager to purchase, if it tells us player cant, return
			return;

		a_inventory.AddGun (GunClass.InitFromString(gunType.ToString() ) );
	}
	
	void OnTriggerEnter(Collider c)
	{
		if( c.tag == "Player" )
		{
			gameManager.DisplayInstruction( SelectInstruction( c.gameObject.GetComponent<PlayerInventoryScript>() ) , (state == BuyBoardState.Ammo ? (GameMangerScript.BuyDelegate)BuyAmmo : (GameMangerScript.BuyDelegate)BuyGun) );
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
