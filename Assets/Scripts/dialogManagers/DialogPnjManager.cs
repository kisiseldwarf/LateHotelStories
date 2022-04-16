using System;
using events;
using Ink.Runtime;
using player;
using UnityEngine;
using Event = events.Event;

namespace dialogManagers
{
    public class DialogPnjManager : DialogManager
    {
        private Story story;
        private Action callback;

        public void LaunchDialog(Action callback, Story story)
        {
            this.story = story;
            this.callback = callback;
            
            beginDialog();

            writeSentenceCoroutineRunning = true;
            
            Next?.Invoke(story.Continue());
            
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
                if (!story.canContinue)
                {
                    quitDialog();
                }
                else
                {
                    writeSentenceCoroutineRunning = true;
                    Next?.Invoke(story.Continue());
                }
            }
        }
        void quitDialog()
        {
            dialogIsRunning = false;
            EndDialog?.Invoke();
            GameObject.FindGameObjectWithTag("Player").SendMessage("setMove", true);
            callback();
        }
        
        void beginDialog()
        {
            BeginDialog?.Invoke();
            GameObject.FindGameObjectWithTag("Player").SendMessage("setMove", false);
        }
    }
}