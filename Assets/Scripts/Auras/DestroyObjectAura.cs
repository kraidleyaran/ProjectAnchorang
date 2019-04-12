
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Destroy Object Aura", menuName = @"Anchorang/Aura/Destroy Object")]
    public class DestroyObjectAura : Aura
    {
        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            Destroy(controller.transform.parent.gameObject);
        }
    }
}