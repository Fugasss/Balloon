using UnityEngine;

public class BonusSpawner : Spawner<BonusBase>
{
    protected override void OnEnable()
    {
        base.OnEnable();
        BonusBase.OutOfBounds += ReturnInPool;
        BonusBase.Destroy += ReturnInPool;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        BonusBase.OutOfBounds -= ReturnInPool;
        BonusBase.Destroy -= ReturnInPool;
    }

    protected override Color CalculateColor(int health)
    {
        return Color.white;
    }
}