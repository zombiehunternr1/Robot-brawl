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
    private float blendAnimSpeed;
    [SerializeField]
    private float crossFadeAnimSpeed;
    [SerializeField]
    private float smoothInputSpeed = .2f;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float punchDistance;
    [SerializeField]
    private float punchForce;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    LayerMask groundMask;
    [SerializeField]
    float distanceToGround;
    private Rigidbody rB;
    private Vector2 inputDirection;
    private Vector2 smoothInputVelocity;
    private Vector3 moveDirection;
    private Animator anim;
    private bool canAttack = true;
    private SphereCollider punchCollider;

    private void OnEnable()
    {
        punchCollider = GetComponentInChildren<SphereCollider>();
        rB = GetComponent<Rigidbody>();
        rB.useGravity = true;
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = gameAnimations;
        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Game");       
    }
    //Enables or disables the gravity if the player is currently in the air or not
    private void Update()
    {
        if (IsGrounded())
        {
            rB.useGravity = false;
            anim.Play("Movement");
            anim.SetFloat("Speed", rB.velocity.magnitude, blendAnimSpeed, Time.deltaTime);
        }
        else
        {
            rB.useGravity = true;
            anim.SetFloat("Speed", 0);
            anim.CrossFade("Falling", crossFadeAnimSpeed);
        }
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
        rB.AddForce(moveDirection * movementSpeed, ForceMode.Impulse);
        //rB.MovePosition(rB.position + moveDirection * movementSpeed * Time.fixedDeltaTime);
        rB.MoveRotation(targetRotation);
    }

    //Turns on the sphere collider when the animation calls this event
    public void EnablePunch()
    {
        punchCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.GetComponent<PlayerControl>() == null)
        //{
        //    return;           
        //}
        //if (other.GetComponent<PlayerControl>() != this)
        //{
        //    Vector3 targetHitDirection = new Vector3();
        //    targetHitDirection = other.transform.position - punchCollider.transform.position.normalized;
        //    other.GetComponent<Rigidbody>().AddForce(targetHitDirection * punchForce, ForceMode.Impulse);
        //}
    }

    //Turns off the sphere collider when the animation calls this event
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
        if (context.canceled)
        {
            inputDirection = Vector3.zero;
            moveDirection = inputDirection;
            rB.velocity = Vector3.zero;
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
    //This checks if the player is currently on the ground or not
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, distanceToGround, groundMask);
    }
}
