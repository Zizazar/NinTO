
using UnityEngine;

public class BasicDialog : ScriptableObject
{

}

[System.Serializable]
public struct Phrase
{
    public Speaker speaker;
    [TextArea]
    public string text;
}