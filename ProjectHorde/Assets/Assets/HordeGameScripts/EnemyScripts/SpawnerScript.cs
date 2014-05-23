using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

	GameObject enemy; //reused everytime spawn is called

	Transform trans;

	public bool bValid = true;
	
	// Use this for initialization
	void Start () {
		trans = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public BasicZombieScript Spawn( GameObject prefab , EnemyManager enemyManager)
	{//will eventually handle checking if it is a valid spawner
		//then return false if it isn't
		enemy = Instantiate (prefab, trans.position, Quaternion.identity) as GameObject;
		enemy.GetComponent<BasicZombieScript> ().manager = enemyManager;
		enemy.GetComponent<BasicZombieScript> ().Init (enemyManager.GrabEnemyHealth(), enemyManager.GrabEnemyArmor());
		return enemy.GetComponent<BasicZombieScript> ();
	}
}
