using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {


    PlayerInventoryScript playerInventory;

	public enum GunType { ErrorGun , p9i8 , p0k6 , rt96 , DoubleBarrel , FarSight };
    public enum ScoreType { EnemyShot , HeadShot , EnemyKilled , HeadShotKill , Melee , MeleeKill};

	public delegate void BuyDelegate(PlayerInventoryScript inventory);

	public GameObject player;
	public HUDManager hudManager;

    public EnemyManager enemyManager;


    public BuyDelegate buyDelegate;


	//private base variables
	bool bCollidingBuyBoard = false;
   

	int iNextMessageID = 0;
	int iCurrentMessageID; //id for the current message bein displayed

    float fTimeSinceEndOfLastWave = 0;

	//public base variables
    public bool bBetweenWaves = true;
	public bool bPowerOn = false;
    public bool bGameOver; // used by everything else if check if game is over

	public int iPlayerScore = 0;
    public int iEnemyShotScore;
	public int iKillScore;
	public int iHeadShotScore;
	public int iMeleeScore;
    public int iCurrentWave {get; private set;}

    public float fTimeBetweenWaves = 10;

	// Use this for initialization
	void Start () 
	{
		playerInventory = player.GetComponent<PlayerInventoryScript> ();
        iCurrentWave = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(bBetweenWaves)
        {
            fTimeSinceEndOfLastWave += Time.deltaTime;
            if( fTimeSinceEndOfLastWave >= fTimeBetweenWaves)
            {
                bBetweenWaves = false;
                iCurrentWave++;
                enemyManager.IncreaseWave();
            }
        }
        else if (enemyManager.enemies.Count <= 0 && enemyManager.AllEnemiesSpawned())
        {
            bBetweenWaves = true;
            fTimeSinceEndOfLastWave = 0;
           
        }
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

	public void DisplayInstruction( string a_instruction , bool a_UseDelegate , BuyDelegate a_delegate )
	{
		hudManager.DisplayInstruction (a_instruction);
		bCollidingBuyBoard = a_UseDelegate;
		buyDelegate = a_delegate;
	}

	public void EndDisplay()
	{
		bCollidingBuyBoard = false;
		hudManager.EndDisplay ();
	}

	public void EndDisplay( string a_string)
	{
		if (hudManager.CompareInstruction (a_string))
		{
			hudManager.EndDisplay ();
			bCollidingBuyBoard = false;
		}
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

	public void TurnOnPower()
	{
		bPowerOn = true;
	}

    public bool PlayerHaveGun( string a_sGunName)
    {
        return playerInventory.HaveGun(a_sGunName);
    }

    public void Player1Score ( ScoreType a_type)
    {
        switch (a_type)
        {
            case ScoreType.EnemyShot:
                iPlayerScore += iEnemyShotScore;
                break;
            case ScoreType.EnemyKilled:
                iPlayerScore += iKillScore;
                break;
            case ScoreType.HeadShot:
                iPlayerScore += iHeadShotScore;
                break;
            case ScoreType.HeadShotKill:
                iPlayerScore += iHeadShotScore + iKillScore;
                break;
            case ScoreType.Melee:
                iPlayerScore += iMeleeScore;
                break;
            case ScoreType.MeleeKill:
                iPlayerScore += iMeleeScore + iKillScore;
                break;
            default:
                Debug.LogError("Bad ScoreType - Player1Score() - GameManagerScript");
                break;
        }
    }
}
