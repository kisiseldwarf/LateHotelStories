using System;
using Ink.Runtime;
using Scriptable_Objects.clues;
using Vector2 = UnityEngine.Vector2;

namespace events
{
    public delegate void Event();
    public delegate void EventLocation(Vector2 location);
    public delegate void EventWrite(string toWrite);
    public delegate void EventClueArray(Clue[] clues);
    public delegate void EventDialog(string[] sentences, Action endCallback);
    public delegate void EventInkDialog(Story story, Action endCallback);
}