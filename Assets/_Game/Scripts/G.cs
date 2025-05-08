using _Game.Scripts.DialogueSystem;
using _Game.Scripts.NPC;
using _Game.Scripts.Player;
using UnityEngine;

// <summary>
// Все ссылки на объекты в игре
// Задаются в классе Main при иницилизации
// Используются в других скриптах если нужны зависимости
// </summary>

// ReSharper disable once InconsistentNaming
public static class G
{
    public static Main main { get; set; }
    public static UIController ui { get; set; }
    public static PlayerController player  { get; set; }
    public static NpcController currentNpc  { get; set; }
    public static DialogueController dialogueController { get; set; }
    
    
    
    public static readonly Camera camera = Camera.main;
    
       
    public enum Characters
    {
        Player,
        Rose, Bill, Dupon, Mer 
    }
    public static string GetCharacterName(Characters character)
    {
        switch (character)
        {
            default: return "";
            case Characters.Player: return "Лукас";
            case Characters.Rose: return "Мадам Роза";
            case Characters.Bill: return "Старик Билл";
            case Characters.Dupon: return "Мисье Дюпон";
            case Characters.Mer: return "Мэр";
        }
    }
}
