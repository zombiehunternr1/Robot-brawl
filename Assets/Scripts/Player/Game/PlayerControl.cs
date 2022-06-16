using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public int playerID { get; set; }
    [SerializeField]
    private AnimatorController gameAnimations;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float crossFadeAnimSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float punchForce;
    [SerializeField]
    private float dizzynessCooldown;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    LayerMask groundMask;
    [SerializeField]
    float distanceToGround;
    private Rigidbody rB;
    private Vector3 moveDirection;
    private Animator anim;
    private bool canAttack = true;
    private bool allowInput;
    private SphereCollider punchCollider;
    private CharacterSkinController playerSkin;

    private void OnEnable()
    {
        playerSkin = GetComponent<CharacterSkinController>();
        punchCollider = GetComponentInChildren<SphereCollider>();
        PlayerInput playerInput = GetComponent<PlayerInput>();
        rB = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        punchCollider.enabled = false;
        rB.useGravity = true;
        anim.runtimeAnimatorController = gameAnimations;
        playerInput.SwitchCurrentActionMap("Game");
    }

    //Enables or disables the gravity if the player is currently in the air or not
    private void Update()
    {
        if (IsGrounded())
        {
            rB.useGravity = false;
            anim.SetFloat("Speed", moveDirection.magnitude);
        }
        else
        {
            rB.useGravity = true;
            anim.SetFloat("Speed", 0);
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
        rB.MovePosition(rB.position + moveDirection * movementSpeed * Time.fixedDeltaTime);
        rB.MoveRotation(targetRotation);
    }

    //Turns on the sphere collider when the animation calls this event
    public void EnablePunch()
    {
        punchCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControl>() == null)
        {
            return;
        }
        if (other.GetComponent<PlayerControl>() != this)
        {
            Vector3 targetHitDirection = new Vector3();
            targetHitDirection = (other.transform.position - transform.position).normalized;
            other.GetComponent<Rigidbody>().AddForce(targetHitDirection * punchForce, ForceMode.Impulse);
            other.GetComponent<Animator>().Play("Pushed");
        }
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

    public void AllowInput()
    {
        allowInput = true;
    }

    public void DisallowInput()
    {
        allowInput = false;
    }

    public void Defeated()
    {
        allowInput = false;
        rB.useGravity = false;
        anim.Play("Falling");
        MiniGameManager.checkMinigameFinishedEvent.Invoke(playerID);
    }

    //Once this function gets called the coroutine to start the dizzyness cooldown
    public void StartDizzynessEffect()
    {
        StartCoroutine(DizzynessCooldown());
    }

    //Gets the players movement input and stores it into a Vector3
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded() && allowInput)
        {
            Vector2 rawInput = context.ReadValue<Vector2>();
            moveDirection = new Vector3(rawInput.x, 0, rawInput.y);
        }
        if (context.canceled)
        {
            moveDirection = Vector3.zero;
        }
    }

    //When the player wants to attack they must first be allowed to
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack && IsGrounded() && allowInput)
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

    //Countdown for how long the player stays in dizzyness mode before being able to move again.
    private IEnumerator DizzynessCooldown()
    {
        float currentTime = 0;
        bool isDizzy = true;
        allowInput = false;
        playerSkin.StunnedExpression();
        anim.Play("Dizzy");
        while(isDizzy)
        {
            currentTime += Time.deltaTime;
            if(currentTime > dizzynessCooldown)
            {
                isDizzy = false;
                currentTime = 0;
            }
            yield return null;
        }
        allowInput = true;
        playerSkin.NormalExpression();
        anim.CrossFade("Game idle", crossFadeAnimSpeed);
    }
}
