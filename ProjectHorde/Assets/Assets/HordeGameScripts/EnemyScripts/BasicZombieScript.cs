using UnityEngine;
using System.Collections;

public class BasicZombieScript : MonoBehaviour {

	enum BasicZombieState { Stalking , Idle , Attacking };
	BasicZombieState state;
	public GameMangerScript gameManager;

	public NavMeshAgent nav;

	public float fAttackDistance;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(state)
		{
		case BasicZombieState.Stalking:
			nav.SetDestination(gameManager.player.transform.position);
			break;
		case BasicZombieState.Attacking:
			break;
		case BasicZombieState.Idle:
			break;
		default:
			break;
		}
	}

	void SelectState()
	{
		//check if game is over
		if( gameManager.bGameOver

		   )
			state = BasicZombieState.Idle;

		//check if is close enough to attack
		if( Vector3.Distance( transform.position , gameManager.player.transform.position ) <= fAttackDistance )
		{
			state = BasicZombieState.Attacking;
		}
		else
		{
			state = BasicZombieState.Stalking;
		}
	}
}
