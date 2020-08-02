using System.Collections;
using System.Collections.Generic;

public static class PlayerState {

    public static DefaultPlayerState DefaultPlayerState = new DefaultPlayerState();
    public static JumpingPlayerState JumpingPlayerState = new JumpingPlayerState();
    public static FallingPlayerState FallingPlayerState = new FallingPlayerState();
}
