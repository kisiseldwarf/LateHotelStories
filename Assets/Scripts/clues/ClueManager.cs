using System;
using events;
using Scriptable_Objects.clues;
using UnityEngine;

public class ClueManager : MonoBehaviour, IDialogManagerHandler
{
    // events
    public static event events.Event IsPicked;
    public static event EventDialog StartDialog;

    public Clue clue;
    
    private bool canPick = false;
    private bool dialogIsRunning = false;
    private bool dialogLaunched;

    public void Interact()
    {
        if (!dialogLaunched)
        {
            IsPicked?.Invoke();
            StartDialog?.Invoke(clue.textOnFound, ((IDialogManagerHandler) this).onDialogEnd);
            dialogLaunched = true;
        }    
    }

    void IDialogManagerHandler.onDialogEnd()
    {
        GameObject.FindGameObjectWithTag("Player").SendMessage("AddClueToInventory", clue);
        Destroy(gameObject);
    }
}