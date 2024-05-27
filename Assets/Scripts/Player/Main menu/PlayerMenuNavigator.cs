using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMenuNavigator : MonoBehaviour
{
    public bool isConfirmed { get; set; }
    public bool canInteract { get; set; }
    public bool allowInput { get; set; }
    public int playerID;

    [SerializeField]
    private GameEventEmpty disAllowInputEvent;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float leaveSpeed;
    [SerializeField]
    private float smoothAnimTransitionTime;
    private PlayerInput playerJoinedRef;
    [SerializeField]
    private Animator animMenu;
    private Vector3 spawnPos;
    private PlayerControl playerControl;
    private CharacterSkinController characterSkin;

    [SerializeField]
    private GameEventCheckReady checkReadyEvent;
    [SerializeField]
    private GameEventLeave leavePlayerEvent;
    [SerializeField]
    private GameEventEmpty switchSceneEvent;
    [SerializeField]
    private GameEventEmpty startGameEvent;

    private void OnEnable()
    {
        allowInput = true;
        characterSkin = GetComponent<CharacterSkinController>();
        playerControl = GetComponent<PlayerControl>();
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

    public void NextSkinType(InputAction.CallbackContext context)
    {
        if (context.performed && !isConfirmed && canInteract && allowInput)
        {
            characterSkin.NextSkinType();
        }
    }

    public void PrevSkinType(InputAction.CallbackContext context)
    {
        if (context.performed && !isConfirmed && canInteract && allowInput)
        {
            characterSkin.PrevSkinType();
        }
    }

    public void ConfirmOption(InputAction.CallbackContext context)
    {
        if (context.performed && !isConfirmed && allowInput)
        {
            isConfirmed = true;
            animMenu.CrossFade("Ready", smoothAnimTransitionTime);
            checkReadyEvent.RaiseCheckReady(playerID, isConfirmed);
        }
        else if (context.performed && PlayerJoinManager.allPlayersReady && canInteract && allowInput)
        {
            canInteract = false;
            disAllowInputEvent.Raise();
            switchSceneEvent.Raise();
        }
        else if (context.performed && PlayerJoinManager.allPlayersReady && !canInteract && allowInput)
        {
            startGameEvent.Raise();
        }
    }

    public void ReturnOption(InputAction.CallbackContext context)
    {
        if (context.performed && isConfirmed && allowInput)
        {
            isConfirmed = false;
            animMenu.CrossFade("Unready", smoothAnimTransitionTime);
            checkReadyEvent.RaiseCheckReady(playerID, isConfirmed);
        }
        else if (context.performed && !isConfirmed && canInteract)
        {
            StartCoroutine(LeaveFromOrigin(playerJoinedRef));
        }
    }

    public void SwitchToMinigameScript()
    {
        playerControl.enabled = true;
        playerControl.playerID = playerID;
        playerControl.AllowInput();
    }

    private IEnumerator MoveToOrigin()
    {
        if (canInteract)
        {
            canInteract = false;
            while (transform.position != spawnPos)
            {
                transform.position = Vector3.Lerp(transform.position, spawnPos, Time.deltaTime * moveSpeed);
                yield return null;
            }
            transform.position = spawnPos;
            canInteract = true;
        }
    }

    private IEnumerator LeaveFromOrigin(PlayerInput playerInput)
    {
        if (canInteract)
        {
            canInteract = false;
            leavePlayerEvent.RaiseLeave(playerInput);
            while (transform.position != new Vector3(spawnPos.x, -3.5f, spawnPos.z))
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(spawnPos.x, -3.5f, spawnPos.y), Time.deltaTime * moveSpeed * leaveSpeed);
                yield return null;
            }
            transform.position = new Vector3(spawnPos.x, -3.5f, spawnPos.z);
            Destroy(gameObject);
        }
    }
}
