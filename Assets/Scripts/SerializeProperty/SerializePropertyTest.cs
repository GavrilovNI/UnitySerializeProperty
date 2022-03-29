using UnityEngine;

namespace SerializePropertyEditing.Test
{
    public class SerializePropertyTest : MonoBehaviour
    {
        public int PublicField;
        private int _privateField;
        [SerializeField] private int _privateFieldWithSerializeFieldAttribute;
        [System.NonSerialized] public int PublicFieldWithNonSerializedNonAttribute;

        public int Public       { get; set; }
        protected int Protected { get; set; }
        private int Private     { get; set; }
        public int PublicPrivateGetter       { private get; set; }
        public int PublicPrivateSetter       { get; private set; }
        public int PublicProtectedGetter     { protected get; set; }
        public int PublicProtectedSetter     { get; protected set; }
        protected int ProtectedPrivateGetter { private get; set; }
        protected int ProtectedPrivateSetter { get; private set; }

        public int    PublicGetterOnly    { get => Public; }
        public int    PublicSetterOnly    { set => Public = value; }
        protected int ProtectedGetterOnly { get => Public; }
        protected int ProtectedSetterOnly { set => Public = value; }
        private int   PrivateGetterOnly   { get => Public; }
        private int   PrivateSetterOnly   { set => Public = value; }

        [SerializeProperty]               public int PublicAttribute           { get; set; }
        [SerializeProperty(true, true)]   public int PublicAttributeTrueTrue   { get; set; }
        [SerializeProperty(false, true)]  public int PublicAttributeFalseTrue  { get; set; }
        [SerializeProperty(true, false)]  public int PublicAttributeTrueFalse  { get; set; }
        [SerializeProperty(false, false)] public int PublicAttributeFalseFalse { get; set; }

        [SerializeProperty]               public int PublicProtectedSetterAttribute           { get; protected set; }
        [SerializeProperty(true, true)]   public int PublicProtectedSetterAttributeTrueTrue   { get; protected set; }
        [SerializeProperty(false, true)]  public int PublicProtectedSetterAttributeFalseTrue  { get; protected set; }
        [SerializeProperty(true, false)]  public int PublicProtectedeSetterAttributeTrueFalse { get; protected set; }
        [SerializeProperty(false, false)] public int PublicProtectedSetterAttributeFalseFalse { get; protected set; }

        [SerializeProperty]               public int PublicProtectedGetterAttribute           { protected get; set; }
        [SerializeProperty(true, true)]   public int PublicProtectedGetterAttributeTrueTrue   { protected get; set; }
        [SerializeProperty(false, true)]  public int PublicProtectedGetterAttributeFalseTrue  { protected get; set; }
        [SerializeProperty(true, false)]  public int PublicProtectedGetterAttributeTrueFalse  { protected get; set; }
        [SerializeProperty(false, false)] public int PublicProtectedGetterAttributeFalseFalse { protected get; set; }

        [SerializeProperty]               public int PublicPrivateSetterAttribute           { get; private set; }
        [SerializeProperty(true, true)]   public int PublicPrivateSetterAttributeTrueTrue   { get; private set; }
        [SerializeProperty(false, true)]  public int PublicPrivateSetterAttributeFalseTrue  { get; private set; }
        [SerializeProperty(true, false)]  public int PublicPrivateSetterAttributeTrueFalse  { get; private set; }
        [SerializeProperty(false, false)] public int PublicPrivateSetterAttributeFalseFalse { get; private set; }

        [SerializeProperty]               public int PublicPrivateGetterAttribute           { private get; set; }
        [SerializeProperty(true, true)]   public int PublicPrivateGetterAttributeTrueTrue   { private get; set; }
        [SerializeProperty(false, true)]  public int PublicPrivateGetterAttributeFalseTrue  { private get; set; }
        [SerializeProperty(true, false)]  public int PublicPrivateGetterAttributeTrueFalse  { private get; set; }
        [SerializeProperty(false, false)] public int PublicPrivateGetterAttributeFalseFalse { private get; set; }

        [SerializeProperty]               protected int ProtectedAttribute           { get; set; }
        [SerializeProperty(true, true)]   protected int ProtectedAttributeTrueTrue   { get; set; }
        [SerializeProperty(false, true)]  protected int ProtectedAttributeFalseTrue  { get; set; }
        [SerializeProperty(true, false)]  protected int ProtectedAttributeTrueFalse  { get; set; }
        [SerializeProperty(false, false)] protected int ProtectedAttributeFalseFalse { get; set; }

        [SerializeProperty]               protected int ProtectedPrivateSetterAttribute           { get; private set; }
        [SerializeProperty(true, true)]   protected int ProtectedPrivateSetterAttributeTrueTrue   { get; private set; }
        [SerializeProperty(false, true)]  protected int ProtectedPrivateSetterAttributeFalseTrue  { get; private set; }
        [SerializeProperty(true, false)]  protected int ProtectedPrivateSetterAttributeTrueFalse  { get; private set; }
        [SerializeProperty(false, false)] protected int ProtectedPrivateSetterAttributeFalseFalse { get; private set; }

        [SerializeProperty]               protected int ProtectedPrivateGetterAttribute           { private get; set; }
        [SerializeProperty(true, true)]   protected int ProtectedPrivateGetterAttributeTrueTrue   { private get; set; }
        [SerializeProperty(false, true)]  protected int ProtectedPrivateGetterAttributeFalseTrue  { private get; set; }
        [SerializeProperty(true, false)]  protected int ProtectedPrivateGetterAttributeTrueFalse  { private get; set; }
        [SerializeProperty(false, false)] protected int ProtectedPrivateGetterAttributeFalseFalse { private get; set; }

        [SerializeProperty]               private int PrivateAttribute           { get; set; }
        [SerializeProperty(true, true)]   private int PrivateAttributeTrueTrue   { get; set; }
        [SerializeProperty(false, true)]  private int PrivateAttributeFalseTrue  { get; set; }
        [SerializeProperty(true, false)]  private int PrivateAttributeTrueFalse  { get; set; }
        [SerializeProperty(false, false)] private int PrivateAttributeFalseFalse { get; set; }


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
        public GameObject GameObjectField { get; set; }
        public SerializePropertyTest MonoBehavourField { get; set; }
        public GameObject SomePrefab { get; set; }
    }
}