using DG.Tweening;

namespace Assets.Scripts.Auras
{
    public class AuraCooldown
    {
        public Aura Aura { get; set; }
        public Sequence Cooldown { get; set; }
        public bool ApplyAfterCooldown { get; set; }
    }
}