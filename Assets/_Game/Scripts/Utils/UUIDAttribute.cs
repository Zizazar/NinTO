using System;
using UnityEditor;
using UnityEngine;

namespace GoT._Game.Scripts.Utils
{
    
    public class UUIDAttribute : PropertyAttribute { }
    
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(UUIDAttribute))]
    public class UUIDDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, "ERROR:", "UUID can only be used with string fields");
                return;
            }

            // Рассчитываем размеры элементов
            float buttonWidth = 25;
            Rect fieldRect = new Rect(position.x, position.y, position.width - buttonWidth - 5, position.height);
            Rect buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height);

            // Отображаем поле UUID (только для чтения)
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(fieldRect, property, label);
            EditorGUI.EndDisabledGroup();

            // Кнопка перегенерации
            if (GUI.Button(buttonRect, EditorGUIUtility.IconContent("Refresh", "Regenerate")))
            {
                GenerateNewUUID(property);
            }
        }

        private void GenerateNewUUID(SerializedProperty property)
        {
            // Генерируем новый UUID
            property.stringValue = Guid.NewGuid().ToString();
            property.serializedObject.ApplyModifiedProperties();
        
            // Фиксируем изменение для системы Undo
            Undo.RecordObjects(property.serializedObject.targetObjects, "Regenerate UUID");
        }
    }
#endif
}