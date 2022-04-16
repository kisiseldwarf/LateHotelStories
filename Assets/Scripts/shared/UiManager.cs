using System.Collections;
using dialogManagers;
using Scriptable_Objects.clues;
using TMPro;
using UnityEngine;

namespace player
{
    public class UiManager : MonoBehaviour
    {
        public static event events.Event WriteFinished;

        public GameObject clueInventory;
        public GameObject textPanel;
        public GameObject pickableInteraction;
        public GameObject textGameObject;
        
        private Animator cluePanelAnimator;
        private Animator clueInventoryAnimator;
        private Animator pickableAnimator;
        
        private TMP_Text textComponent;
        
        private bool clueInventoryShowed = false;
        private bool writing;
        
        private static readonly int IsOpen = Animator.StringToHash("isOpen");
        private static readonly int Show = Animator.StringToHash("show");

        private void Start()
        {
            pickableAnimator = pickableInteraction.GetComponent<Animator>();
            cluePanelAnimator = textPanel.GetComponent<Animator>();
            clueInventoryAnimator = clueInventory.GetComponent<Animator>();
            textComponent = textGameObject.GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            PlayerInput.ObjectFound += ShowPickableTooltip;
            PlayerInput.ObjectLost += HidePickableTooltip;
            PlayerInput.InventoryButtonPressed += SwitchClueInventory;
            
            ClueManager.IsPicked += HidePickableTooltip;
            
            DialogClueManager.BeginDialog += ShowCluePanel;
            DialogClueManager.EndDialog += HideCluePanel;
            DialogClueManager.Next += WriteOnCluePanel;
            DialogClueManager.FinishCurrentSentence += finishCurrentSentence;

            DialogPnjManager.Next += WriteOnDialogPanelPnj;
            DialogPnjManager.FinishCurrentSentence += FinishCurrentSentencePnj;
        }
        
        private void OnDisable()
        {
            PlayerInput.ObjectFound -= ShowPickableTooltip;
            PlayerInput.ObjectLost -= HidePickableTooltip;
            PlayerInput.InventoryButtonPressed -= SwitchClueInventory;
            
            ClueManager.IsPicked -= HidePickableTooltip;
            
            DialogClueManager.BeginDialog -= ShowCluePanel;
            DialogClueManager.EndDialog -= HideCluePanel;
            DialogClueManager.Next -= WriteOnCluePanel;
            DialogClueManager.FinishCurrentSentence -= finishCurrentSentence;
            
            DialogPnjManager.Next -= WriteOnDialogPanelPnj;
            DialogPnjManager.FinishCurrentSentence -= FinishCurrentSentencePnj;
        }

        private void FinishCurrentSentencePnj()
        {
            throw new System.NotImplementedException();
        }

        private void WriteOnDialogPanelPnj(string towrite)
        {
            throw new System.NotImplementedException();
        }

        private void SwitchClueInventory(Clue[] clue)
        {
            clueInventoryAnimator.SetBool(IsOpen, !clueInventoryAnimator.GetBool(IsOpen));
        }

        private void WriteOnCluePanel(string toWrite)
        {
            StartCoroutine(write(toWrite));
        }

        private void HideCluePanel()
        {
            cluePanelAnimator.SetBool(IsOpen, false);
        }

        private void ShowCluePanel()
        {
            cluePanelAnimator.SetBool(IsOpen, true);
        }

        private void HidePickableTooltip()
        {
            pickableAnimator.SetBool(Show, false);
        }

        private void ShowPickableTooltip(Vector2 location)
        {
            pickableAnimator.SetBool(Show, true);
            float uiHeightDistance = 0.7f;
            location.Set(location.x, location.y + uiHeightDistance);
            pickableInteraction.transform.position = location;
        }
        
        private IEnumerator write(string sentence)
        {
            textComponent.text = sentence;
            textComponent.maxVisibleCharacters = 0;

            do
            {
                textComponent.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.1f);
            } while (textComponent.maxVisibleCharacters != textComponent.text.Length);

            WriteFinished?.Invoke(); 
        }
        
        void finishCurrentSentence()
        {
            textComponent.maxVisibleCharacters = textComponent.textInfo.characterCount;
            StopAllCoroutines();
        }
    }
}