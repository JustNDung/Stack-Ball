using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody _rb;
    private float _currentTime;
    private bool _smash, _invincible;

    public enum BallState
    {
        Prepare,
        Playing,
        Died,
        Finish
    }
    
    [HideInInspector]
    public BallState ballState = BallState.Prepare;
    
    public AudioClip bounceOffClip, deadClip, winClip, destroyClip, iDestroyClip;
    
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
        if (ballState == BallState.Playing)
        {
            if (Input.GetMouseButtonDown(0)) _smash = true;
            if (Input.GetMouseButtonUp(0)) _smash = false;

            if (_invincible)
            {
                _currentTime -= Time.deltaTime * 0.35f;
            }
            else
            {
                if (_smash)
                {
                    _currentTime += Time.deltaTime * 0.8f;
                }
                else
                {
                    _currentTime -= Time.deltaTime * 0.5f;
                }
            }

            if (_currentTime >= 1)
            {
                _currentTime = 1;
                _invincible = true;
            }
            else if (_currentTime <= 0)
            {
                _currentTime = 0;
                _invincible = false;
            }
        }

        if (ballState == BallState.Prepare)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ballState = BallState.Playing;
            }
        }
        
        if (ballState == BallState.Finish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FindFirstObjectByType<LevelSpawner>().NextLevel();
            }
        }
    }

    private void FixedUpdate()
    {
        if (ballState == BallState.Playing)
        {
            if (Input.GetMouseButton(0))
            {
                _smash = true;
                _rb.linearVelocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
        }
        
        if (_rb.linearVelocity.y > 5) _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 5, _rb.linearVelocity.z);
    }

    public void IncreaseBrokenStacks()
    {
        if (!_invincible)
        {
            ScoreManager.Instance.AddScore(1);
            SoundManager.Instance.PlaySoundFX(destroyClip, 0.5f);
        }
        else
        {
            ScoreManager.Instance.AddScore(2);
            SoundManager.Instance.PlaySoundFX(iDestroyClip, 0.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_smash)
        {
            _rb.linearVelocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            SoundManager.Instance.PlaySoundFX(bounceOffClip, 0.5f);
        }
        else
        {
            if (_invincible)
            {
                if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("plane"))
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
            }
            else
            {
                if (collision.gameObject.CompareTag("enemy"))
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }

                if (collision.gameObject.CompareTag("plane"))
                {
                    Debug.Log("Game Over");
                    ScoreManager.Instance.ResetScore();
                    SoundManager.Instance.PlaySoundFX(deadClip, 0.5f);
                }
            }
        }

        if (collision.gameObject.CompareTag("Finish") && ballState == BallState.Playing)
        {
            ballState = BallState.Finish;
            SoundManager.Instance.PlaySoundFX(winClip, 0.7f);
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
