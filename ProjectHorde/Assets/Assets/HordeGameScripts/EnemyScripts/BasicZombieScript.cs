using UnityEngine;
using System.Collections;

public class BasicZombieScript : MonoBehaviour {

	float fHealth;
	float fAmour;

    int iWave; //wave spawned on

	Vector3 v3TargetLocPlayer;

	public enum BasicZombieState { Stalking , Idle , Attacking , Dead};

	public BasicZombieState tree_state;		//variable tree gets to set, this script checks against this state to handle transitions
	public BasicZombieState state { get; protected set;}
	
	public NavMeshAgent nav;

    public Transform headTrans;

	public float fAttackDistance;
	[HideInInspector]
	public EnemyManager manager; //after Instantiate, spawner will assign this variable

	// Use this for initialization
	void Start () 
	{
        iWave = manager.manager.iCurrentWave;
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

    public void HeadShot( float a_fDamage)
    {
        fHealth -= a_fDamage;
        if (fHealth > 0)
            manager.PlayerScorePassThrough(GameManagerScript.ScoreType.HeadShot);
        else
            manager.PlayerScorePassThrough(GameManagerScript.ScoreType.HeadShotKill);
        //Debug.Log("Headshot - " + fHealth.ToString() + " health - " + (a_fDamage).ToString() + " damage taken");
    }

	public void Shot( Vector2 a_v2DamgeYcoord)
	{
        //print(a_v2DamgeYcoord.y.ToString());
        if (a_v2DamgeYcoord.y > headTrans.position.y)
            HeadShot(a_v2DamgeYcoord.x);
        else
        {
            fHealth -= a_v2DamgeYcoord.x - fAmour;
            if (fHealth > 0)
                manager.PlayerScorePassThrough(GameManagerScript.ScoreType.EnemyShot);
            else
                manager.PlayerScorePassThrough(GameManagerScript.ScoreType.EnemyKilled);
            //Debug.Log("hit - " + fHealth.ToString() + " health - " + (a_v2DamgeYcoord.x - fAmour).ToString() + " damage taken ( " + a_v2DamgeYcoord.x.ToString() + " )");
        }
	}
}
