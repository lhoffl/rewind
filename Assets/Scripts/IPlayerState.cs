using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public interface IPlayerState { 
    void HandleInput(Inputs inputs);
    void HandleCollision(Collision2D other);
    void Enter(Player player);
    void Exit();
    void Update();
    Stack<Inputs> GetInputs();
}
