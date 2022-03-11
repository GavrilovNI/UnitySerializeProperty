using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace SerializePropertyEditing
{
    public class MemberInfoByNameComparer : IEqualityComparer<MemberInfo>
    {
        public bool Equals(MemberInfo a, MemberInfo b)
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

        public int GetHashCode(MemberInfo propertyInfo)
        {
            if(propertyInfo is null)
                return String.Empty.GetHashCode();
            return propertyInfo.Name.GetHashCode();
        }
    }

    public class SerializePropertyEditor : Editor
    {
        private MonoBehaviour _target;
        private Type _type;
        private List<MemberInfo> _fieldsAndProperties = new List<MemberInfo>();

        private void OnEnable()
        {
            _type = target.GetType();
            _target = (MonoBehaviour)target;
            _fieldsAndProperties = GetFieldsAndProperties(_type);
        }

        public static List<MemberInfo> GetFieldsAndProperties(Type type)
        {
            BindingFlags instanceFieldsAndProperties = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var monoBehaviorMembers = typeof(MonoBehaviour).GetMembers(instanceFieldsAndProperties);

            MemberInfoByNameComparer comparer = new MemberInfoByNameComparer();
            var localMembers = type.GetMembers(instanceFieldsAndProperties | BindingFlags.DeclaredOnly).ToList();
            var parentMembers = type.GetMembers(instanceFieldsAndProperties)
                                       .Except(monoBehaviorMembers, comparer)
                                       .Except(localMembers, comparer)
                                       .ToList();

            foreach(var local in localMembers)
            {
                if(local is FieldInfo || local is PropertyInfo)
                    Debug.Log(local.Name);
            }

            var allProperties = parentMembers.Concat(localMembers);

            List<MemberInfo> result = new List<MemberInfo>();

            foreach(var memberInfo in allProperties)
            {
                if(memberInfo is FieldInfo fieldInfo)
                {
                    if(IsFieldSerializable(fieldInfo))
                        result.Add(memberInfo);
                }
                else if(memberInfo is PropertyInfo propertyInfo)
                {
                    if(IsGetterSerializable(propertyInfo))
                        result.Add(propertyInfo);
                }
            }

            return result;
        }

        public static bool IsFieldSerializable(FieldInfo fieldInfo)
        {
            return fieldInfo != null &&
                   Attribute.IsDefined(fieldInfo, typeof(System.NonSerializedAttribute)) == false &&
                   (fieldInfo.IsPrivate == false ||
                   Attribute.IsDefined(fieldInfo, typeof(SerializeField)));
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
            foreach(var memberInfo in _fieldsAndProperties)
            {
                if(memberInfo is FieldInfo)
                {
                    SerializedProperty property = serializedObject.FindProperty(memberInfo.Name);
                    EditorGUILayout.PropertyField(property);
                }
                else
                {
                    DrawProperty(target, memberInfo as PropertyInfo);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
