using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _smash;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) _smash = true;
        if (Input.GetMouseButtonUp(0)) _smash = false;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            _smash = true;
            _rb.linearVelocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
        }
        
        if (_rb.linearVelocity.y > 5) _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 5, _rb.linearVelocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_smash)
        {
            _rb.linearVelocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
        else
        {
            if (collision.gameObject.CompareTag("enemy"))
            {
                Destroy(collision.transform.parent.gameObject);
            }

            if (collision.gameObject.CompareTag("plane"))
            {
                Debug.Log("Game Over");
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!_smash || collision.gameObject.CompareTag("Finish"))
        {
            _rb.linearVelocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
    }
}
