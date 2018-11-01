using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private Vector3 _targetPosition;

    private void Start()
    {
        _targetPosition = _player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player.position = _targetPosition;
    }
}
