using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEditor.SceneManagement;

namespace SerializePropertyEditing
{
    public class PropertyInfoComparerByName : IEqualityComparer<PropertyInfo>
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

            PropertyInfoComparerByName comparer = new PropertyInfoComparerByName();
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
            SerializePropertyAttribute attribute = (SerializePropertyAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(SerializePropertyAttribute));
            return propertyInfo.GetMethod != null && (attribute == null ? propertyInfo.GetMethod.IsPrivate == false : attribute.SerializeGetter);
        }
        public static bool IsSetterSerializable(PropertyInfo propertyInfo)
        {
            SerializePropertyAttribute attribute = (SerializePropertyAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(SerializePropertyAttribute));
            return propertyInfo.SetMethod != null && (attribute == null ? propertyInfo.SetMethod.IsPrivate == false : attribute.SerializeSetter);
        }

        public static bool AllowSceneObjectByLabel(string label) =>
            label.EndsWith("prefab", StringComparison.InvariantCultureIgnoreCase) == false;

        public static object DrawInspectorField(Type type, string label, object value, params GUILayoutOption[] options)
        {
            string typeName;

            if(type.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                bool allowSceneObjects = AllowSceneObjectByLabel(label);
                return EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, type, allowSceneObjects, options);
            }
            else
            {
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
        }

        public static T DrawInspectorField<T>(string label, T value, params GUILayoutOption[] options)
        {
            Type type = typeof(T);
            return (T)DrawInspectorField(type, label, value, options);
        }

        public static string PropertyNameToLabel(string name)
        {
            return string.Concat(name.Select(c => char.IsUpper(c) ? $" {c}" : c.ToString())).TrimStart(' ');
        }

        public static void DrawProperty(object target, PropertyInfo property)
        {
            if(IsGetterSerializable(property) == false)
                return;
            Type propertyType = property.PropertyType;
            string label = PropertyNameToLabel(property.Name);

            bool canSet = IsSetterSerializable(property);
            EditorGUI.BeginDisabledGroup(canSet == false);
            object result = DrawInspectorField(propertyType, label, property.GetValue(target));
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
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(((MonoBehaviour)target).gameObject.scene);
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
