
using UnityEngine;

public class BasicDialog : ScriptableObject
{

}

[System.Serializable]
public struct Phrase
{
    public CharacterType speaker;
    [TextArea]
    public string text;
}