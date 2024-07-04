using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SeleniumAutotest
{
    internal enum StepTypes
    {
        Group, Open, FindElement, Click, CheckText, EnterText, CheckElement, WaitTime, CheckClassExists, DoubleClick, CheckAttribute, ReadAttributeToParameter, ReadTextToParameter, CheckClassNotExists, ReadAddressToParameter, CompareParameters, JsClick, AltClick, JsEvent, SetAttribute, InputToParameterByUser
    }
    // TODO: Предусмотреть прямое нажатие клавиш клавиатуры (например, для сочетаний)

    internal class StepTypesGroup
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public List<StepTypes> Types { get; set; }

        public List<StepTypes?> Parents { get; set; } = new List<StepTypes?>();

        public override string ToString()
        {
            return Name;
        }
    }

    internal static class StepType
    {
        public static List<StepTypesGroup> StepTypesGroups { get; set; } = new List<StepTypesGroup>()
        {
            new StepTypesGroup(){ 
                Index = 0,
                Name = "Группа шагов",
                Parents = new List<StepTypes?>(){ StepTypes.Group, null },
                Types = new List<StepTypes>(){ StepTypes.Group } },
            new StepTypesGroup(){
                Index = 1,
                Name = "Поиск",                
                Parents = new List<StepTypes?>(){ StepTypes.Group, StepTypes.FindElement },       
                Types = new List<StepTypes>(){ StepTypes.FindElement } },
            new StepTypesGroup(){
                Index = 2,
                Name = "Кликнуть",
                Parents = new List<StepTypes?>(){ StepTypes.FindElement },
                Types = new List<StepTypes>(){ StepTypes.Click, StepTypes.AltClick, StepTypes.JsClick, StepTypes.DoubleClick } },
            new StepTypesGroup(){
                Index = 3,
                Name = "Изменить",
                Parents = new List<StepTypes?>(){ StepTypes.FindElement },
                Types = new List<StepTypes>(){ StepTypes.EnterText, StepTypes.SetAttribute } },
            new StepTypesGroup(){
                Index = 4,
                Name = "Проверить",
                Parents = new List<StepTypes?>(){ StepTypes.FindElement },
                Types = new List<StepTypes>(){ StepTypes.CheckText, StepTypes.CheckClassExists, StepTypes.CheckClassNotExists, StepTypes.CheckAttribute, StepTypes.CheckElement, StepTypes.CompareParameters } },
            new StepTypesGroup(){
                Index = 6,
                Name = "Сохранить в параметр",
                Parents = new List<StepTypes?>(){ StepTypes.FindElement },
                Types = new List<StepTypes>(){ StepTypes.ReadTextToParameter, StepTypes.ReadAttributeToParameter, StepTypes.ReadAddressToParameter, StepTypes.InputToParameterByUser } },
            new StepTypesGroup(){
                Index = 7,
                Name = "Ждать время",
                Parents = new List<StepTypes?>(){ StepTypes.Group, StepTypes.FindElement },
                Types = new List<StepTypes>(){ StepTypes.WaitTime } },
            new StepTypesGroup(){
                Index = 8,
                Name = "Открыть сайт",
                Parents = new List<StepTypes?>(){ StepTypes.Group },
                Types = new List<StepTypes>(){ StepTypes.Open } },
            new StepTypesGroup(){
                Index = 5,
                Name = "Вызвать JS действие",  
                Parents = new List<StepTypes?>(){ StepTypes.FindElement }, 
                Types = new List<StepTypes>(){ StepTypes.JsEvent } },
        };

        public static Dictionary<StepTypes, string> Descriptions { get; } = new Dictionary<StepTypes, string>{
            { StepTypes.Group, "Группа шагов" },
            { StepTypes.Open, "Открыть сайт" },
            { StepTypes.FindElement, "Найти элемент" },
            { StepTypes.WaitTime, "Ждать время" },
            { StepTypes.JsEvent, "Вызвать событие" },

            { StepTypes.Click, "Просто" },
            { StepTypes.AltClick, "Альтернативно" },
            { StepTypes.JsClick, "Через JS" },
            { StepTypes.DoubleClick, "Двойной" },

            { StepTypes.CheckText, "Текст" },
            { StepTypes.CheckAttribute, "Атрибут" },
            { StepTypes.CheckClassExists, "Наличие класса" },
            { StepTypes.CheckClassNotExists, "Отсутствие класса" },
            { StepTypes.CheckElement, "Существования элемента" },
            { StepTypes.CompareParameters, "Сравнить 2 параметра" },

            { StepTypes.EnterText, "Ввести значение" },
            { StepTypes.SetAttribute, "Атрибут" },

            { StepTypes.ReadTextToParameter, "Текст" },
            { StepTypes.ReadAttributeToParameter, "Атрибут" },
            { StepTypes.ReadAddressToParameter, "URL" },
            { StepTypes.InputToParameterByUser, "Ввод пользователя" },
        };

        public static int GetIndexOfGroupByType(StepTypes stepType)
        {
            return StepTypesGroups.First(x => x.Types.Contains(stepType)).Index;
        }

        public static StepTypes GetTypeByName(string name)
        {
            if (Descriptions.ContainsValue(name))
            {
                return Descriptions.FirstOrDefault(x => x.Value == name).Key;
            }
            return StepTypes.Group;
        }

        public static string GetNamesByList(List<StepTypes> types)
        {
            List<string> res = new List<string>();
            foreach (StepTypes type in types)
            {
                res.Add(Descriptions[type]);
            }
            return string.Join("\r\n", res.ToArray());
        }
    }
}
