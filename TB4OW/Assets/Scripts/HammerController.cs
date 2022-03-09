using UnityEngine;

public class HammerController : MeleeController
{
    public static float _swingSpeed = 0.4f;
    public static float _delay = 0.2f;

    public static Vector3 equipPos = new Vector3(0.8f, 0.8f, 0);

    public HammerController() : base(_swingSpeed, _delay, equipPos) { }
}
