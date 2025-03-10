using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;

public class GridManager : MonoBehaviour
{
    [HeaderLine("GridSlot")] [SerializeField]
    private GridSlot gridSlot;

    [HeaderLine("Properties")] [SerializeField]
    private int columns;

    [SerializeField] private int rows;
    [SerializeField] private Transform containGridSlot;
    [SerializeField] private OnCollisionGateEvent onCollisionGateEvent;

    [HeaderLine("BlockPuzzles")] [SerializeField]
    private List<BlockPuzzleSetup> blockPuzzleSetups;

    [HeaderLine("Gate Setup")] [SerializeField]
    private List<SetupGate> setupGates;

    private GridSlot[,] _gridSlots;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void SetPositionOnGrid(Transform target, int colum, int row)
    {
        target.transform.position = new Vector3(containGridSlot.position.x + gridSlot.GetSizeGrid().x * colum,
            containGridSlot.position.y + gridSlot.GetSizeGrid().y * row, 0);
    }

    private void OnCheckGate(Tuple<BaseBlockPuzzle, int, int> blockInfor)
    {
    }

    [Button]
    private void OnSpawnGrid()
    {
        _gridSlots = new GridSlot[rows, columns];
        containGridSlot.ClearTransform();
        for (var i = 0; i < rows; i++)
        for (var j = 0; j < columns; j++)
        {
            var slot = PrefabUtility.InstantiatePrefab(gridSlot, containGridSlot) as GridSlot;
            _gridSlots[i, j] = slot;
            SetPositionOnGrid(slot.transform, j, i);
        }
    }

    [Button]
    private void SetUpAllBlockPuzzle()
    {
        foreach (var blockPuzzleSetUp in blockPuzzleSetups)
            SetPositionOnGrid(blockPuzzleSetUp.blockPuzzle.transform, blockPuzzleSetUp.columnPos,
                blockPuzzleSetUp.rowPos);
    }

    [Button]
    private void SetUpGate()
    {
        foreach (var gate in setupGates)
        foreach (var grid in gate.GateInfors)
            _gridSlots[grid.row, grid.column].OnSetUpGate(gate.ColorType);
    }
}

[Serializable]
public class BlockPuzzleSetup
{
    public BaseBlockPuzzle blockPuzzle;
    public int columnPos;
    public int rowPos;
}

[Serializable]
public class SetupGate
{
    public ColorType ColorType;
    public GateDirection gateDirection;
    public List<GateInfor> GateInfors = new();

    [Serializable]
    public struct GateInfor
    {
        public int row;
        public int column;
    }
}

public enum GateDirection
{
    Left,
    Right,
    Top,
    Bottom
}