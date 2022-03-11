using UnityEngine;

namespace SerializePropertyEditing
{
    public class SerializePropertyTest : MonoBehaviour
    {
        public int PublicField;
        private int _privateField;
        [SerializeField] private int _privateFieldWithAttribute;

        [NotSerializeProperty] public int PublicWithNotSerializePropertyAttribute { get; set; }
        [NotSerializeGetter] public int PublicWithNotSerializeGetterAttribute { get; set; }
        [NotSerializeSetter] public int PublicWithNotSerializeSetterAttribute { get; set; }

        public int Public { get; set; }
        public int PublicWithPrivateGetter { private get; set; }
        public int PublicWithPrivateSetter { get; private set; }

        public int PublicGetterOnly { get => Public; }
        public int PublicSetterOnly { set => Public = value; }

        [SerializeProperty] public int PublicWithPrivateGetterWithAttribute { private get; set; }
        [SerializeGetter] public int PublicWithPrivateGetterWithGetterAttribute { private get; set; }
        [SerializeSetter] public int PublicWithPrivateGetterWithSetterAttribute { private get; set; }

        [SerializeProperty] public int PublicWithPrivateSetterWithAttribute { get; private set; }
        [SerializeGetter] public int PublicWithPrivateSetterWithGetterAttribute { get; private set; }
        [SerializeSetter] public int PublicWithPrivateSetterWithSetterAttribute { get; private set; }


        [SerializeProperty] public int PublicWithProtectedGetterWithAttribute { protected get; set; }
        [SerializeGetter] public int PublicWithProtectedGetterWithGetterAttribute { protected get; set; }
        [SerializeSetter] public int PublicWithProtectedGetterWithSetterAttribute { protected get; set; }

        [SerializeProperty] public int PublicWithProtectedSetterWithAttribute { get; protected set; }
        [SerializeGetter] public int PublicWithProtectedSetterWithGetterAttribute { get; protected set; }
        [SerializeSetter] public int PublicWithProtectedSetterWithSetterAttribute { get; protected set; }


        private int Private { get; set; }
        [SerializeProperty] private int PrivateWithAttribute { get; set; }
        [SerializeGetter] private int PrivateWithGetterAttribute { get; set; }
        [SerializeSetter] private int PrivateWithSetterAttribute { get; set; }

        private int PrivateGetterOnly { get => Public; }
        [SerializeProperty] private int PrivateGetterOnlyWithAttribute { get => Public; }
        [SerializeGetter] private int PrivateGetterOnlyWithGetterAttribute { get => Public; }
        [SerializeSetter] private int PrivateGetterOnlyWithSetterAttribute { get => Public; }

        private int PrivateSetterOnly { set => Public = value; }
        [SerializeProperty] private int PrivateSetterOnlyWithAttribute { set => Public = value; }
        [SerializeGetter] private int PrivateSetterOnlyWithGetterAttribute { set => Public = value; }
        [SerializeSetter] private int PrivateSetterOnlyWithSetterAttribute { set => Public = value; }


        protected int Protected { get; set; }
        [SerializeProperty] protected int ProtectedWithAttribute { get; set; }
        [SerializeGetter] protected int ProtectedWithGetterAttribute { get; set; }
        [SerializeSetter] protected int ProtectedWithSetterAttribute { get; set; }

        protected int ProtectedGetterOnly { get => Public; }
        [SerializeProperty] protected int ProtectedGetterOnlyWithAttribute { get => Public; }
        [SerializeGetter] protected int ProtectedGetterOnlyWithGetterAttribute { get => Public; }
        [SerializeSetter] protected int ProtectedGetterOnlyWithSetterAttribute { get => Public; }

        protected int ProtectedSetterOnly { set => Public = value; }
        [SerializeProperty] protected int ProtectedSetterOnlyWithAttribute { set => Public = value; }
        [SerializeGetter] protected int ProtectedSetterOnlyWithGetterAttribute { set => Public = value; }
        [SerializeSetter] protected int ProtectedSetterOnlyWithSetterAttribute { set => Public = value; }


        private int _min0 = 0;
        public int Min0
        {
            get => _min0;
            set
            {
                if(value < 0)
                    throw new System.ArgumentOutOfRangeException(nameof(Min0));
                else
                    _min0 = value;
            }
        }

        public int DefaultIs51 { get; set; } = 51;

        public Vector2 Vector2 { get; set; }
    }
}