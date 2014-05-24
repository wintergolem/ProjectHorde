using UnityEngine;
using System.Collections;

public class TeleTriggerScript : MonoBehaviour {

    public bool bTriggered = false;
    public bool bActive = false;

    GameMangerScript gameManager;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameMangerScript>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            if( !bActive )
                gameManager.DisplayInstruction("")
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
