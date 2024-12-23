using UnityEngine.Events;

public class WindEvy : Interactable
{
    public UnityEvent EndLevel;

    public override void Interact()
    {
        EndLevel.Invoke();
    }
}