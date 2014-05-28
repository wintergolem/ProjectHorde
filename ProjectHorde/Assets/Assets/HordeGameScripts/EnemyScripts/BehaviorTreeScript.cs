using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Tree runs each node depth first until a success is found
//Node executable should contain a condition for pass/fail or action
//Execute funcs hail from tree class ( to player doesn't have to be sent from node to node)
//pass: run children if any
//fail: return
public delegate bool Execute( List<Node> a_children , BasicZombieScript a_enemy);

public class Node
{
	bool bNoChildren = false;

	public Execute execute;

	public List<Node> children;

	public void Init( Execute a_exe , List<Node> a_children )
	{
		children = new List<Node> ();
		execute = a_exe;
		if( a_children == null)
			bNoChildren = true;
		else
			children.AddRange (a_children);
	}

	public void AddChild( Node a_node)
	{
		children.Add (a_node);
		bNoChildren = false;
	}
}

public partial class BehaviorTree 
{
	List<Node> nodes;

	PlayerInventoryScript player; //still has transfrom and such, plus we can how check for power-ups and whatnot
	GameManagerScript gameManager;
	EnemyManager enemyManager;

	bool bInitRun = false;

	public void Init( PlayerInventoryScript a_player, GameManagerScript a_gameManager , EnemyManager a_enemyManager)
	{
		nodes = new List<Node> ();

		player = a_player;
		gameManager = a_gameManager;
		enemyManager = a_enemyManager;
		bInitRun = true;
		Build ();
	}

	public void Run( BasicZombieScript a_enemy )
	{
		if (!bInitRun)
					Debug.LogError ("Behavior Tree called to run without running Init - be sure to call Init first");
		else
			nodes [0].execute ( nodes[0].children , a_enemy );
	}

}
