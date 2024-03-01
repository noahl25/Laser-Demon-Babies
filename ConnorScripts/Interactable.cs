using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;
    [SerializeField]
    public string promptMessage;
    public virtual string OnLook()
    {
        return promptMessage;
    }
    public void baseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().onInteract.Invoke();
        Interact();
    }
    protected virtual void Interact()
    {
        //will be overridden by subclasses
    }
}
