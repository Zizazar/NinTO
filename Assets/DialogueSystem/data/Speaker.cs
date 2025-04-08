using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewSpeaker", menuName ="Dialogue/NewSpeaker")]
[System.Serializable]
public class Speaker : ScriptableObject
{
    public string speakerName;
    public Color nameColor;
    public Sprite sprite;

}
