using UnityEngine;
using UnityEditor;

namespace SerializePropertyEditing.Test
{
    [CustomEditor(typeof(SerializePropertyTest)), CanEditMultipleObjects]
    public class SerializePropertyTestEditor : SerializePropertyEditor
    {

    }
}
