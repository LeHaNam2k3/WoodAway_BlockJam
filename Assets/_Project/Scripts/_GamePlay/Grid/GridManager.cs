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
    [HeaderLine("BlockPuzzles")] [SerializeField]
    private List<BlockPuzzleSetup> blockPuzzleSetups;

    void SetPositionOnGrid(Transform target, int colum, int row)
    {
        target.transform.position = new Vector3(containGridSlot.position.x + gridSlot.GetSizeGrid().x * colum, containGridSlot.position.y + gridSlot.GetSizeGrid().y * row, 0);
    }

    [Button]
    void OnSpawnGrid()
    {
        containGridSlot.ClearTransform();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
               GridSlot slot= PrefabUtility.InstantiatePrefab(gridSlot, containGridSlot) as GridSlot;
               SetPositionOnGrid(slot.transform,j,i);
            }
        }
    }

    [Button]
    void SetUpAllBlockPuzzle()
    {
        foreach (var blockPuzzleSetUp in blockPuzzleSetups)
        {
            SetPositionOnGrid(blockPuzzleSetUp.blockPuzzle.transform, blockPuzzleSetUp.columnPos, blockPuzzleSetUp.rowPos);
        }
    }
}

[Serializable]
public class BlockPuzzleSetup
{
    public BlockPuzzle blockPuzzle;
    public int columnPos;
    public int rowPos;
}
