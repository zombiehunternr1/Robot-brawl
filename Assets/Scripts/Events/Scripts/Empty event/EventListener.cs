using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField]
    private GameEventEmpty _gameEvent;
    [SerializeField]
    private UnityEvent _respondse;

    private void OnEnable()
    {
        _gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        _gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        _respondse.Invoke();
    }
}
