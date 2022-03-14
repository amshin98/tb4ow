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

	[Header("Script References")]
	public ProjectileBehavior ProjectilePrefab;
	public Transform aiTargetPos;

	float horizontalMove = 0f;
	bool jump = false;

	private float knockbackStartTime = 0;
	private float knockbackDuration = 0.5f;
	

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
			if (Input.GetButtonDown("Fire1") && curWeapon != null)
			{
				curWeapon.Attack();
			}

			if (Input.GetButtonDown("Fire2"))
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

	public void Damage(float value)
	{
		curPercent += value;
		//healthBar.setSliderHealth((int)curPercent);
	}
}