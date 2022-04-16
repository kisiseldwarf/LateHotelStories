using System;
using System.Collections.Generic;
using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Scriptable_Objects.pnj
{
    [CreateAssetMenu(menuName = "Create Pnj", fileName = "Pnj", order = 0)]
    public class Pnj : ScriptableObject
    {
        public Image portrait = null;
        public TextAsset inkFile = null;
    }
}
