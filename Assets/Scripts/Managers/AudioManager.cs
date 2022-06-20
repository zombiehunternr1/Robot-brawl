using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private EventReference joinEvent;
    [SerializeField]
    private EventReference leaveEvent;
    [SerializeField]
    private EventReference switchColorEvent;
    [SerializeField]
    private EventReference playerReadyEvent;
    [SerializeField]
    private EventReference playerUnreadyEvent;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayJoinEvent()
    {
        RuntimeManager.PlayOneShot(joinEvent);
    }

    public void PlayLeaveEvent()
    {
        RuntimeManager.PlayOneShot(leaveEvent);
    }

    public void PlaySwitchColorEvent()
    {
        RuntimeManager.PlayOneShot(switchColorEvent);
    }

    public void PlayReadyEvent()
    {
        RuntimeManager.PlayOneShot(playerReadyEvent);
    }

    public void PlayerUnreadyEvent()
    {
        RuntimeManager.PlayOneShot(playerUnreadyEvent);
    }
}
