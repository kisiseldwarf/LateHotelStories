using events;
using player;

namespace dialogManagers
{
    public abstract class DialogManager
    {
        public static EventWrite Next;
        public static event Event BeginDialog;
        public static event Event EndDialog;
        public static event Event FinishCurrentSentence;
        
        protected bool writeSentenceCoroutineRunning;
        protected bool dialogIsRunning;

        public DialogManager()
        {
            dialogIsRunning = false;
            UiManager.WriteFinished += OnWriteFinished;
        }
        
        private void OnWriteFinished()
        {
            writeSentenceCoroutineRunning = false;
        }

    }
}