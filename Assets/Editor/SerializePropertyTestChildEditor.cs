using UnityEngine;
using UnityEditor;

namespace SerializePropertyEditing.Test
{
    [CustomEditor(typeof(SerializePropertyTestChild)), CanEditMultipleObjects]
    public class SerializePropertyTestChildEditor : SerializePropertyEditor
    {

    }
}
