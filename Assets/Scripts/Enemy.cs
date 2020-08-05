using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {
    
    [SerializeField]
    Vector3 _pointA, _pointB;

    Vector3 _currentPoint;

    private Stack<ICommand> _commands;

    void Start() {
        _commands = new Stack<ICommand>();
        _currentPoint = _pointA;
        transform.position = _currentPoint;
    }

    void Update() {
        CalculateDirectionToNextPoint();
    }

    private void CalculateDirectionToNextPoint() {
        float distance = Vector3.Distance(transform.position, _currentPoint);
        if(distance <= 0.01) {
            transform.position = _currentPoint;
            SwapPoints();
        }
        else {
            Vector3 normalized = Vector3.Normalize(_currentPoint - transform.position);
            Inputs input = new Inputs(normalized, false, false);
            Move(input);
        }
    }

    private void SwapPoints() {
        if(_currentPoint.Equals(_pointA)) {
            _currentPoint = _pointB;
        }
        else {
            _currentPoint = _pointA;
        }
    }

    private void Move(Inputs inputs) {
        MoveCommand moveCommand = new MoveCommand(PlayerSettings.DefaultAccelerationFactor/2, PlayerSettings.DefaultMaxSpeed/2, transform.position.y);
        moveCommand.execute(inputs, this);
        _commands.Push(moveCommand);
    }

    public void Undo() {
        ICommand command = _commands.Pop();
        command.undo();
    }

    public bool UndoComplete() {
        return _commands.Count <= 0;
    }
}
