﻿using UnityEngine;
using System.Collections;

public class PlayerInventoryScript : MonoBehaviour {

	public GunClass[] gunInventory; // element 0 pistol only
	public int iMaxNumberOfWeapons = 3;
	public int iActiveIndex { get; private set; }
	public int iMaxIndex;
	// Use this for initialization
	void Start () 
	{
		iActiveIndex = 0;
		gunInventory = new GunClass[iMaxNumberOfWeapons];
		iMaxIndex = iMaxNumberOfWeapons - 1; //minus one to indexing from zero
		gunInventory [0] = GunClass.Init_P9i8 ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		gunInventory [iActiveIndex].Update (Time.deltaTime);

		if( Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			RotateInventory(true);
		}
		else if(Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			RotateInventory(false);
		}
	}

	void RotateInventory( bool a_bForward)
	{
		if(a_bForward)
		{
			iActiveIndex++;
			if( iActiveIndex > iMaxIndex )
				iActiveIndex = 0;
		}
		else
		{
			iActiveIndex--;
			if( iActiveIndex < 0 )
				iActiveIndex = iMaxIndex;
		}

		if( gunInventory[iActiveIndex] == null )
			RotateInventory(a_bForward);
	}

	public void Reload()
	{
		gunInventory [iActiveIndex].Reload ();
	}

	public void AddGun( GunClass gun )
	{
		if( gun.weaponType == GunClass.WeaponType.Pistol )
		{
			gunInventory[0] = gun;
		}
		else
		{
			for( int i =0; i < iMaxIndex ; i++ )//find empty slot
			{
				if( gunInventory[i] == null)
				{
					gunInventory[i] = gun; //fill it with the gun
					return;					//and we're done
				}
			}
			//no empty slots, replace current gun, unless it's a pistol, then replace next gun
			gunInventory[iActiveIndex != 0 ? iActiveIndex : iActiveIndex++] = gun;
		}
	}
}
