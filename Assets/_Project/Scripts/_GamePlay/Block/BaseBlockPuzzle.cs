using System;
using UnityEngine;
using VirtueSky.Events;

public abstract class BaseBlockPuzzle : MonoBehaviour
{
    [SerializeField] private Vector3Event onPositionBlockStopMoveEvent;
    [SerializeField] private Vector3Event onStopMoveBlockEvent;
    [SerializeField] private Vector3Event onCheckBlockCollisionGate;
    [SerializeField] private BlockDirection blockDirection;
    [SerializeField] private ColorType colorType;
    [SerializeField] private float speedMove = 35f;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private BlockState blockState;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private Mesh gridMesh;
    [SerializeField] private LayerMask gridMask;
    [SerializeField] private OnCollisionGateEvent onCollisionGateEvent;
    private Vector3 _direction;
    private Vector3 _offsetMovement;
    private Vector3 _offsetPosEndMove;
    private Vector3 _offPivot;
    private Vector3 pivot;

    private void Start()
    {
        pivot = new Vector3(meshCollider.bounds.min.x+gridMesh.bounds.size.x/2,
            meshCollider.bounds.min.y+gridMesh.bounds.size.y/2, transform.position.z);
        Debug.Log(pivot);
        _offPivot = transform.position - pivot;
    }

    public Vector3 GetPivotBlock() => transform.position - _offPivot;

    private void FixedUpdate()
    {
        if (blockState == BlockState.Move)
        {
            var moveDirection = _direction - GetPositionInputDown();
            var remainingDistance = moveDirection.magnitude;
            var maxDistanceThisFrame = speedMove * Time.deltaTime;

            if (remainingDistance <= maxDistanceThisFrame)
            {
                var slowDownFactor = Mathf.Clamp01(remainingDistance / maxDistanceThisFrame);
                rigidbody.velocity = moveDirection.normalized * (speedMove * slowDownFactor);
                if (remainingDistance < 0.01f)
                {
                    rigidbody.velocity = Vector3.zero;
                    blockState = BlockState.Stop;
                }
            }
            else
            {
                rigidbody.velocity = moveDirection.normalized * speedMove;
            }

            var rayPos = transform.position + _offPivot;
            Debug.DrawRay(rayPos, new Vector3(rayPos.x, rayPos.y, rayPos.z + 100), Color.yellow);
        }
    }

    public ColorType GetColorType() => colorType;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Gate"))
        {
            var gate = other.gameObject.GetComponent<Gate>();
            if (gate.IsCorrectGate(colorType))
            {
                switch (gate.GetGateInformation().gateDirection)
                {
                    case Direction.Left:
                        break;
                    case Direction.Right:
                        break;
                    case Direction.Bottom:
                        break;
                    case Direction.Top:
                        break;
                }
            }
        }
    }

    void OnShootRaycastCheckGate(Vector3 origin, Vector3 direction)
    {
        
    }

    public void OnStartMove(Vector3 startInputPos)
    {
        _offsetMovement = startInputPos - transform.position;
        onPositionBlockStopMoveEvent.OnRaised += OnPositionStopMove;
    }

    public void OnStopMove()
    {
        if (blockState != BlockState.Stop) blockState = BlockState.Stop;
        Debug.Log("dung");
        rigidbody.velocity = Vector3.zero;
        onStopMoveBlockEvent.Raise(transform.position-_offPivot);
    }

    void OnPositionStopMove(Vector3 currentSlotPos)
    {
        var offset =new Vector3(currentSlotPos.x,currentSlotPos.y,transform.position.z)- (transform.position - _offPivot);
        transform.position += offset;
        onPositionBlockStopMoveEvent.OnRaised -= OnPositionStopMove;
    }

    private Vector3 GetPositionInputDown()
    {
        return transform.position + _offsetMovement;
    }

    protected abstract void OnCheckColorBlock();

    public void OnUpdaetBlockPos(Vector3 getDirection)
    {
        if (blockState != BlockState.Move) blockState = BlockState.Move;
        _direction = getDirection;
    }
}

[Serializable]
public class BlockDirection
{
    public BlockDirection Up;
    public BlockDirection Down;
    public BlockDirection Left;
    public BlockDirection Right;
    
    [Serializable]
    public struct BlockDirectionInfor
    {
        public Direction blockDirection;
        public int rowLengthToCheck;
        public int columnLengthToCheck;
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