using UnityEngine;
using System.Collections;

public class TeleporterScript : MonoBehaviour {

	public TeleTriggerScript[] triggers;

	public Transform[] placesToSpawn;

	//public base variables
	public float fCoolDownTime = 1;

	//private unique variables
	GameManagerScript gameManager;

	//private base variables
	bool bActive = false;
	bool bAllTriggered = false;
	bool bPressToActivate = true;
	bool bCoolingDown = false;

	int i; //reused for loops

	float fTimeCooling = 0;

	// Use this for initialization
	void Start () 
	{
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManagerScript> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( gameManager.bPowerOn )
			bActive = true;

		if( bCoolingDown )
		{
			fTimeCooling += Time.deltaTime;
			if(fTimeCooling >= fCoolDownTime )
			{
				fTimeCooling = 0;
				bCoolingDown = false;
				Reset();
			}
		}

		if( bActive && !bAllTriggered )
		{
			CheckTriggers();
		}
	}

	void CheckTriggers()
	{
		bAllTriggered = true;
		for( i = 0; i < triggers.Length ; i++ )
		{
			if( !triggers[i].bTriggered )
			{
				bAllTriggered = false;
				return;
			}
		}
	}

	void Reset()
	{
		for( i = 0; i < triggers.Length ; i++ )
		{
			triggers[i].bTriggered = false;
		}
		gameManager.EndDisplay ("Cooling Down");
		bAllTriggered = false;
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Player")
		{
			if( !bActive )
				gameManager.DisplayInstruction("Turn On Power First" , false, null);
			else if(!bAllTriggered )
				gameManager.DisplayInstruction("Activate Triggers" , false, null);
			else if(bCoolingDown )
				gameManager.DisplayInstruction("Cooling Down" , false, null); //if text is changed, change Reset() text too
			else if(bPressToActivate)
				gameManager.DisplayInstruction("Activate to Teleport" , true , (GameManagerScript.BuyDelegate)(Teleport));
			else
				Teleport(gameManager.player);
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if (c.tag == "Player")
		{
			gameManager.EndDisplay();
		}
	}

	public void Teleport( PlayerInventoryScript a_inventory )
	{
		Teleport (a_inventory.gameObject);
	}

	public void Teleport( GameObject a_player )
	{
		a_player.transform.position = placesToSpawn [Random.Range (0, placesToSpawn.Length)].position; //plus due to Random.Range excludive max
		bCoolingDown = true;
	}
}
