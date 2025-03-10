using UnityEngine;

public abstract class BaseBlockPuzzle : MonoBehaviour
{
    [SerializeField] private ColorType colorType;
    [SerializeField] private int columnLength;
    [SerializeField] private int rowLength;
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
    private Vector3 _offsetRaycast;
    private Vector3 _raycastPosCheckMovement;

    private void Start()
    {
        _raycastPosCheckMovement = new Vector3(meshCollider.bounds.min.x + gridMesh.bounds.size.x / 2,
            meshCollider.bounds.max.y - gridMesh.bounds.size.y / 2, transform.position.z);
        _offsetRaycast = transform.position - _raycastPosCheckMovement;
    }

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

            var rayPos = transform.position + _offsetRaycast;
            Debug.DrawRay(rayPos, new Vector3(rayPos.x, rayPos.y, rayPos.z + 100), Color.yellow);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("GridSlot"))
        {
            var slot = other.gameObject.GetComponent<GridSlot>();
            // if (slot.IsCorrectGate(colorType))
            //     onCollisionGateEvent.Raise(new Tuple<BaseBlockPuzzle, int, int>(this, rowLength, columnLength));
        }
    }

    public void OnStartMove(Vector3 startInputPos)
    {
        _offsetMovement = startInputPos - transform.position;
    }

    public void OnStopMove()
    {
        if (blockState != BlockState.Stop) blockState = BlockState.Stop;
        Debug.Log("dung");
        rigidbody.velocity = Vector3.zero;
        ShootRaycastCheckGridEndMove();
        transform.position += _offsetPosEndMove;
    }

    private Vector3 GetPositionInputDown()
    {
        return transform.position + _offsetMovement;
    }

    private void ShootRaycastCheckGridEndMove()
    {
        var rayPos = transform.position + _offsetRaycast;
        Debug.DrawRay(rayPos, new Vector3(rayPos.x, rayPos.y, rayPos.z + 100), Color.yellow);
        RaycastHit hit;
        Physics.Raycast(rayPos, new Vector3(rayPos.x, rayPos.y, rayPos.z + 100), out hit, Mathf.Infinity, gridMask);
        {
            if (hit.collider != null)
            {
                var getHitPos = hit.collider.transform.position;
                _offsetPosEndMove = new Vector3(getHitPos.x, getHitPos.y, transform.position.z) - rayPos;
            }
        }
    }

    protected abstract void OnCheckColorBlock();

    public void OnUpdaetBlockPos(Vector3 getDirection)
    {
        if (blockState != BlockState.Move) blockState = BlockState.Move;
        _direction = getDirection;
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