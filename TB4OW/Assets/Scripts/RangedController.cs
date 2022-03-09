using UnityEngine;

public class RangedController : WeaponController
{
    private const float X_OFFSET_R = 0.5f;
    private const float X_OFFSET_L = -0.5f;
    private const float Y_OFFSET = 0.0f;

    public ProjectileBehavior projectile;
    public static float fireRate = 1f;

    public static Vector3 equipPos = new Vector3(1f, 0.002f, 0);

    RangedController() : base(fireRate, equipPos) { }

    public override void ToggleEquipped()
    {
        // Do nothing
    }

    override public void UseWeapon()
    {
        // Shoot projectile
        //TODO FIX THE FISH DIRECTION
        var xOffset = transform.parent.rotation.y == 0 ? X_OFFSET_R : X_OFFSET_L;
        Instantiate(projectile,
            transform.position + new Vector3(xOffset, Y_OFFSET, 0), transform.rotation);
    }
}
