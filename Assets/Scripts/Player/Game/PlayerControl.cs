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
    [SerializeField]
    private float punchDistance;
    [SerializeField]
    private float punchForce;
    private Rigidbody rB;
    private Vector2 inputDirection;
    private Vector3 moveDirection;
    private Animator anim;
    private bool canAttack = true;
    private BoxCollider punchCollider;

    private void OnEnable()
    {
        punchCollider = GetComponentInChildren<BoxCollider>();
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

    //Turns on the box collider when the animation calls this event and does a forward raycast with a certain distance to see if the punch animation hit something
    //Once it hits another player it puches the other player away
    public void EnablePunch()
    {
        RaycastHit hit;
        punchCollider.enabled = true;
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
        inputDirection = context.ReadValue<Vector2>();
        moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
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
