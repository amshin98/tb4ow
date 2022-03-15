using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
	[Header("Movement Parameters")]
	public CharacterController2D controller = null;
	public float movementSpeed = 40f;

	[Header("Weapon Interact Parameters")]
	public float pickupRange = 1.0f;

	[Header("Player Parameters")]
	public WeaponController curWeapon;
	public float curPercent = 0;
	public bool isAI = false;
	public bool canMove = true;

	[Header("Script References")]
	public Transform aiTargetPos;

	protected float horizontalMove = 0f;
	protected bool jump = false;

	protected  float knockbackStartTime = 0;
	protected  float knockbackDuration = 2f;
	

	private void Awake()
	{
		if (controller == null)
			controller = GetComponent<CharacterController2D>();
	}

	void Update()
	{

		if (Time.time - knockbackStartTime >= knockbackDuration)
		{
			controller.animator.SetFloat("Knocking", 0.0f);
		}


		horizontalMove = Input.GetAxisRaw("Horizontal") * movementSpeed;
		jump = Input.GetButton("Jump");
		

		if(curWeapon != null)
			curWeapon.isFacingRight = controller.m_FacingRight;

		if(!isAI)
		{
			bool fire;
			bool pickup;

			fire = Input.GetButtonDown("Fire1");
			pickup = Input.GetButtonDown("Fire2");

			if (fire && curWeapon != null)
			{
				curWeapon.Attack();
			}

			if (pickup)
			{
				WeaponInteract();
			}
		}
	}

	void FixedUpdate()
	{
		// Move our character
		if(!isAI && canMove)
		{
			controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
			jump = false;
		}
	}

	public void WeaponInteract()
	{
		if (curWeapon != null)
		{
			if (!curWeapon.GetAttacking())
			{
				// Drop weapon
				curWeapon.ToggleEquipped();
				curWeapon.transform.parent = null;
				curWeapon = null;
			}
		}
		else
		{
			// Check for weapon pickup
			GameObject nearestWeapon = GetNearestWeapon();

			if (nearestWeapon != null)
			{
				// Pick up, equip, and active weapon
				curWeapon = nearestWeapon.GetComponent<WeaponController>();
				curWeapon.transform.parent = gameObject.transform;
				curWeapon.Equip();
				curWeapon.ToggleEquipped();
			}
		}
	}

	protected GameObject GetNearestWeapon() {

		GameObject nearestWeapon = null;

		// get all weapons in scene
		GameObject[] sceneWeapons = GameObject.FindGameObjectsWithTag("weapon");
		float minDist = float.MaxValue;

		foreach (GameObject weapon in sceneWeapons)
		{
			float dist = Vector2.Distance(weapon.transform.position, transform.position);
			if (dist <= pickupRange && dist < minDist && !weapon.GetComponent<WeaponController>().GetEquipped())
			{
				minDist = dist;
				nearestWeapon = weapon;
			}
		}

		return nearestWeapon; 
	}

	public void Knockback()
	{
		controller.animator.SetFloat("Knocking", 1);
		knockbackStartTime = Time.time;
	}

	public void SetDamage(float value)
	{
		curPercent = value;
	}

	public void SetMove(bool value)
    {
		canMove = value;
    }

}