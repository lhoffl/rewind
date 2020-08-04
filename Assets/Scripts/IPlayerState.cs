using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public interface IPlayerState { 
    void HandleInput(Inputs inputs);
    void HandleCollision(Collider2D other);
    void Enter(Player player);
    void Exit();
    void Update();
    bool UndoComplete();
    void Undo();
}