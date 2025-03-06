
using UnityEngine;

public class BlockPuzzle : MonoBehaviour
{
    [SerializeField] private float speedMove;
    [SerializeField] private Rigidbody rigidbody;
    private Vector3 _currentDirection;
    private bool _isStartMove;


    public void OnStartMove()
    {
        _isStartMove = true;
    }

    public void OnStopMove()
    {
        _isStartMove = false;
        rigidbody.velocity = Vector3.zero;
    }

    private void LateUpdate()
    {
        if (_isStartMove)
        {
            if (Vector3.Distance(_currentDirection,transform.position)>=0.1f)
            {
                var dir = (_currentDirection - transform.position).normalized;
                rigidbody.velocity = dir * speedMove;
            }
            else
            {
                rigidbody.velocity = Vector3.zero;
            }
        }
    }

    public void OnUpdaetBlockPos(Vector3 direction)
    {
        _currentDirection = direction;
    }
}
