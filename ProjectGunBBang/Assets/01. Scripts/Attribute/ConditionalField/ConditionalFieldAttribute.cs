using System;
using UnityEngine;

namespace GB.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public string Condition;
        public bool BoolOption;
        public int EnumOption;
        public ConditionType Type = ConditionType.None;

        public ConditionalFieldAttribute(string conditionName, bool option)
        {
            Condition = conditionName;
            BoolOption = option;
            Type = ConditionType.Boolean;
        }

        public ConditionalFieldAttribute(string conditionName, int enumOption)
        {
            Condition = conditionName;
            EnumOption = enumOption;
            Type = ConditionType.Enum;
        }

        public enum ConditionType
        {
            None,
            Boolean,
            Enum
        }
    }
}