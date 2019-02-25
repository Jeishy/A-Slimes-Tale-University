using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject {
    // Editable scriptable objects for dialogue sessions
    public string characterName;
    [TextArea(2, 10)]
    public string[] sentences;
}
