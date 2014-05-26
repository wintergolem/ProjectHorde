using UnityEngine;
using System.Collections;
using System.Linq;

public class GunClass 
{
	//Ammo
	int iMaxAmmo;
	int iMaxAmmoInClip;
	int iCurrentAmmo;
	int iCurrentAmmoInClip;
	float fReloadTime;
	float fTimeReloading = 0;
	bool bReloading = false;

	//Damage
	float fDamage;

	//Fire rate
	float fShotsPerSecond;
	float fTimeBetweenShots; //calculated from fShotsPerSecond
	float fTimeSinceLastShot;
	bool bFired = false;

	//Name
	public string sName { get ; private set; }

	//Upgrade
	bool bUpgraded = false;

	//Range
	float fRange;

	//WeaponType
	public enum WeaponType { Pistol , Rifle };
	public WeaponType weaponType;

	//Firing
	public delegate void FireDelegate( Vector3 a_v3Direction , Vector3 a_v3Origin );
	public FireDelegate Fire;

	void PistolFire( Vector3 a_v3Direction , Vector3 a_v3Origin )
	{
		if( iCurrentAmmoInClip <= 0 )
			Reload();
		if( (iCurrentAmmo <= 0 && iCurrentAmmoInClip <= 0) || bReloading || bFired)
			return;

		iCurrentAmmoInClip--;
		bFired = true;

		if( Physics.Raycast( a_v3Origin , a_v3Direction , out rayCastHit , fRange ) )
		{
			if( rayCastHit.collider.tag == "Enemy")
			{
				rayCastHit.collider.gameObject.SendMessage( "Shot", fDamage , SendMessageOptions.DontRequireReceiver);
			}
		}

	}

	void RifleFire( Vector3 a_v3Direction , Vector3 a_v3Origin )
	{
		if( iCurrentAmmoInClip <= 0 )
			Reload();
		if( (iCurrentAmmo <= 0 && iCurrentAmmoInClip <= 0) || bReloading || bFired)
			return;
		
		iCurrentAmmoInClip--;
		bFired = true;
		
		if( Physics.Raycast( a_v3Origin , a_v3Direction , out rayCastHit , fRange ) )
		{
			if( rayCastHit.collider.tag == "Enemy")
			{
				rayCastHit.collider.gameObject.SendMessage( "Shot", fDamage , SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	void ShotgunFire( Vector3 a_v3Direction , Vector3 a_v3Origin )
	{
		if( iCurrentAmmoInClip <= 0 )
			Reload();
		if( (iCurrentAmmo <= 0 && iCurrentAmmoInClip <= 0) || bReloading || bFired)
			return;
		
		iCurrentAmmoInClip--;
		bFired = true;

		//projectile 1
		if( Physics.Raycast( a_v3Origin , a_v3Direction , out rayCastHit , fRange ) )
		{
			if( rayCastHit.collider.tag == "Enemy")
			{
				rayCastHit.collider.gameObject.SendMessage( "Shot", fDamage , SendMessageOptions.DontRequireReceiver);
			}
		} 
	}

	void SniperFire( Vector3 a_v3Direction , Vector3 a_v3Origin )
	{
		if( iCurrentAmmoInClip <= 0 )
			Reload();
		if( (iCurrentAmmo <= 0 && iCurrentAmmoInClip <= 0) || bReloading || bFired)
			return;
		
		iCurrentAmmoInClip--;
		bFired = true;
		
		RaycastHit[] hits = Physics.RaycastAll( a_v3Origin , a_v3Direction , fRange ).OrderBy(h=>h.distance).ToArray();
		for( int i = 0; i < hits.Length ; i ++ )
		{
			rayCastHit = hits[i];
			if( rayCastHit.collider.tag == "Enemy")
			{
				rayCastHit.collider.gameObject.SendMessage( "Shot", fDamage , SendMessageOptions.DontRequireReceiver);
			}
			else 
			{
				break;
			}
		}

	}

	//Functions
	public void RefillAmmo( int a_iFillAmount = 0 )
	{
		if( a_iFillAmount == 0 )
		{//fill ammo to full
			iCurrentAmmo = iMaxAmmo;
			iCurrentAmmo -= iMaxAmmoInClip; //take ammo from "pile" and put in clip
			iCurrentAmmoInClip = iMaxAmmoInClip;
		}
		else
		{
			iCurrentAmmo += a_iFillAmount;
			if( iCurrentAmmo > iMaxAmmo )//cap ammo to max
				iCurrentAmmo = iMaxAmmo;
		}
	}

	void Upgrade( int a_NewMaxAmmo , int a_NewMaxClipAmmo , float a_fNewDamage , float a_NewSPS , float a_NewRange , FireDelegate a_fire )
	{
		if (bUpgraded)
						return;
		iMaxAmmo = a_NewMaxAmmo;
		iMaxAmmoInClip = a_NewMaxClipAmmo;
		fDamage = a_fNewDamage;
		fShotsPerSecond = a_NewSPS;
		fRange = a_NewRange;
		Fire = a_fire;

		bUpgraded = true;
	}

	//public functions
	public string AmmoToString()
	{
		//Debug.Log (iCurrentAmmo.ToString ());
		string s = "";
		s += iCurrentAmmoInClip + " / " + iCurrentAmmo;

		return s;
	}

	public void Reload( bool a_bWait = true)
	{
		if( a_bWait )
		{
			if( !bReloading )//make sure that we are not already reloading
			{
				bReloading = true;
				fTimeReloading = 0;
			}
		}
		else
		{
			if( iCurrentAmmo > iMaxAmmoInClip - iCurrentAmmoInClip )
			{
				iCurrentAmmo -= iMaxAmmoInClip - iCurrentAmmoInClip;
				iCurrentAmmoInClip = iMaxAmmoInClip;
			}
			else
			{
				iCurrentAmmoInClip += iCurrentAmmo;
				iCurrentAmmo = 0;
			}
		}
	}

	public void Update(float a_fDeltaTime)
	{
		if( bFired )
		{
			fTimeSinceLastShot += a_fDeltaTime;
			if( fTimeSinceLastShot >= fTimeBetweenShots )
			{
				bFired = false;
				fTimeSinceLastShot = 0;
			}
		}
		else if( bReloading )
		{
			fTimeReloading += a_fDeltaTime;
			if( fTimeReloading >= fReloadTime )
			{
				fTimeReloading = 0;
				bReloading = false;
				Reload(false);
			}
		}
	}
	//reuseable variables declared here to lower garbage
	RaycastHit rayCastHit;

	//Initing for all guns avaible in game
	void Init( string a_sName , int a_iMaxAmmo , int a_iMaxAmmoInClip , 
	          float a_fDamage , float a_sps , float a_fRange ,
	          float a_ReloadTime ,
	          WeaponType a_weaponType , FireDelegate a_fire )
	{
		sName = a_sName;
		iMaxAmmo = a_iMaxAmmo;
		iMaxAmmoInClip = a_iMaxAmmoInClip;
		
		iCurrentAmmo = iMaxAmmo - iMaxAmmoInClip;
		iCurrentAmmoInClip = iMaxAmmoInClip;
		
		fDamage = a_fDamage;
		fShotsPerSecond = a_sps;
		fTimeBetweenShots = 1 / fShotsPerSecond;
		
		fRange = a_fRange;
		fReloadTime = a_ReloadTime;
		weaponType = a_weaponType;
		Fire = a_fire;
	}
	



	public static GunClass InitFromString(string sGunName)
	{
		switch (sGunName)
		{
		case "p9i8":
			return Init_P9i8();
		case "p0k6":
			return Init_P0k6();
		case "rt96":
			return Init_rt96();
		case "DoubleBarrel":
			return Init_DoubleBarrel();
		default:
			return Init_ErrorGun();
		}
	}
	public static GunClass Init_ErrorGun()
	{
		GunClass gun = new GunClass ();
		gun.Init ( "ErrorGun" , 1800, 9, 15, 1, 15, 1, WeaponType.Pistol, gun.PistolFire);
		return gun;
	}
	//Pistols
	//p9i8
	public static GunClass Init_P9i8()
	{
		GunClass gun = new GunClass ();
		gun.Init ( "p9i8" , 1800, 9, 15, 4, 15, 1, WeaponType.Pistol, gun.PistolFire);
		return gun;
	}
	//p0k6
	public static GunClass Init_P0k6()
	{
		GunClass gun = new GunClass ();
		gun.Init ( "p0k6" , 32, 9, 50, 8, 15, 0.4f, WeaponType.Pistol, gun.PistolFire);
		return gun;
	}

	//Rifles
	//rt-96
	public static GunClass Init_rt96()
	{
		GunClass gun = new GunClass ();
		gun.Init ( "rt96" , 18, 9, 15, 15, 15, 1, WeaponType.Rifle, gun.RifleFire);
		return gun;
	}

	//shotgun
	//DoubleBarrel
	public static GunClass Init_DoubleBarrel()
	{
		GunClass gun = new GunClass ();
		gun.Init ( "DoubleBarrel" , 18, 2, 150, 0.5f, 20, 10, WeaponType.Rifle, gun.ShotgunFire);
		return gun;
	}
}
