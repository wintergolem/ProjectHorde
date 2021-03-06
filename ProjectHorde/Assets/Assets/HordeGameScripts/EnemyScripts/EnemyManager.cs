﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	BasicZombieScript temp;

	BehaviorTree tree;

	public GameManagerScript manager; //temp public

	bool bReuseable;

	int iSpawnerIndex;
	int iCurrentWaveEnemyCount = 0;
	int iMaxEnemies = 0;

	float fTimeSinceLastSpawn = 0;
	float fCurrentEnemyHealth;
	float fCurrentEnemyArmor;

	public bool bIdle = false; //enemies check this bool to know whether or not to enter idle state
	public bool bDistracted = false;

	public GameObject enemyPrefab;

	public int iStartingEnemyCount = 5;
	public int iEnemyCountIncreaseAmount = 4;
	public int iAttackDistance = 2;

	public List<BasicZombieScript> enemies;

	//public float fTimeBetweenSpawns;
	public float fEnemyStartingHealth = 10;
	public float fEnemyStartingArmor = 0;
	public float fEnemyHealthIncreasePerWave = 5;
	public float fEnemyArmorIncreasePerWave = 5;

	public List<SpawnerScript> spawners;

    public Vector2 v2SpawnVari;
	public Vector3 v3target;

	//private functions
	void Start () 
	{
		manager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManagerScript>();
		enemies = new List<BasicZombieScript> ();
		GameObject[] temp  = GameObject.FindGameObjectsWithTag ("Spawner");
		spawners = new List<SpawnerScript> ();
		for( int i = 0 ; i < temp.Length; i++ )
		{
			spawners.Add( temp[i].GetComponent<SpawnerScript>() );
		}
		tree = new BehaviorTree ();
		tree.Init (manager.player.GetComponent<PlayerInventoryScript> (), manager, this);

		fCurrentEnemyArmor = fEnemyStartingArmor;
		fCurrentEnemyHealth = fEnemyStartingHealth;
	}

	void Update () 
	{
        //spawn new enemies
		fTimeSinceLastSpawn += Time.deltaTime;
		if( iCurrentWaveEnemyCount < iStartingEnemyCount + (iEnemyCountIncreaseAmount * manager.iCurrentWave ) )
            if( fTimeSinceLastSpawn >= Random.Range(v2SpawnVari.x , v2SpawnVari.y ) )
		    {
                fTimeSinceLastSpawn = 0;
			    AddEnemy();
		    }

		do{//remove dead enemies
			bReuseable = false;
			for( int i = 0 ; i < enemies.Count ; i++ )
			{
				if( enemies[i] == null)
				{
					bReuseable = true;
					enemies.RemoveAt(i);
					break;
				}
			}
		}while(bReuseable);
	}

	void FixedUpdate()
	{
        if (!manager.bBetweenWaves)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null)
                    RunThroughTree(enemies[i]);
            }
        }
	}

	void AddEnemy()
	{
		if( spawners.Count <= 0 )
			return;
		do
		{
			iSpawnerIndex = Random.Range (0, spawners.Count);
			temp = spawners [iSpawnerIndex].Spawn (enemyPrefab, this);
		}while( temp == null ) ;
		enemies.Add (temp);
		iCurrentWaveEnemyCount++;
	}

	//public functions

	public Vector3 GrabPlayerPos()
	{
		if( bDistracted )
			return v3target;
		return manager.player.transform.position;
	}

	public float GrabEnemyHealth()
	{
		return fCurrentEnemyHealth;
	}

	public float GrabEnemyArmor()
	{
		return fCurrentEnemyArmor;
	}

	public void IncreaseWave()
	{
		fCurrentEnemyArmor = fEnemyStartingArmor + fEnemyArmorIncreasePerWave * manager.iCurrentWave;
		fCurrentEnemyHealth = fEnemyStartingHealth + fEnemyHealthIncreasePerWave * manager.iCurrentWave;

		iCurrentWaveEnemyCount = 0;
		iMaxEnemies = iStartingEnemyCount + iEnemyCountIncreaseAmount * manager.iCurrentWave;
	}

	public void RunThroughTree( BasicZombieScript a_enemy )
	{
		tree.Run (a_enemy);
	}

	public void SetDistracted( bool a_bDist , Vector3 a_v3NewTarget )
	{
		bDistracted = a_bDist;
		if( bDistracted)
			v3target = a_v3NewTarget;
	}

    public bool AllEnemiesSpawned()
    {
        if (iCurrentWaveEnemyCount == iStartingEnemyCount + (iEnemyCountIncreaseAmount * manager.iCurrentWave))
            return true;
        else if (iCurrentWaveEnemyCount > iStartingEnemyCount + (iEnemyCountIncreaseAmount * manager.iCurrentWave))
            print("Error too any enemies spawned - EnemyManagerScript - AllEnemiesSpawned()");
        return false;
    }

    public void PlayerScorePassThrough( GameManagerScript.ScoreType a_type)
    {
        manager.Player1Score(a_type);
    }
}
