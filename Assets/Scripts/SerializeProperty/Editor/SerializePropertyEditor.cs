using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace SerializePropertyEditing
{
    public class PropertyInfoByNameComparer : IEqualityComparer<PropertyInfo>
    {
        public bool Equals(PropertyInfo a, PropertyInfo b)
        {
            if(object.ReferenceEquals(a, b))
                return true;
            if(a is null)
            {
                if(b is null)
                    return true;
                else
                    return false;
            }
            return a.Name == b.Name;
        }

        public int GetHashCode(PropertyInfo propertyInfo)
        {
            if(propertyInfo is null)
                return String.Empty.GetHashCode();
            return propertyInfo.Name.GetHashCode();
        }
    }

    public class SerializePropertyEditor : Editor
    {
        private Type _type;
        private List<PropertyInfo> _properties;

        private void Initialize()
        {
            _type = target.GetType();
            _properties = GetProperties(_type);
        }
        private void Awake()
        {
            Initialize();
        }
        private void OnEnable()
        {
            Initialize();
        }

        public static List<PropertyInfo> GetProperties(Type type)
        {
            BindingFlags instanceFieldsAndProperties = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var monoBehaviorMembers = typeof(MonoBehaviour).GetProperties(instanceFieldsAndProperties);

            PropertyInfoByNameComparer comparer = new PropertyInfoByNameComparer();
            var localMembers = type.GetProperties(instanceFieldsAndProperties | BindingFlags.DeclaredOnly).ToList();
            var parentMembers = type.GetProperties(instanceFieldsAndProperties)
                                       .Except(monoBehaviorMembers, comparer)
                                       .Except(localMembers, comparer)
                                       .ToList();

            var allProperties = parentMembers.Concat(localMembers);

            List<PropertyInfo> result = new List<PropertyInfo>();

            foreach(var propertyInfo in allProperties)
            {
                if(IsGetterSerializable(propertyInfo))
                    result.Add(propertyInfo);
            }

            return result;
        }

        public static bool IsGetterSerializable(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetMethod != null &&
                   Attribute.IsDefined(propertyInfo, typeof(NotSerializeGetterAttribute)) == false &&
                   Attribute.IsDefined(propertyInfo, typeof(NotSerializePropertyAttribute)) == false &&
                   (propertyInfo.GetMethod.IsPrivate == false ||
                   Attribute.IsDefined(propertyInfo, typeof(SerializeGetterAttribute)) ||
                   Attribute.IsDefined(propertyInfo, typeof(SerializePropertyAttribute)));
        }
        public static bool IsSetterSerializable(PropertyInfo propertyInfo)
        {
            return propertyInfo.SetMethod != null &&
                   Attribute.IsDefined(propertyInfo, typeof(NotSerializeSetterAttribute)) == false &&
                   Attribute.IsDefined(propertyInfo, typeof(NotSerializePropertyAttribute)) == false &&
                   (propertyInfo.SetMethod.IsPrivate == false ||
                   Attribute.IsDefined(propertyInfo, typeof(SerializeSetterAttribute)) ||
                   Attribute.IsDefined(propertyInfo, typeof(SerializePropertyAttribute)));
        }

        public static object DrawInspectorField(Type type, string label, object value, params GUILayoutOption[] options)
        {
            string typeName;
            if(type == typeof(Int32))
                typeName = "Int";
            else
                typeName = type.Name;

            MethodInfo methodInfo = typeof(EditorGUILayout).GetMethod($"{typeName}Field", new Type[] { typeof(string), type, typeof(GUILayoutOption[]) });

            bool found = (methodInfo is null) == false && methodInfo.ReturnType == type;

            if(found == false)
                throw new NotImplementedException($"Can't draw field for type {type.Name}");

            return methodInfo.Invoke(null, new object[] { label, value, options });
        }

        public static T DrawInspectorField<T>(string label, T value, params GUILayoutOption[] options)
        {
            Type type = typeof(T);
            return (T)DrawInspectorField(type, label, value, options);
        }

        public static void DrawProperty(object target, PropertyInfo property)
        {
            if(IsGetterSerializable(property) == false)
                return;
            Type propertyType = property.PropertyType;
            string propertyName = property.Name;

            bool canSet = IsSetterSerializable(property);

            EditorGUI.BeginDisabledGroup(canSet == false);
            object result = DrawInspectorField(propertyType, propertyName, property.GetValue(target));
            EditorGUI.EndDisabledGroup();

            if(canSet)
            {
                try
                {
                    property.SetValue(target, result);
                }
                catch
                {

                }
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if(GUILayout.Button("Reset Properties"))
                ResetProperties();
            DrawDefaultInspector();
            foreach(var propertyInfo in _properties)
            {
                DrawProperty(target, propertyInfo);
            }
            serializedObject.ApplyModifiedProperties();
        }

        public void ResetProperties()
        {
            if(_type is null)
                return;
            object defaultInstance = Activator.CreateInstance(_type);
            Debug.Log("That's Ok to create a MonoBehaviour using 'new' word. It's needed for reseting properties.");
            foreach(var propertyInfo in _properties)
            {
                if(propertyInfo.SetMethod != null && propertyInfo.GetMethod != null)
                {
                    object defaultValue = propertyInfo.GetValue(defaultInstance);
                    propertyInfo.SetValue(target, defaultValue);
                }
            }
        }
    }
}
