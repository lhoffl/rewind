using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSettings {

    public static float DefaultSpeed = 125f;
    public static float DefaultMaxSpeed = 250f;
    public static float FallingMaxSpeed = 500f;
    
    public static float DefaultAccelerationFactor = 5f;
    public static float JumpingAccelerationFactor = 8f;
    
    public static float JumpForce = 500f;
    public static float DoubleJumpForce = 1000f;
    
    public static float DefaultFallForce = -5f;
    public static float DoubleFallForce = -35f;

    public static float DefaultFallAcceleration = -1.5f;

    public static int PoweredSteelMultipler = 3;

    public static int MaxHealth = 3;
    public static int HealthPerHeart = 4;

    public static Color JumpColor = new Color(172f, 60f, 255f, 255f);
}
