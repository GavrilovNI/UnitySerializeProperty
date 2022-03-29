using System;

namespace SerializePropertyEditing
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SerializePropertyAttribute : Attribute
    {
        public bool SerializeGetter { get; private set; }
        public bool SerializeSetter { get; private set; }

        public SerializePropertyAttribute(bool serializeGetter = true, bool serializeSetter = true)
        {
            SerializeGetter = serializeGetter;
            SerializeSetter = serializeSetter;
        }
    }
}
