using UnityEngine;
using System.Collections;

public class BasicZombieScript : MonoBehaviour {

	float fHealth;
	float fAmour;

	Vector3 v3TargetLocPlayer;

	public enum BasicZombieState { Stalking , Idle , Attacking , Dead};

	public BasicZombieState tree_state;		//variable tree gets to set, this script checks against this state to handle transitions
	public BasicZombieState state { get; protected set;}
	
	public NavMeshAgent nav;
	
	public float fAttackDistance;
	[HideInInspector]
	public EnemyManager manager; //after Instantiate, spawner will assign this variable

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		Perception ();
		Decision ();
		Controller ();
	}

	void Perception()
	{
		//omniscience
		v3TargetLocPlayer = manager.GrabPlayerPos ();
	}

	void Decision()
	{
		//handled by enemyManager
		if( state != BasicZombieState.Dead)
			state = tree_state;

		if (fHealth <= 0)
			state = BasicZombieState.Dead;
	}

	void Controller()
	{
		switch( state)
		{
		case BasicZombieState.Attacking:
			nav.enabled = false;
			break;
		case BasicZombieState.Idle:
			nav.enabled = false;
			break;
		case BasicZombieState.Stalking:
			nav.enabled = true;
			nav.SetDestination( manager.GrabPlayerPos() );
			break;
		case BasicZombieState.Dead:
			nav.enabled = false;
			Destroy(gameObject);
			break;
		default:
			break;
		}
	}

	public void Init( float a_fHealth , float a_fArmor )
	{
		fHealth = a_fHealth;
		fAmour = a_fArmor;
	}

	public void Shot( float a_fDamage )
	{
		fHealth -= a_fDamage - fAmour;
		Debug.Log("hit - " + fHealth.ToString() + " health - " + (a_fDamage - fAmour).ToString() + " damage taken ( " + a_fDamage.ToString() + " )" );
	}
}
