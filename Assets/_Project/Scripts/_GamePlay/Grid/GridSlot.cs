using UnityEngine;
using VirtueSky.Inspector;

public class GridSlot : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int column;
    [SerializeField] private Mesh gridMesh;
    [SerializeField] private Gate gate;

    public void Init(int getRow, int getColumn)
    {
        row = getRow;
        column = getColumn;
        gate.GetCollider().enabled = false;
    }
    
    public void OnSetUpGate(ColorType getColorType, Direction getDirection, int gateLength)
    {
      gate.InitGate(getColorType,getDirection, gateLength);
    }
    
    public (int row, int colum) GetRowAndColumn()
    {
        return (row, column);
    }

    public Vector2 GetSizeGrid()
    {
        return gridMesh.bounds.size;
    }
}