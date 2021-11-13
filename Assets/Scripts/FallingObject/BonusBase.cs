using System;

public abstract class BonusBase : FallingObjectBase
{
    public static event Action<BonusBase> OutOfBounds = delegate { };
    public new static event Action<BonusBase> Destroy = delegate { };
    public abstract void Use();

    protected override void OnOutOfBounds()
    {
        OutOfBounds?.Invoke(this);
    }

    protected override void AfterDie()
    {
        base.AfterDie();
        
        Destroy?.Invoke(this);
    }
}