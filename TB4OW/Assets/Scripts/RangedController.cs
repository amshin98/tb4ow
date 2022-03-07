using UnityEngine;

public class RangedController : WeaponController
{
    public static SpriteRenderer spriteRenderer;

    private const float X_OFFSET = 0f;
    private const float Y_OFFSET = -0.3f;

    public ProjectileBehavior projectile;
    public static float fireRate = 1f;

    public static Vector3 equipPos = new Vector3(0.192f, 0.002f, 0);

    RangedController() : base(fireRate, equipPos, spriteRenderer) { }

    public override void ToggleEquipped()
    {
        // Do nothing
    }

    override public void UseWeapon()
    {
        Debug.Log("here");
        // Shoot projectile
        Instantiate(projectile,
            transform.position + new Vector3(X_OFFSET, Y_OFFSET, 0), transform.rotation);
    }
}
