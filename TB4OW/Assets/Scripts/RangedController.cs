using UnityEngine;

public class RangedController : WeaponController
{
    private const float PROJ_OFFSET = -0.3f;

    public ProjectileBehavior projectile;
    public static float fireRate = 1f;

    RangedController() : base(fireRate) { }

    override public void UseWeapon()
    {
        // Shoot projectile
        Instantiate(projectile,
            transform.position + new Vector3(0, PROJ_OFFSET, 0), transform.rotation);
    }
}
