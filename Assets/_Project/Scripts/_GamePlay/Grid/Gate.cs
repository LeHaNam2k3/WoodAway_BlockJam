using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Collider gateCollider;
    [SerializeField] private GridSlot gridSlot;
    [SerializeField] private ColorType gateColorType;
    [SerializeField] private Direction gateDirection;
    [SerializeField] private int _gateLength;

    public void InitGate(ColorType getColor, Direction getDirection, int gateLength)
    {
        gateCollider.enabled = true;
        gateColorType = getColor;
        gateDirection = getDirection;
        _gateLength = gateLength;
    }

    public (ColorType gateColor, Direction gateDirection, int gateLength) GetGateInformation()
    {
        return (gateColorType, gateDirection, _gateLength);
    }

    public Collider GetCollider()
    {
        return gateCollider;
    }

    public bool IsCorrectGate(ColorType getColorType)
    {
        return gateColorType == getColorType;
    }
}