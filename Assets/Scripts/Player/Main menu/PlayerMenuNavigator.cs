using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMenuNavigator : MonoBehaviour
{
    public bool isConfirmed { get; set; }
    public bool canInteract { get; set; }
    public int playerID;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float leaveSpeed;
    [SerializeField]
    private float smoothAnimTransitionTime;
    private PlayerInput playerJoinedRef;
    private Animator anim;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        canInteract = true;
        isConfirmed = false;
        StartCoroutine(MoveToOrigin());
    }

    public void GetPlayerInput(PlayerInput playerInput)
    {
        playerJoinedRef = playerInput;
    }

    public void ConfirmOption(InputAction.CallbackContext context)
    {
        if (context.performed && !isConfirmed && canInteract)
        {
            isConfirmed = true;
            anim.CrossFade("Ready", smoothAnimTransitionTime);
            PlayerJoinManager.changePlayerReadyStatus.Invoke(playerID, isConfirmed);
        }
        else if (context.performed && PlayerJoinManager.allPlayersReady && canInteract)
        {
            canInteract = false;
            PlayerJoinManager.startGameEvent.Invoke();
        }
    }

    public void ReturnOption(InputAction.CallbackContext context)
    {
        if (context.performed && isConfirmed && canInteract)
        {
            isConfirmed = false;
            anim.CrossFade("Unready", smoothAnimTransitionTime);
            PlayerJoinManager.changePlayerReadyStatus.Invoke(playerID, isConfirmed);
        }
        else if (context.performed && !isConfirmed)
        {
            StartCoroutine(LeaveFromOrigin(playerJoinedRef));
        }
    }

    private IEnumerator MoveToOrigin()
    {
        if (canInteract)
        {
            canInteract = false;
            while (transform.localPosition != new Vector3(0, 0, 0))
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * moveSpeed);
                yield return transform.localPosition;
            }
            transform.localPosition = new Vector3(0, 0, 0);
            canInteract = true;
            StopAllCoroutines();
        }
    }

    private IEnumerator LeaveFromOrigin(PlayerInput playerInput)
    {
        if (canInteract)
        {
            canInteract = false;
            AudioManager.instance.PlayLeaveEvent();
            while (transform.localPosition != new Vector3(0, -3.5f, 0))
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, -3.5f, 0), Time.deltaTime * moveSpeed * leaveSpeed);
                yield return transform.localPosition;
            }
            transform.localPosition = new Vector3(0, -3.5f, 0);
            PlayerJoinManager.leavePlayerEvent.Invoke(playerInput);
            Destroy(gameObject);
        }
    }
}
