using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    private Rigidbody _rb;
    private float _currentTime;
    private bool _smash, _invincible;
    private int _currentBrokenStacks, _totalStacks;

    public GameObject invincibleObj;
    public Image invincibleFill;
    public GameObject fireEffect, winEffect, splashEffect;

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
        _currentBrokenStacks = 0;
    }
    
    void Start()
    {
        StartCoroutine(DelayedInit());
    }

    private System.Collections.IEnumerator DelayedInit()
    {
        // Wait for LevelSpawner.Start() to create all stack objects first
        yield return new WaitForEndOfFrame();
        
        // Count all StackController instances - each one represents one stack platform
        _totalStacks = FindObjectsByType<StackController>(FindObjectsSortMode.None).Length;
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
                if (!fireEffect.activeInHierarchy) fireEffect.SetActive(true);
            }
            else
            {
                if (fireEffect.activeInHierarchy) fireEffect.SetActive(false);
                
                if (_smash)
                {
                    _currentTime += Time.deltaTime * 0.8f;
                }
                else
                {
                    _currentTime -= Time.deltaTime * 0.5f;
                }
            }
            
            if (_currentTime >= 0.3f || invincibleFill.color == Color.red) 
            {
                invincibleObj.SetActive(true);
            }
            else invincibleObj.SetActive(false);

            if (_currentTime >= 1)
            {
                _currentTime = 1;
                _invincible = true;
                invincibleFill.color = Color.red;
            }
            else if (_currentTime <= 0)
            {
                _currentTime = 0;
                _invincible = false;
                invincibleFill.color = Color.white;
            }
            
            if (invincibleObj.activeInHierarchy) invincibleFill.fillAmount = _currentTime / 1;
        }

        // if (ballState == BallState.Prepare)
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         ballState = BallState.Playing;
        //     }
        // }
        
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
        _currentBrokenStacks++;
        
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

            if (!collision.gameObject.CompareTag("Finish"))
            {
                GameObject splash = Instantiate(splashEffect, collision.transform);
                splash.transform.localEulerAngles = new Vector3(90, Random.Range(0, 359), 0);
                float randomScale = Random.Range(0.18f, 0.25f);
                splash.transform.localScale = new Vector3(randomScale, randomScale, 1);
                splash.transform.position = new Vector3(transform.position.x, transform.position.y - 0.22f,
                    transform.position.z);
                splash.GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;

            }
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
                    _rb.isKinematic = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    ballState = BallState.Died;
                    ScoreManager.Instance.ResetScore();
                    SoundManager.Instance.PlaySoundFX(deadClip, 0.5f);
                }
            }
        }
        
        if (_totalStacks > 0)
            FindFirstObjectByType<GameUI>().LevelSliderFill((float)_currentBrokenStacks / _totalStacks);

        if (collision.gameObject.CompareTag("Finish") && ballState == BallState.Playing)
        {
            ballState = BallState.Finish;
            SoundManager.Instance.PlaySoundFX(winClip, 0.7f);
            GameObject win = Instantiate(winEffect, Camera.main.transform);
            win.transform.localPosition = Vector3.up * 1.5f;
            win.transform.eulerAngles = Vector3.zero;
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
