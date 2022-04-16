using Ink.Runtime;
using Scriptable_Objects.pnj;
using UnityEngine;

public class PnjManager : MonoBehaviour
{
    
    public Pnj pnj;
    private Story story;
    private bool dialogLaunched;
    
    public static event events.EventInkDialog StartDialog;
    
    // Start is called before the first frame update
    void Start()
    {
        story = new Story(pnj.inkFile.text);
    }

    public void Interact()
    {
        story.Continue();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogLaunched)
        {
            StartDialog?.Invoke(story, ((IDialogManagerHandler) this).onDialogEnd);
            dialogLaunched = true;
        }  
    }
}
