using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Events;

public abstract class BaseBlockPuzzle : MonoBehaviour
{
    [SerializeField] private float raycastCheckGateLength, speedMove = 50f;
    [SerializeField] private Vector3Event onPositionBlockStopMoveEvent, onStopMoveBlockEvent;
    [SerializeField] private BlockDirection blockDirection;
    [SerializeField] private ColorType colorType;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private Mesh gridMesh;
    [SerializeField] private LayerMask gridMask;
    private Gate _currentGate;
    private Vector3 _direction, _offPivot, _offsetMovement, _prevPosition;
    private BlockState blockState;

    private void Start()
    {
        rigidbody.isKinematic = true;
        _offPivot = transform.position - (meshCollider.bounds.min + gridMesh.bounds.size / 2);
    }

    private void FixedUpdate()
    {
        if (blockState != BlockState.Move) return;
        var moveDirection = _direction - GetPositionInputDown();
        float remainingDistance = moveDirection.magnitude, maxDistance = speedMove * Time.deltaTime;

        rigidbody.velocity = moveDirection.normalized * (remainingDistance <= maxDistance
            ? speedMove * Mathf.Clamp01(remainingDistance / maxDistance)
            : speedMove);

        if (remainingDistance < 0.01f)
        {
            rigidbody.velocity = Vector3.zero;
            blockState = BlockState.Stop;
        }

        Debug.DrawRay(transform.position + _offPivot, Vector3.forward * 100, Color.yellow);
    }

    private void OnDisable()
    {
        onPositionBlockStopMoveEvent.OnRaised -= OnPositionStopMove;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Gate") ||
            Vector3.Distance(_prevPosition, transform.position) < gridMesh.bounds.size.magnitude / 4)
            return;

        _currentGate = other.GetComponent<Gate>();
        if (!_currentGate.IsCorrectGate(colorType)) return;

        _prevPosition = transform.position;
        var countGate = CountValidGateHits(_currentGate.GetGateInformation().gateDirection);
        if (countGate == _currentGate.GetGateInformation().gateLength) OnActionInGate();
    }

    private int CountValidGateHits(Direction direction)
    {
        var directions = direction switch
        {
            Direction.Left => blockDirection.Left,
            Direction.Right => blockDirection.Right,
            Direction.Bottom => blockDirection.Down,
            Direction.Top => blockDirection.Up,
            _ => null
        };

        var countGate = 0;
        foreach (var dir in directions)
            if (OnShootRaycastCheckGate(dir.rowRaycast, dir.columnRaycast, direction))
                countGate++;
        return countGate;
    }

    private bool OnShootRaycastCheckGate(int row, int column, Direction direction)
    {
        var origin = GetPivotBlock() + new Vector3(gridMesh.bounds.size.x * column, gridMesh.bounds.size.y * row, 0);
        var directionVector = direction switch
        {
            Direction.Left => Vector3.left,
            Direction.Right => Vector3.right,
            Direction.Bottom => Vector3.down,
            Direction.Top => Vector3.up,
            _ => Vector3.zero
        } * raycastCheckGateLength;

        Debug.DrawRay(origin, directionVector, Color.red);
        return Physics.Raycast(origin, directionVector, out var hit, Mathf.Infinity) && hit.collider.CompareTag("Gate");
    }

    public void OnStartMove(Vector3 startInputPos)
    {
        rigidbody.isKinematic = false;
        _offsetMovement = startInputPos - transform.position;
        onPositionBlockStopMoveEvent.OnRaised += OnPositionStopMove;
    }

    public void OnStopMove()
    {
        rigidbody.isKinematic = true;
        blockState = BlockState.Stop;
        rigidbody.velocity = Vector3.zero;
        onStopMoveBlockEvent.Raise(GetPivotBlock());
    }

    private void OnPositionStopMove(Vector3 currentSlotPos)
    {
        var offset = new Vector3(currentSlotPos.x, currentSlotPos.y, transform.position.z) -
                     (transform.position - _offPivot);
        transform.position += offset;
        onPositionBlockStopMoveEvent.OnRaised -= OnPositionStopMove;
    }

    private Vector3 GetPositionInputDown()
    {
        return transform.position + _offsetMovement;
    }

    public Vector3 GetPivotBlock()
    {
        return transform.position - _offPivot;
    }

    public ColorType GetColorType()
    {
        return colorType;
    }

    public void OnUpdateBlockPos(Vector3 getDirection)
    {
        blockState = BlockState.Move;
        _direction = getDirection;
    }

    protected abstract void OnActionInGate();
}

[Serializable]
public class BlockDirection
{
    public List<BlockDirectionInfor> Up, Down, Left, Right;

    [Serializable]
    public struct BlockDirectionInfor
    {
        public int rowRaycast, columnRaycast;
    }
}

public enum BlockState
{
    Stop,
    Move
}

public enum ColorType
{
    Red,
    Yellow,
    Orange,
    Green,
    Blue,
    Purple
}