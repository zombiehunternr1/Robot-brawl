using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private AnimatorController gameAnimations;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float smoothInputSpeed = .2f;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float punchDistance;
    [SerializeField]
    private float punchForce;
    private Rigidbody rB;
    private Vector2 inputDirection;
    private Vector2 smoothInputVelocity;
    private Vector3 moveDirection;
    private Animator anim;
    private bool canAttack = true;
    private BoxCollider punchCollider;

    private void OnEnable()
    {
        punchCollider = GetComponentInChildren<BoxCollider>();
        rB = GetComponent<Rigidbody>();
        rB.useGravity = true;
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = gameAnimations;
        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Game");       
    }

    //Updates the players position and rotation depending on the movement direction
    private void FixedUpdate()
    {
        if(moveDirection == Vector3.zero)
        {
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.fixedDeltaTime);
        rB.MovePosition(rB.position + moveDirection * movementSpeed * Time.fixedDeltaTime);
        rB.MoveRotation(targetRotation);
        anim.SetFloat("Speed", rB.velocity.magnitude);
    }

    //Turns on the box collider when the animation calls this event and does a forward raycast with a certain distance to see if the punch animation hit something
    //Once it hits another player it puches the other player away
    public void EnablePunch()
    {
        punchCollider.enabled = true;
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(punchCollider.transform.position, fwd, out hit, punchDistance))
        {
            Debug.Log(hit.collider.name);
            hit.collider.GetComponent<Rigidbody>().AddForce(transform.forward * punchForce, ForceMode.Impulse);
        }
    }

    //Turns off the box collider when the animation calls this event
    public void DisablePunch()
    {
        punchCollider.enabled = false;
    }

    //After the punching animation is done this function will reset the can attack boolean so the player can punch again
    public void ResetAttack()
    {
        canAttack = true;
    } 

    //Gets the players movement input and stores it into a Vector3
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 rawInput = context.ReadValue<Vector2>();
            inputDirection = Vector2.SmoothDamp(inputDirection, rawInput, ref smoothInputVelocity, smoothInputSpeed);
            moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
        }
    }

    //When the player wants to attack and is allowed to the can attack boolean will get set to false and the punching animation will get played
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack)
        {
            canAttack = false;
            anim.Play("Attack");
        }
    }
}
