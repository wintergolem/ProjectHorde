using UnityEngine;
using System.Collections;

public class GameMangerScript : MonoBehaviour {

	public delegate void BuyDelegate(PlayerInventoryScript inventory);

	public NavMeshAgent enemyNav;
	public GameObject player;
	public HUDManager hudManager;

	//Score values
	public int iPlayerScore = 0;
	public int iKillScore;
	public int iHeadShotScore;
	public int iMeleeScore;

	PlayerInventoryScript playerInventory;

	public GUIText guiText;

	bool bCollidingBuyBoard = false;
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
	}

	void FixedUpdate()
	{
		//enemyNav.SetDestination (player.transform.position);
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
