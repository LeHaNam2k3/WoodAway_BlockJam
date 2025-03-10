using UnityEngine;
using VirtueSky.Inspector;

public class GridSlot : MonoBehaviour
{
    [SerializeField] private Mesh gridMesh;
    [SerializeField] private bool isGate;

    [ShowIf(nameof(isGate))] [SerializeField]
    private ColorType colorType;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void OnSetUpGate(ColorType getColorType)
    {
        colorType = getColorType;
        isGate = true;
    }

    public bool IsCorrectGate(ColorType getColorType)
    {
        if (!isGate)
            return false;
        return colorType == getColorType;
    }

    public Vector2 GetSizeGrid()
    {
        return gridMesh.bounds.size;
    }
}