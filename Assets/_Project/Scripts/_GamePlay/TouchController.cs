using UnityEngine;
using VirtueSky.TouchInput;
public class TouchController : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private InputEventTouchBegin inputEventTouchBegin;
    [SerializeField] private InputEventTouchMove inputEventTouchMove;
    [SerializeField] private InputEventTouchEnd inputEventTouchEnd;
    private BlockPuzzle _currentSelectBlock;

    private void OnEnable()
    {
        inputEventTouchBegin.OnRaised += OnTouchBegin;
        inputEventTouchMove.OnRaised += OnTouchMove;
        inputEventTouchEnd.OnRaised += OnTouchEnd;
    }

    private void OnDisable()
    {
        inputEventTouchBegin.OnRaised -= OnTouchBegin;
        inputEventTouchMove.OnRaised -= OnTouchMove;
        inputEventTouchEnd.OnRaised -= OnTouchEnd;
    }

    void OnTouchBegin(Vector3 pos)
    {
        HandlePoint(pos);
    }

    void OnTouchMove(Vector3 pos)
    {
        if (_currentSelectBlock!=null) 
        {
            var direction = camera.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 19.6f));
            _currentSelectBlock.OnUpdaetBlockPos(new Vector3(direction.x, direction.y,_currentSelectBlock.transform.position.z));
        }
    }

    void OnTouchEnd(Vector3 pos)
    {
        if (_currentSelectBlock)
        {
            Debug.Log("end");
            _currentSelectBlock.OnStopMove();
            _currentSelectBlock = null;
        }
    }

    void HandlePoint(Vector3 direction)
    {
        Ray ray = camera.ScreenPointToRay(direction);
        Debug.DrawRay(ray.origin, ray.direction*100, Color.yellow);
        RaycastHit raycastHit;
            Physics.Raycast(ray.origin, ray.direction,out raycastHit,Mathf.Infinity);
        if (raycastHit.collider!=null)
        {
            if (raycastHit.collider.gameObject.CompareTag("Block"))
            {
                _currentSelectBlock = raycastHit.collider.GetComponent<BlockPuzzle>();
                _currentSelectBlock.OnStartMove();
            }
        }
    }


}
