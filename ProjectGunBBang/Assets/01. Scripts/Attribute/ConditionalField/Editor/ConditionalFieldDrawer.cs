using UnityEditor;
using UnityEngine;

namespace GB.Attributes
{

    [CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditional.Condition);

            if(conditionProperty == null)
                return;

            bool condition = false;
            switch(conditional.Type)
            {
                case ConditionalFieldAttribute.ConditionType.Boolean:
                    condition = conditionProperty.boolValue == conditional.BoolOption;
                    break;
                case ConditionalFieldAttribute.ConditionType.Enum:
                    condition = conditionProperty.enumValueIndex == conditional.EnumOption;
                    break;
            }

            if (condition)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(position, property, label, true);
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditional.Condition);

            if(conditionProperty == null)
                return -EditorGUIUtility.standardVerticalSpacing;

            bool condition = false;
            switch(conditional.Type)
            {
                case ConditionalFieldAttribute.ConditionType.Boolean:
                    condition = conditionProperty.boolValue == conditional.BoolOption;
                    break;
                case ConditionalFieldAttribute.ConditionType.Enum:
                    condition = conditionProperty.enumValueIndex == conditional.EnumOption;
                    break;
            }

            if (condition)
                return EditorGUI.GetPropertyHeight(property, label);
            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}