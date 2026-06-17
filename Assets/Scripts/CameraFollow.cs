using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _camFollow;
    private Transform _ball, _win;

    private void Awake()
    {
        _ball = FindFirstObjectByType<Ball>().transform;
    }

    private void Update()
    {
        if (_win == null)
        {
            _win = GameObject.Find("Win(Clone)").GetComponent<Transform>();

            if (transform.position.y > _ball.transform.position.y && transform.position.y > _win.position.y + 4f)
            {
                _camFollow = new Vector3(transform.position.x, _ball.position.y, transform.position.z);
            }

            transform.position = new Vector3(transform.position.x, _camFollow.y, -5);
        }
    }
}