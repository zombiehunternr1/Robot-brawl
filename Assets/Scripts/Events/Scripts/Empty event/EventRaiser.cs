using UnityEngine;

public class EventRaiser : MonoBehaviour
{
    [SerializeField]
    private GameEventEmpty _gameEvent;

    public void RaiseEvent()
    {
        _gameEvent.Raise();
    }
}
