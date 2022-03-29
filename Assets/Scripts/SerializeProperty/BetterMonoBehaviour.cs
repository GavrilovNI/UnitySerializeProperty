using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SerializePropertyEditing
{
    public abstract class BetterMonoBehaviourInner : MonoBehaviour
    {
        protected abstract void Reset();
    }

    public abstract class BetterMonoBehaviour : BetterMonoBehaviourInner
    {
        public const string Prefix = "Better";

        private void CallMethodIfExists(string name)
        {
            CallMethodIfExists(this, name);
        }
        private void CallMethodIfExists(object @object, string name)
        {
            MethodInfo methodInfo = @object.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { }, null);
            if(methodInfo != null)
                methodInfo.Invoke(@object, new object[] { });
        }

        sealed protected override void Reset()
        {
#if UNITY_EDITOR
            Editor editor = Editor.CreateEditor(this);
            CallMethodIfExists(editor, "ResetProperties");
#endif
            CallMethodIfExists($"{Prefix}Reset");
        }
    }
}