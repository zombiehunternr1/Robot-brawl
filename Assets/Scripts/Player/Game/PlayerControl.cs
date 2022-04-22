using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    private Rigidbody rB;
    private Vector2 inputDirection;
    private Vector3 moveDirection;

    private void OnEnable()
    {
        rB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.Translate(moveDirection * movementSpeed * Time.fixedDeltaTime);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
        moveDirection = new Vector3(inputDirection.x, rB.velocity.y, inputDirection.y);
    }
}
