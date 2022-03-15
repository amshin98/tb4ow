using System.Collections;
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




}