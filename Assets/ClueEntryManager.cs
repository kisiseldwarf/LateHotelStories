using System;
using System.Linq;
using Scriptable_Objects.clues;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClueEntryManager : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public Clue clue;
    public GameObject titleGo;
    public GameObject descGo;

    private LineRenderer lr;
    private Canvas canvas;
    
    private DialogClueManager _dialogClueManager;
    private ClueInventoryManager clueInventoryManager;
    private bool writing = false;
    private Text title;
    private Text desc;

    public void OnBeginDrag(PointerEventData eventData)
    {
        lr.positionCount = 2;
        Vector3 anchoredPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform.GetComponent<RectTransform>(), eventData.position, Camera.main, out anchoredPos);
        lr.SetPosition(0, anchoredPos);
        Debug.Log("begin drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 anchoredPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform.GetComponent<RectTransform>(), eventData.position, Camera.main, out anchoredPos);
        lr.SetPosition(1, anchoredPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");
        lr.positionCount = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("on pointer down");
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 1;
        canvas = GetComponentInParent<Canvas>();

        _dialogClueManager = GetComponentInParent<DialogClueManager>();
        clueInventoryManager = GetComponentInParent<ClueInventoryManager>();
        title = titleGo.GetComponent<Text>();
        desc = descGo.GetComponent<Text>();
        title.text = clue.name;
        desc.text = clue.desc;
    }

    // Update is called once per frame
    void Update()
    {
        if(writing)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                _dialogClueManager.NextLine();
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var otherClueGO = eventData.pointerDrag;
        if (!otherClueGO.Equals(gameObject))
        {
            var otherClue = otherClueGO.GetComponent<ClueEntryManager>().clue;
            var linkedHashs = clue.linkedCluesHash.Union(otherClue.linkedCluesHash).ToList();
            var conclusionEntry = linkedHashs.Find(el => el == clue.hash);
            
            if(conclusionEntry == null)
            {
                Debug.Log("no link between those two clues");
            } else
            {
                _dialogClueManager.LaunchDialog(EntryCallback, clue.textOnLinked);
                writing = true;
            }
        }
    }

    void EntryCallback()
    {
        foreach (var foundClue in clue.cluesOnFound)
        {
            clueInventoryManager.AddClueToInventory(foundClue);
        }
        writing = false;
    }
}
