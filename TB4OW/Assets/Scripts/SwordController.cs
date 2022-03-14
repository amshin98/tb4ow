using UnityEngine;

public class SwordController : MeleeController
{
    public static SpriteRenderer spriteRenderer;

    public static float _swingSpeed = 0.2f;
    public static float _delay = 0.2f;

    public static Vector3 equipPos = new Vector3(0.9f, 0.8f, 0);

    public SwordController() : base(_swingSpeed, _delay, equipPos, spriteRenderer, "sword") {
        base.percentDamage = 15f;
        base.launchVector = (new Vector2(1, 1)).normalized;
    }
}
