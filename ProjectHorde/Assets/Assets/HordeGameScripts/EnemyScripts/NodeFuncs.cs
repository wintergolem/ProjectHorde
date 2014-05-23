using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class BehaviorTree
{
	//node execute funcs
	bool bExeReturned = false;

	void Build()
	{
		Node RootNode = new Node ();
		Node IdleNode = new Node ();
		Node CheckAttackDistanceNode = new Node ();
		Node ChaseNode = new Node ();

		ChaseNode.Init (Chase, null);
		IdleNode.Init (CheckShouldBeIdle, null);
		CheckAttackDistanceNode.Init (CheckAttackDistance, null);

		List<Node> children = new List<Node> ();
		children.Add (IdleNode);
		children.Add (CheckAttackDistanceNode);
		children.Add (ChaseNode);

		RootNode.Init (RunChildren, children );
		nodes.Add( RootNode )  ;
	}

	public bool RunChildren( List<Node> children , BasicZombieScript a_enemy)
	{
		for( int i = 0 ; i < children.Count ; i++ )
		{
			bExeReturned = children[i].execute( children , a_enemy );
			if( bExeReturned )//==true
				return true;

		}
		return false;
	}

	public bool CheckAttackDistance( List<Node> children , BasicZombieScript a_enemy)
	{
		if( Vector3.Distance(player.transform.position , a_enemy.transform.position ) < enemyManager.iAttackDistance )
		{//player is in attack range of enemy, tell enemy to switch to attack state
			a_enemy.tree_state = BasicZombieScript.BasicZombieState.Attacking;
			return true;
		}
		return false;
	}

	public bool CheckShouldBeIdle( List<Node> children , BasicZombieScript a_enemy)
	{
		if( enemyManager.bIdle )
		{
			a_enemy.tree_state = BasicZombieScript.BasicZombieState.Idle;
			return true;
		}
		return false;
	}

	public bool Chase( List<Node> children , BasicZombieScript a_enemy)
	{
		a_enemy.tree_state = BasicZombieScript.BasicZombieState.Stalking;
		return true;
	}
}