using UnityEditor;
using UnityEngine;

namespace SerializePropertyEditing.Test
{
    [CustomEditor(typeof(BetterMonoBehaviourTest)), CanEditMultipleObjects]
    public class BetterMonobehaviourTestEditor : SerializePropertyEditor
    {

    }
}
