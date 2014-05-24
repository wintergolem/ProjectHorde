using UnityEngine;
using System.Collections;

public class GameMangerScript : MonoBehaviour {

	public enum GunType { p9i8 , p0k6 , rt96 , DoubleBarrel };

	public delegate void BuyDelegate(PlayerInventoryScript inventory);

	public GameObject player;
	public HUDManager hudManager;
	public GUIText guiText;


	//private base variables
	bool bCollidingBuyBoard = false;

	int iNextMessageID = 0;
	int iCurrentMessageID; //id for the current message bein displayed

	//public base variables

	//Score values
	public int iPlayerScore = 0;
	public int iKillScore;
	public int iHeadShotScore;
	public int iMeleeScore;
	public int iCurrentWave {get; private set;}

	PlayerInventoryScript playerInventory;

	public EnemyManager enemyManager;


	public BuyDelegate buyDelegate;

	public bool bGameOver; // used by everything else if check if game is over
	// Use this for initialization
	void Start () 
	{
		playerInventory = player.GetComponent<PlayerInventoryScript> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		guiText.text = playerInventory.gunInventory [playerInventory.iActiveIndex].sName;

		if (enemyManager.enemies.Count <= 0)
						enemyManager.IncreaseWave ();
	}

	void FixedUpdate()
	{
		
	}

	public void ActivateButton()
	{
		if( bCollidingBuyBoard )
		{
			buyDelegate( playerInventory );
		}
	}

	public void DisplayInstruction( string a_instruction , BuyDelegate a_delegate )
	{
		hudManager.DisplayInstruction (a_instruction);
		bCollidingBuyBoard = true;
		buyDelegate = a_delegate;
	}

	public void EndDisplay()
	{
		bCollidingBuyBoard = false;
		hudManager.EndDisplay ();
	}

	public int GetMessageID()//probably don't need
		//, but is there is as a solution having multiple messages wanting to be displayed at one time
	{
		iNextMessageID++;
		return iNextMessageID - 1;
	}

	public bool Player1Buy ( int a_iCost , bool a_bBuyIt = true)
	{//check if player can afford cost, if so buy it
		if( iPlayerScore >= a_iCost )
		{
			if( a_bBuyIt ) //if unchanged, buy it, or else just return saying player can afford it
				iPlayerScore -= a_iCost;
			return true;
		}
		//TODO: add visual feedback to cant afford it
		return false; //player cant afford it
	}

}
