using Assets.Scripts.Auras;

namespace Assets.Scripts.System
{
    public class AuraIcon
    {
        public Aura Parent { get; set; }
        public UiAuraIconController Icon { get; set; }
        public int StackCount = 1;
    }
}