using Core;

namespace FallingObject
{
    public class BonusDamage : BonusBase
    {
        private Clicker m_Clicker;

        internal void Construct(Clicker clicker)
        {
            m_Clicker = clicker;
        }

        protected override void AfterDie()
        {
            Use();
        }

        public override int GetScore()
        {
            return MaxHealth / 5;
        }

        public override void Use()
        {
            m_Clicker.AddDamage(1);
        }
    }
}