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
            GameObject winObj = GameObject.Find("Win(Clone)");
            if (winObj != null)
            {
                _win = winObj.GetComponent<Transform>();
            }
        }
        
        if (_win != null && transform.position.y > _ball.transform.position.y && transform.position.y > _win.position.y + 4f)
        {
            _camFollow = new Vector3(transform.position.x, _ball.position.y, transform.position.z);
        }
        else if (_win == null && transform.position.y > _ball.transform.position.y)
        {
            _camFollow = new Vector3(transform.position.x, _ball.position.y, transform.position.z);
        }

        transform.position = new Vector3(transform.position.x, _camFollow.y, -5);
    }
}