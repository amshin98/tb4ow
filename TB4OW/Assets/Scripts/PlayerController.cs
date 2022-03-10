using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
	[Header("Movement Parameters")]
	public CharacterController2D controller = null;
	public float movementSpeed = 40f;

	[Header("Weapon Interact Parameters")]
	public float pickupRange = 10f;

	[Header("Player Parameters")]
	public WeaponController curWeapon;
	public float curPercent = 0;
	public HealthBar healthBar;
	public bool isAI = false;

	[Header("Script References")]
	public ProjectileBehavior ProjectilePrefab;
    public string jumpSound;
    public string landSound;

	float horizontalMove = 0f;
	bool jump = false;
	bool attack = false;
	bool interact = false;
    

    private void Awake()
    {
		if (controller == null)
			controller = GetComponent<CharacterController2D>();
    }

    void Update()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * movementSpeed;

		jump = Input.GetButton("Jump");
        attack = Input.GetKey(KeyCode.J);
        interact = Input.GetKey(KeyCode.K);
		if(curWeapon != null)
			curWeapon.isFacingRight = controller.m_FacingRight;
	}

	void FixedUpdate()
	{
        // Move our character
		if(!isAI)
        {
			controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
			jump = false;

			if (attack && curWeapon != null)
			{
				curWeapon.Attack();
			}

			if (interact)
			{
				WeaponInteract();
			}
		}
    }

	public void WeaponInteract()
	{
		if (curWeapon != null)
		{
			// Drop weapon
			// curWeapon.transform.parent = null;
			// curWeapon = null;
		}
		else
		{
			// Check for weapon pickup
			LayerMask weaponLayerMask = LayerMask.GetMask("Weapons");

			Collider2D[] weaponsInRange = Physics2D.OverlapCircleAll(
			transform.position, pickupRange, weaponLayerMask);

			if (weaponsInRange.Length > 0)
			{


				// Pick up, equip, and active weapon
				curWeapon = weaponsInRange[0].gameObject.GetComponent<WeaponController>();
				curWeapon.transform.parent = gameObject.transform;
				curWeapon.Equip();
				curWeapon.ToggleEquipped();
			}
		}
	}

	public void Damage(float value)
	{
		curPercent += value;
		healthBar.setSliderHealth((int)curPercent);
	}
}