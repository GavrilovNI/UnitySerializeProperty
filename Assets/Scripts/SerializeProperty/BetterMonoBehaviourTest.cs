using UnityEngine;

namespace SerializePropertyEditing.Test
{
    public class BetterMonoBehaviourTest : BetterMonoBehaviour
    {
        public int DefaultIs51 { get; set; } = 51;

        private void BetterReset()
        {
            Debug.Log("BetterReset");
        }
    }
}
