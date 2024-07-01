﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SeleniumAutotest
{
    internal enum StepTypes
    {
        Group, Open, WaitElement, Click, CheckText, EnterValue, CheckElement, WaitTime, CheckClass, DoubleClick, CheckAttribute, ReadAttributeToParameter, ReadTextToParameter, CheckClassNotExists, ReadAddressToParameter
    }
    // TODO: Предусмотреть прямое нажатие клавиш клавиатуры (например, для сочетаний)

    internal static class StepType
    {
        public static Dictionary<StepTypes, string> descriptions = new Dictionary<StepTypes, string>{
            { StepTypes.Group, "Группа шагов" },
            { StepTypes.Open, "Открыть сайт" },
            { StepTypes.ReadAddressToParameter, "Считать адрес в параметр" },
            { StepTypes.WaitElement, "Ожидать нахождения элемента" },
            { StepTypes.Click, "Кликнуть" },
            { StepTypes.DoubleClick, "Двойной клик" },
            { StepTypes.CheckText, "Проверить текст" },
            { StepTypes.CheckClass, "Проверить наличие класса" },
            { StepTypes.CheckClassNotExists, "Проверить отсутствия класса" },
            { StepTypes.CheckAttribute, "Проверить атрибут" },
            { StepTypes.EnterValue, "Ввести значение" },
            { StepTypes.CheckElement, "Проверка нахождения элемента" },
            { StepTypes.WaitTime, "Ждать время" },
            { StepTypes.ReadAttributeToParameter, "Считать атрибут в параметр" },
            { StepTypes.ReadTextToParameter, "Считать текст в параметр" }
        };

        public static List<StepTypes> GetElementsForStepType(StepTypes stepType)
        {
            switch (stepType)
            {
                case StepTypes.Group: return new List<StepTypes> { StepTypes.Group, StepTypes.Open, StepTypes.WaitElement, StepTypes.CheckElement, StepTypes.WaitTime, StepTypes.ReadAddressToParameter };
                case StepTypes.WaitElement: return new List<StepTypes> { StepTypes.Click, StepTypes.DoubleClick, StepTypes.EnterValue, StepTypes.CheckText, StepTypes.CheckAttribute, StepTypes.CheckClass, StepTypes.CheckClassNotExists, StepTypes.CheckElement, StepTypes.ReadTextToParameter, StepTypes.ReadAttributeToParameter, StepTypes.WaitElement, StepTypes.WaitTime };
                case StepTypes.Open:
                case StepTypes.Click:
                case StepTypes.DoubleClick:
                case StepTypes.CheckText:
                case StepTypes.CheckAttribute:
                case StepTypes.CheckClass:
                case StepTypes.CheckClassNotExists:
                case StepTypes.EnterValue:
                case StepTypes.CheckElement:
                case StepTypes.ReadAttributeToParameter:
                case StepTypes.ReadTextToParameter:
                case StepTypes.ReadAddressToParameter:
                case StepTypes.WaitTime: return new List<StepTypes> { };
            }
            return new List<StepTypes> { StepTypes.Group };
        }

        public static StepTypes GetTypeByName(string name)
        {
            if (descriptions.ContainsValue(name))
            {
                return descriptions.FirstOrDefault(x => x.Value == name).Key;
            }
            return StepTypes.Group;
        }

        public static string GetNamesByList(List<StepTypes> types)
        {
            List<string> res = new List<string>();
            foreach (StepTypes type in types)
            {
                res.Add(descriptions[type]);
            }
            return string.Join("\r\n", res.ToArray());
        }
    }
}