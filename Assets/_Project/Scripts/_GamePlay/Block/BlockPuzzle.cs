public class BlockPuzzle : BaseBlockPuzzle
{
    protected override void OnActionInGate()
    {
        Destroy(gameObject);
    }
}