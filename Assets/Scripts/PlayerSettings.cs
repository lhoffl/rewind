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
    public static float DoubleJumpForce = 750f;
    
    public static float DefaultFallForce = -5f;
    public static float DoubleFallForce = -35f;

    public static Color JumpColor = new Color(255f, 0f, 0f, 255f);
}
