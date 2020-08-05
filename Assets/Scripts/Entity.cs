using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidbody;

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void FlipSprite(bool active) {
        _spriteRenderer.flipX = !active;
    }

    public void Move(float acceleration, float maxSpeed, float positionX) {
        _rigidbody.velocity = new Vector3(acceleration * positionX, _rigidbody.velocity.y, 0);
        Mathf.Clamp(_rigidbody.velocity.x, 0, maxSpeed);
    }

    public virtual bool OnPoweredSteel() {
        return false;
    }

    public void RideMovingEntity(Entity entity) {
        transform.position = new Vector3(entity.transform.position.x, transform.position.y, transform.position.z);
    }
}
