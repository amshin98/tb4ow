
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // With 4.2 gravity scale
    public float MovementSpeed = 12;
    public float jumpForce = 14; 
    public ProjectileBehavior ProjectilePrefab;
    public Transform launchPoint;

    private Rigidbody2D _rigidbody;

    // Weapon
    public WeaponController curWeapon;
    public float pickupRange = 10f;
    private Collider2D _collider;

    public bool isAI = false;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isAI)
        {
            var playerInput = Input.GetAxisRaw("Horizontal");

            // Movement
            MovePlayer(playerInput, Input.GetButtonDown("Jump"));

            // Attacking
            if (Input.GetButtonDown("Fire1") && curWeapon != null)
            {
                curWeapon.Attack();
            }

            // TODO: pickup
            if (Input.GetButtonDown("Fire2"))
            {
                WeaponInteract();
            }
        }
    }

    public void MovePlayer(float playerInput, bool jump)
    {
        transform.position += MovementSpeed * Time.deltaTime * new Vector3(playerInput, 0, 0);

        if (!Mathf.Approximately(0, playerInput))
        {
            transform.rotation = playerInput < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }
        if (jump && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void WeaponInteract()
    {
        if (curWeapon != null)
        {
            // Drop weapon
            curWeapon.transform.parent = null;
            curWeapon = null;
            Debug.Log("dropp");
        }
        else
        {
            // Check for weapon pickup
            LayerMask weaponLayerMask = LayerMask.GetMask("Weapons");

            Collider2D[] weaponsInRange = Physics2D.OverlapCircleAll(
                transform.position, pickupRange, weaponLayerMask);

            if (weaponsInRange.Length > 0)
            {

                // Find closest weapon
                int minDistIdx = 0;
                float minDist = _collider.Distance(weaponsInRange[0]).distance;

                for (int i = 1; i < weaponsInRange.Length; i++)
                {
                    float curDist = _collider.Distance(weaponsInRange[i]).distance;
                    if (curDist < minDist)
                    {
                        minDistIdx = i;
                        minDist = curDist;
                    }
                }

                // Pick up, equip, and active weapon
                curWeapon = weaponsInRange[minDistIdx].gameObject.GetComponent<WeaponController>();
                curWeapon.transform.parent = gameObject.transform;
                curWeapon.Equip();
                curWeapon.ToggleEquipped();
            }
        }
    }
}
