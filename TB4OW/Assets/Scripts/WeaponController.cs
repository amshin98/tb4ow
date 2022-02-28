using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    private float _fireRate;

    private float _nextFire;

    public WeaponController(float fireRate)
    {
        _fireRate = fireRate;
        _nextFire = 0.0f;
    }

    public float GetFireRate()
    {
        return _fireRate;
    }

    // Handles dealing damage/knockback. For now, destroy targets but in
    // future change to knock back player
    void OnTriggerEnter2D(Collider2D collider)
    {
        string curTag = collider.gameObject.tag;
        if (curTag == "destructable")
        {
            Destroy(collider.gameObject);
        }
        else if (curTag == "player")
        {
            Debug.Log("KNOCKBACK");
        }
    }

    // Handle attacking
    public void Attack()
    {
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            UseWeapon();
        }
    }


    // Handles moving the weapon for attacking
    public abstract void UseWeapon();
}
