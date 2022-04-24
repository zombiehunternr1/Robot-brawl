using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float rotationSpeed;
    private Rigidbody rB;
    private Vector2 inputDirection;
    private Vector3 moveDirection;
    private Animator anim;
    private bool canAttack = true;

    private void OnEnable()
    {
        rB = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    //Updates the players position and rotation depending on the movement direction
    private void FixedUpdate()
    {
        rB.MovePosition(transform.position + (moveDirection * movementSpeed) * Time.fixedDeltaTime);
        anim.SetFloat("Speed", rB.velocity.magnitude);
        if(moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rB.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    //Gets the players movement input and stores it into a Vector3
    public void OnMovement(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
        moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    public void ResetAttack()
    {
        canAttack = true;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack)
        {
            canAttack = false;
            anim.Play("Attack");
        }
    }
}
