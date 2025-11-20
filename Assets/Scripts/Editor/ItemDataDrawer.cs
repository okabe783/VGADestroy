using UnityEngine;
using VGADestroy.Item;

namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(ItemData.ItemDataList))]
    public class ItemDataDrawer :  PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // デフォルトは 1 行（ItemType）
            int line = 1;

            var typeProp = property.FindPropertyRelative("ItemType");
            var type = (ItemType)typeProp.enumValueIndex;

            // ItemTypeごとに表示行数追加
            switch (type)
            {
                case ItemType.SpeedUp:
                case ItemType.SpeedDown:
                    line += 1; // Speed を表示
                    break;

                case ItemType.PowerUp:
                case ItemType.PowerDown:
                    line += 1; // Power を表示
                    break;
            }

            return (EditorGUIUtility.singleLineHeight + 4) * line;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float height = EditorGUIUtility.singleLineHeight;
            Rect rect = new(position.x, position.y, position.width, height);

            // Enum
            var typeProp = property.FindPropertyRelative("ItemType");
            EditorGUI.PropertyField(rect, typeProp);
            rect.y += height + 2;

            var type = (ItemType)typeProp.enumValueIndex;

            // 条件でフィールド表示
            switch (type)
            {
                case ItemType.SpeedUp:
                case ItemType.SpeedDown:
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("Speed"));
                    break;

                case ItemType.PowerUp:
                case ItemType.PowerDown:
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("Power"));
                    break;
            }

            EditorGUI.EndProperty();
        }
    }
}