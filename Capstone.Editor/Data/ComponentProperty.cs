﻿using Capstone.Core;
using Capstone.Editor.Common;
using System;
using System.Reflection;

namespace Capstone.Editor.Data
{
    public class ComponentProperty : BindableBase
    {
        private object _data;
        public object Data
        {
            get { return _data; }
            set
            {
                if (_data.Equals(value)) return;
                _data = value;
            }
        }

        public string Name { get; private set; }
        public Type DataType { get; private set; }

        protected ComponentProperty(string name, Type dataType)
        {
            Name = name;
            DataType = dataType;

            InitialiseData();
        }

        private void InitialiseData()
        {
            if (DataType == typeof(string))
                _data = string.Empty;
            else if (DataType == typeof(bool))
                _data = false;
            else if (DataType == typeof(float))
                _data = 0.0f;
            else if (DataType == typeof(int))
                _data = 0;
            else if (DataType == typeof (double))
                _data = 0.0F;
        }

        internal ComponentProperty Clone()
        {
            return new ComponentProperty(Name, DataType) { Data = Data };
        }

        public static ComponentProperty Create(PropertyInfo pi)
        {
            string displayName = string.Empty;
            Type t = null;
            bool found = false;
            foreach (var att in pi.CustomAttributes)
            {
                if (att.AttributeType == typeof(ComponentParameterAttribute))
                {
                    foreach (var arg in att.NamedArguments)
                    {
                        if (arg.MemberName == "DisplayName" && arg.TypedValue.ArgumentType == typeof(string))
                            displayName = (string)arg.TypedValue.Value;
                    }
                    found = true;
                    t = pi.PropertyType;
                    break;
                }
            }

            if (found)
                return new ComponentProperty(displayName, t);
            else
                return null;
        }
    }
}