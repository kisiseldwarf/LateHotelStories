using System;
using System.Collections.Generic;
using dialogManagers;
using events;
using Ink.Runtime;
using player;
using UnityEngine;
using Event = events.Event;

/// <summary>
/// Handy class to manipulate dialog UI
/// </summary>
public class DialogClueManager : DialogManager
{
    // events
    public static event Event BeginDialog;
    public static event Event EndDialog;
    public static event Event FinishCurrentSentence;
    public static event EventWrite Next;
    
    private string[] sentences;
    private Action callback;
    private Queue<string> sentencesToWrite = new Queue<string>();
    
    private bool writeSentenceCoroutineRunning;
    public bool dialogIsRunning;
    private Story story;

    public DialogClueManager()
    {
        dialogIsRunning = false;
        UiManager.WriteFinished += OnWriteFinished;
    }

    private void OnWriteFinished()
    {
        writeSentenceCoroutineRunning = false;
    }

    public void LaunchDialog(Action callback, string[] sentences)
    {
        this.sentences = sentences;
        this.callback = callback;
        reloadQueue();
        beginDialog();
        writeSentenceCoroutineRunning = true;
        Next?.Invoke(sentencesToWrite.Dequeue());
        dialogIsRunning = true;
    }

    public void NextLine()
    {
        if (writeSentenceCoroutineRunning)
        {
            writeSentenceCoroutineRunning = false;
            FinishCurrentSentence?.Invoke();
        }
        else
        {
            if (sentencesToWrite.Count == 0)
            {
                quitDialog();
            }
            else
            {
                writeSentenceCoroutineRunning = true;
                Next?.Invoke(sentencesToWrite.Dequeue());
            }
        }
    }

    void beginDialog()
    {
        BeginDialog?.Invoke();
        GameObject.FindGameObjectWithTag("Player").SendMessage("setMove", false);
    }

    void quitDialog()
    {
        dialogIsRunning = false;
        EndDialog?.Invoke();
        GameObject.FindGameObjectWithTag("Player").SendMessage("setMove", true);
        callback();
    }

    void reloadQueue()
    {
        foreach (var sentence in sentences)
        {
            sentencesToWrite.Enqueue(sentence);
        }
    }
}
