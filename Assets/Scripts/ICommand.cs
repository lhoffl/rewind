using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand {

    void execute(Inputs input, Entity entity);
    void undo();
    bool WasOnPoweredSteel();
    void ModifySpeed();
}
