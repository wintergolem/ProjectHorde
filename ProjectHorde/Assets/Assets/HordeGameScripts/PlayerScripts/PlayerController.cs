using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float fMoveSpeed;
	public float fRotateSpeed;
	public float fHealth { get; private set; }
	public float fArmor { get; private set; }
	public float fTimeBetweenDamages;
	public float fTimeBeforeHealthing;
	public float fHealingAmount = 5;

	public Transform tFeet;

	public PlayerInventoryScript inventory;

	public Vector3 v3JumpForce;
	

	//private variables
	bool bGrounded = false;

	float fTimeSinceLastDamage = 0;

	RaycastHit rayHit;

	Vector3 moveForce = Vector3.zero;

	GameMangerScript gameManager;

	// Use this for initialization
	void Start () {
		bGrounded = true;
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameMangerScript> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//check if grounded
		if( !bGrounded && Physics.Linecast( transform.position , tFeet.position , out rayHit ) )
		{
			if( rayHit.collider.tag == "Ground")
				bGrounded = true;
		}

		Move ();

		if( Input.GetMouseButton( 0 ) )
			inventory.gunInventory[inventory.iActiveIndex].Fire( transform.forward , transform.position );
		if( Input.GetKeyDown( KeyCode.R ) )
		{
			inventory.Reload();
		}
		if( Input.GetKeyDown(KeyCode.E) )
		{
			gameManager.ActivateButton();
		}

		fTimeSinceLastDamage += Time.deltaTime;

		//check if enough time has passed to start health
		if( fTimeSinceLastDamage > fTimeBeforeHealthing )
		{
			fHealth += fHealingAmount;
		}
	}

	void Move()
	{
		moveForce = Vector3.zero;
		//jump
		if( Input.GetKey(KeyCode.Space) && bGrounded)
		{
			moveForce += v3JumpForce;
			bGrounded = false;
		}

		//move
		if( bGrounded )
		{
			if( Input.GetKey( KeyCode.W) )
			{
				moveForce += transform.forward;
			}
			if( Input.GetKey( KeyCode.S) )
			{
				moveForce += -transform.forward;
			}
			//sidestep
			if( Input.GetKey(KeyCode.A) )
			{
				moveForce += -transform.right;
			}
			if( Input.GetKey(KeyCode.D) )
			{
				moveForce += transform.right;
			}
		}
		moveForce.Normalize ();
		moveForce.x *= fMoveSpeed + v3JumpForce.x;
		moveForce.y *= v3JumpForce.y + fMoveSpeed;
		moveForce.z *= fMoveSpeed + v3JumpForce.z;
		rigidbody.AddForce (moveForce * Time.deltaTime);
	}

	public void TakeDamage(float a_fDamage)
	{
		if( fTimeSinceLastDamage > fTimeBetweenDamages )
		{
			fTimeSinceLastDamage = 0;
			fHealth -= a_fDamage - fArmor;
		}
	}

	public void IncreaseArmor( float a_fIncrease )
	{
		fArmor += a_fIncrease;
	}
}
