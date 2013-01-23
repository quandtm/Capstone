using Capstone.Core;
using Capstone.Editor.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Capstone.Editor.Data
{
    internal enum ComponentPropertyType : int
    {
        String,
        Bool,
        Float,
        Int,
        Double,
        Enum
    }

    public sealed class ComponentProperty : BindableBase
    {
        private object _data;
        public object Data
        {
            get { return _data; }
            set
            {
                if (_data != null && _data.Equals(value)) return;
                _data = value;
            }
        }

        private PropertyInfo _propInfo;
        public string Name { get; private set; }
        public Type DataType { get; private set; }
        private ComponentPropertyType _type;

        public List<Enum> EnumOptions { get; private set; }

        protected ComponentProperty(string name, Type dataType)
        {
            Name = name;
            DataType = dataType;
            EnumOptions = new List<Enum>();

            InitialiseData();
        }

        public void ApplyProperty(object target)
        {
            _propInfo.SetMethod.Invoke(target, new[] { _data });
        }

        private void InitialiseData()
        {
            if (DataType == typeof(string))
            {
                _data = string.Empty;
                _type = ComponentPropertyType.String;
            }
            else if (DataType == typeof(bool))
            {
                _data = false;
                _type = ComponentPropertyType.Bool;
            }
            else if (DataType == typeof(float))
            {
                _data = 0.0f;
                _type = ComponentPropertyType.Float;
            }
            else if (DataType == typeof(int))
            {
                _data = 0;
                _type = ComponentPropertyType.Int;

            }
            else if (DataType == typeof(double))
            {
                _data = 0.0F;
                _type = ComponentPropertyType.Double;
            }
            else if (DataType.GetRuntimeMethod("HasFlag", new[] { typeof(Enum) }) != null)
            {
                _type = ComponentPropertyType.Enum;
                var items = Enum.GetValues(DataType);
                foreach (var i in items)
                    EnumOptions.Add((Enum)i);
                Data = EnumOptions[0];
            }
        }

        internal ComponentProperty Clone()
        {
            return new ComponentProperty(Name, DataType) { Data = this.Data, EnumOptions = this.EnumOptions, _propInfo = this._propInfo };
        }

        internal void SaveData(DataWriter dw)
        {
            switch (_type)
            {
                case ComponentPropertyType.String:
                    dw.WriteStringEx((string)_data);
                    break;

                case ComponentPropertyType.Bool:
                    dw.WriteBoolean((bool)_data);
                    break;

                case ComponentPropertyType.Float:
                    dw.WriteSingle((float)_data);
                    break;

                case ComponentPropertyType.Int:
                    dw.WriteInt32((int)_data);
                    break;

                case ComponentPropertyType.Double:
                    dw.WriteDouble((double)_data);
                    break;

                case ComponentPropertyType.Enum:
                    var name = Enum.GetName(DataType, _data);
                    dw.WriteStringEx(name);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal async Task LoadData(DataReader dr)
        {
            switch (_type)
            {
                case ComponentPropertyType.String:
                    Data = await dr.ReadStringEx();
                    break;

                case ComponentPropertyType.Bool:
                    await dr.LoadAsync(sizeof(bool));
                    Data = dr.ReadBoolean();
                    break;

                case ComponentPropertyType.Float:
                    await dr.LoadAsync(sizeof(float));
                    Data = dr.ReadSingle();
                    break;

                case ComponentPropertyType.Int:
                    await dr.LoadAsync(sizeof(int));
                    Data = dr.ReadInt32();
                    break;

                case ComponentPropertyType.Double:
                    await dr.LoadAsync(sizeof(double));
                    Data = dr.ReadDouble();
                    break;

                case ComponentPropertyType.Enum:
                    var name = await dr.ReadStringEx();
                    Data = Enum.Parse(DataType, name);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
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
                return new ComponentProperty(displayName, t) { _propInfo = pi };
            else
                return null;
        }
    }
}
