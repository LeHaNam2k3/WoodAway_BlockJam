using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VirtueSky.Events;
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
    [SerializeField] private Vector3Event onStopMoveBlockEvent;
    [SerializeField] private Vector3Event onPositionBlockStopMoveEvent;

    [HeaderLine("BlockPuzzles")] [SerializeField]
    private List<BlockPuzzleSetup> blockPuzzleSetups;

    [HeaderLine("Gate Setup")] [SerializeField]
    private List<SetupGate> setupGates;

    [SerializeField] List<GridSlot> _gridSlots = new List<GridSlot>();

    private void OnEnable()
    {
        onStopMoveBlockEvent.OnRaised += OnCheckBlockStopMoveEvent;
    }

    private void OnDisable()
    {
        onStopMoveBlockEvent.OnRaised -= OnCheckBlockStopMoveEvent;
    }

    private void SetPositionOnGrid(Transform target, int colum, int row)
    {
        target.transform.position = new Vector3(containGridSlot.position.x + gridSlot.GetSizeGrid().x * colum,
            containGridSlot.position.y + gridSlot.GetSizeGrid().y * row, 0);
    }

    void OnCheckBlockStopMoveEvent(Vector3 blockPivot)
    {
        onPositionBlockStopMoveEvent.Raise(_gridSlots[GetGridSlotIndex(blockPivot).getRowGridSlot*rows+GetGridSlotIndex(blockPivot).getColumnSlot].transform.position);
    }

    (int getRowGridSlot, int getColumnSlot) GetGridSlotIndex(Vector3 blockPivot)
    {
        Vector2 gridSize = gridSlot.GetSizeGrid();
        Vector2 firstSlotPos = _gridSlots[0].transform.position;
        Vector2 offset = firstSlotPos - gridSize / 2;
        int rowIndex = Mathf.FloorToInt((blockPivot.y - offset.y) / gridSize.y);
        int columnIndex = Mathf.FloorToInt((blockPivot.x - offset.x) / gridSize.x);
        return (rowIndex, columnIndex);
    }

    private void OnCheckGate(Tuple<BaseBlockPuzzle,GridSlot, BlockDirection> blockInfor)
    {
        //blockInfor.Item1.GetPivotBlock()
        var validGate = setupGates
            .FirstOrDefault(gate => gate.GateInfors
                .Any(c => c.row == blockInfor.Item2.GetRowAndColumn().row 
                          && c.column == blockInfor.Item2.GetRowAndColumn().colum));
        GridSlot currentGrid =
            _gridSlots[GetGridSlotIndex(blockInfor.Item1.GetPivotBlock()).getRowGridSlot * rows + GetGridSlotIndex(blockInfor.Item1.GetPivotBlock()).getColumnSlot];
        switch (validGate.direction)
        {
            case Direction.Left:
                
                break;
            case Direction.Right:
                
                break;
            case Direction.Top:
                
                break;
            case Direction.Bottom:
                    
                break;
        }
    }

    [Button]
    private void OnSpawnGrid()
    {
        containGridSlot.ClearTransform();
        for (var i = 0; i < rows; i++)
        for (var j = 0; j < columns; j++)
        {
            var slot = PrefabUtility.InstantiatePrefab(gridSlot, containGridSlot) as GridSlot;
            slot.Init(i,j);
            _gridSlots.Add(slot);
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
            _gridSlots[grid.row*rows+ grid.column].OnSetUpGate(gate.ColorType, gate.direction,gate.GateInfors.Count);
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
    public Direction direction;
    public List<GateInfor> GateInfors = new();

    [Serializable]
    public struct GateInfor
    {
        public int row;
        public int column;
    }
}

public enum Direction
{
    Left,
    Right,
    Top,
    Bottom
}