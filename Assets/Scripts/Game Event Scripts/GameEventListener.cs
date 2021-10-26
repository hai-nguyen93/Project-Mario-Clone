using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with")]
    public GameEvent Event;

    [Tooltip("Response to invoke when Event is Raised")]
    public UnityEvent Response;

    public void OnEventRaised()
    {
        Response?.Invoke();
    }

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }
}
