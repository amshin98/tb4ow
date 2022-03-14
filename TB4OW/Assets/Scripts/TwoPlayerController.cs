﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController2D))]
public class TwoPlayerController : PlayerController
{
	public KeyCode leftMoveKey;
	public KeyCode rightMoveKey;
	public KeyCode jumpKey;
	public KeyCode attackKey;
	public KeyCode pickupKey;
	

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


		horizontalMove = 0;

		if(Input.GetKey(rightMoveKey))
			horizontalMove += 1;
		if(Input.GetKey(leftMoveKey))
			horizontalMove -= 1;
		jump = Input.GetKey(jumpKey);

		horizontalMove *= movementSpeed;

		if(curWeapon != null)
			curWeapon.isFacingRight = controller.m_FacingRight;

		if(!isAI)
		{
			bool fire;
			bool pickup;
			fire = Input.GetKeyDown(attackKey);
			pickup = Input.GetKeyDown(pickupKey);

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
		if(!isAI)
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

	private GameObject GetNearestWeapon() {

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


}