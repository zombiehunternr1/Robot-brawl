using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float animationSpeed;
    private Rigidbody rB;
    private Vector2 inputDirection;
    private Vector3 moveDirection;
    private Animator anim;

    private void OnEnable()
    {
        rB = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rB.MovePosition(rB.position + (moveDirection * movementSpeed) * Time.fixedDeltaTime);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
        moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
    }
}
