#if UNITY_EDITOR
using _Game.Scripts.NPC;
using GoT._Game.Scripts.Utils;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CharacterSelectorAttribute))]
public class CharacterDataSelectorDrawer : PropertyDrawer
{
    private const float BUTTON_WIDTH = 24f;
    private const float BUTTON_PADDING = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ObjectReference)
        {
            EditorGUI.HelpBox(position, "Use CharacterDataSelector with Object references only", MessageType.Error);
            return;
        }

        NpcData[] allData = Resources.LoadAll<NpcData>("");

        GUIContent[] options = new GUIContent[allData.Length + 1];
        int[] optionIds = new int[allData.Length + 1];
        
        options[0] = new GUIContent("None");
        optionIds[0] = -1;
        
        int currentIndex = 0;
        for (int i = 0; i < allData.Length; i++)
        {
            options[i + 1] = new GUIContent(allData[i].name);
            optionIds[i + 1] = i;
            
            if (property.objectReferenceValue == allData[i])
            {
                currentIndex = i + 1;
            }
        }

        EditorGUI.BeginProperty(position, label, property);
        
        // Разделение области на основную часть и кнопку
        Rect fieldPosition = new Rect(position.x, position.y, position.width - BUTTON_WIDTH - BUTTON_PADDING, position.height);
        Rect buttonPosition = new Rect(position.x + position.width - BUTTON_WIDTH, position.y, BUTTON_WIDTH, position.height);
        
        // Отображение выпадающего списка
        int newIndex = EditorGUI.Popup(fieldPosition, label, currentIndex, options);
        
        if (newIndex != currentIndex)
        {
            property.objectReferenceValue = newIndex > 0 ? allData[newIndex - 1] : null;
        }
        
        // Кнопка для открытия объекта
        if (property.objectReferenceValue != null)
        {
            if (GUI.Button(buttonPosition, EditorGUIUtility.IconContent("d_ViewToolOrbit")))
            {
                EditorUtility.OpenPropertyEditor(property.objectReferenceValue);
            }
        }
        else
        {
            // Неактивная кнопка когда ничего не выбрано
            EditorGUI.BeginDisabledGroup(true);
            GUI.Button(buttonPosition, EditorGUIUtility.IconContent("d_ViewToolOrbit"));
            EditorGUI.EndDisabledGroup();
        }
        
        EditorGUI.EndProperty();
    }
}
#endif