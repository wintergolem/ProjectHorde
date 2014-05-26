using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

	GameMangerScript gameManager;
	PlayerInventoryScript playerInventory;

	public GUIText weaponName;
	public GUIText weaponAmmo;
	public GUIText instruction;

	GunClass activeGun;
	// Use this for initialization
	void Start () 
	{
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameMangerScript>();
		playerInventory = gameManager.player.GetComponent<PlayerInventoryScript> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		activeGun = playerInventory.gunInventory [playerInventory.iActiveIndex];

		weaponAmmo.text = activeGun.AmmoToString ();
		weaponName.text = activeGun.sName;
	}

	public void DisplayInstruction( string a_instruction)
	{
		instruction.text = a_instruction;
	}

	public void EndDisplay ()
	{
		instruction.text = "";
	}

	public bool CompareInstruction(string a_string)
	{
		if (a_string == instruction.text)
			return true;
		return false;
	}
}
