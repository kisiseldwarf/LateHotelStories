using System.Collections.Generic;
using Scriptable_Objects.clues;
using UnityEngine;

public class ClueInventoryManager : MonoBehaviour
{
    public List<Clue> clues = new List<Clue>();
    public GameObject clueEntry;
    private bool clueInventoryShowed = false;

    public void AddClueToInventory(Clue clue)
    {
        clues.Add(clue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
