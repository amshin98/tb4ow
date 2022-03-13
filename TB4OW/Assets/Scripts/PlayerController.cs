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
	public float pickupRange = .1f;

	[Header("Player Parameters")]
	public WeaponController curWeapon;
	public float curPercent = 0;
	public HealthBar healthBar;
	public bool isAI = false;

	[Header("Script References")]
	public ProjectileBehavior ProjectilePrefab;
    public string jumpSound;
    public string landSound;
	public Transform aiTargetPos;

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

        // iterate through each one
        foreach (GameObject weapon in sceneWeapons)
        {
			Debug.Log(Vector2.Distance(weapon.transform.position, transform.position));
			Debug.Log(pickupRange);

            // calculate the distance to it
            if(Vector2.Distance(weapon.transform.position, transform.position) < pickupRange && Vector2.Distance(weapon.transform.position, transform.position) < minDist)
            {
				nearestWeapon = weapon;
            }
        }

        return nearestWeapon; 
    }

	public void Damage(float value)
	{
		curPercent += value;
		healthBar.setSliderHealth((int)curPercent);
	}
}