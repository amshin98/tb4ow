using UnityEngine;

public class RangedController : WeaponController
{
    public static SpriteRenderer spriteRenderer;

    private const float X_OFFSET_R = 0.5f;
    private const float X_OFFSET_L = -0.5f;
    private const float Y_OFFSET = 0.0f;

    public ProjectileBehavior projectile;
    public static float fireRate = 1f;

    public static Vector3 equipPos = new Vector3(1f, 0.002f, 0);

    RangedController() : base(fireRate, equipPos, spriteRenderer, "fish bucket") {
        base.percentDamage = 10f;
        base.launchVector = (new Vector2(1, 1)).normalized;
    }

    public override void ToggleEquipped()
    {
        // Do nothing
    }

    override public void UseWeapon()
    {
        // Shoot projectile
        var xOffset = transform.parent.rotation.y == 0 ? X_OFFSET_R : X_OFFSET_L;
        Quaternion rotation = Quaternion.Euler(0, isFacingRight ? 180 : 0, 0);
        ProjectileBehavior newFish = Instantiate(projectile,
                    transform.position + new Vector3(xOffset, Y_OFFSET, 0), rotation);
        newFish.isFacingRight = base.isFacingRight;
    }
}
