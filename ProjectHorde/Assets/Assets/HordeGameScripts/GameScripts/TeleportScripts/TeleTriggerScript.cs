using UnityEngine;
using System.Collections;

public class TeleTriggerScript : MonoBehaviour {

    public bool bTriggered = false;
    public bool bActive = false;
	public bool bNeedToPurchaseTrigger = false;

	public int iBuyCost = 0;

    GameManagerScript gameManager;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if( gameManager.bPowerOn )
			bActive = true;
	}

	public void TriggerPad( PlayerInventoryScript a_inventory)
	{
		if( bActive )
			bTriggered = true;
		gameManager.EndDisplay ();
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            if( !bActive )
				gameManager.DisplayInstruction("Turn On Power First" , false, null);
			else if(!bTriggered )
				gameManager.DisplayInstruction("Activate to Trigger" , true, (GameManagerScript.BuyDelegate)(TriggerPad));
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            gameManager.EndDisplay();
        }
    }
}
