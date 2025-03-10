using UnityEngine;
using VirtueSky.TouchInput;

public class TouchController : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float distanceToMove;
    [SerializeField] private InputEventTouchBegin inputEventTouchBegin;
    [SerializeField] private InputEventTouchMove inputEventTouchMove;
    [SerializeField] private InputEventTouchEnd inputEventTouchEnd;
    [SerializeField] private InputEventTouchCancel inputEventTouchCancel;
    private BaseBlockPuzzle _currentSelectBlock;
    private float _currentTime;
    private Vector3 _prevPos;
    private float _prevTime;

    private void OnEnable()
    {
        inputEventTouchBegin.OnRaised += OnTouchBegin;
        inputEventTouchMove.OnRaised += OnTouchMove;
        inputEventTouchEnd.OnRaised += OnTouchEnd;
        inputEventTouchCancel.OnRaised += OnTouchEnd;
    }

    private void OnDisable()
    {
        inputEventTouchBegin.OnRaised -= OnTouchBegin;
        inputEventTouchMove.OnRaised -= OnTouchMove;
        inputEventTouchEnd.OnRaised -= OnTouchEnd;
        inputEventTouchCancel.OnRaised -= OnTouchEnd;
    }

    private void OnTouchBegin(Vector3 pos)
    {
        HandlePoint(pos);
    }

    private void OnTouchMove(Vector3 pos)
    {
        if (_currentSelectBlock != null)
        {
            var direction =
                camera.ScreenToWorldPoint(new Vector3(pos.x, pos.y, Mathf.Abs(camera.transform.position.z)));
            _currentSelectBlock.OnUpdateBlockPos(new Vector3(direction.x, direction.y,
                _currentSelectBlock.transform.position.z));
        }
    }

    private void OnTouchEnd(Vector3 pos)
    {
        if (_currentSelectBlock)
        {
            Debug.Log("end");
            _currentSelectBlock.OnStopMove();
            _currentSelectBlock = null;
        }
    }

    private void HandlePoint(Vector3 direction)
    {
        var ray = camera.ScreenPointToRay(direction);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
        RaycastHit raycastHit;
        Physics.Raycast(ray.origin, ray.direction, out raycastHit, Mathf.Infinity);
        if (raycastHit.collider != null)
            if (raycastHit.collider.gameObject.CompareTag("Block"))
            {
                _currentSelectBlock = raycastHit.collider.GetComponent<BaseBlockPuzzle>();
                var pos = camera.ScreenToWorldPoint(new Vector3(direction.x, direction.y,
                    Mathf.Abs(camera.transform.position.z)));
                _currentSelectBlock.OnStartMove(new Vector3(pos.x, pos.y, _currentSelectBlock.transform.position.z));
                _prevPos = direction;
            }
    }
}