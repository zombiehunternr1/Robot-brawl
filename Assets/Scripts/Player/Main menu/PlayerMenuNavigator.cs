using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMenuNavigator : MonoBehaviour
{
    public bool isConfirmed { get; set; }
    public bool canInteract { get; set; }
    public int playerID;

    [SerializeField]
    private AnimatorController menuAnimations;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float leaveSpeed;
    [SerializeField]
    private float smoothAnimTransitionTime;
    private PlayerInput playerJoinedRef;
    private Animator anim;
    private Vector3 spawnPos;
    private PlayerControl playerControl;

    private void OnEnable()
    {
        playerControl = GetComponent<PlayerControl>();
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = menuAnimations;
        canInteract = true;
        isConfirmed = false;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(MoveToOrigin());
    }

    public void GetPlayerInput(PlayerInput playerInput)
    {
        playerJoinedRef = playerInput;
    }

    public void GetSpawnPos(Vector3 getSpawnPoint)
    {
        spawnPos = getSpawnPoint;
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
            PlayerJoinManager.loadMinigameEvent.Invoke();
        }
        else if (context.performed && PlayerJoinManager.allPlayersReady && !canInteract)
        {
            MiniGameManager.hidePanelEvent.Invoke();
            PlayerJoinManager.switchControlsEvent.Invoke();
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

    public void SwitchToMinigameScript()
    {
        playerControl.enabled = true;
        this.enabled = false;
    }

    private IEnumerator MoveToOrigin()
    {
        if (canInteract)
        {
            canInteract = false;
            while (transform.position != spawnPos)
            {
                transform.position = Vector3.Lerp(transform.position, spawnPos, Time.deltaTime * moveSpeed);
                yield return transform.position;
            }
            transform.position = spawnPos;
            canInteract = true;
            StopAllCoroutines();
        }
    }

    private IEnumerator LeaveFromOrigin(PlayerInput playerInput)
    {
        if (canInteract)
        {
            canInteract = false;
            PlayerJoinManager.leavePlayerEvent.Invoke(playerInput);
            while (transform.position != new Vector3(spawnPos.x, -3.5f, spawnPos.z))
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(spawnPos.x, -3.5f, spawnPos.y), Time.deltaTime * moveSpeed * leaveSpeed);
                yield return transform.position;
            }
            transform.position = new Vector3(spawnPos.x, -3.5f, spawnPos.z);
            Destroy(gameObject);
        }
    }
}
