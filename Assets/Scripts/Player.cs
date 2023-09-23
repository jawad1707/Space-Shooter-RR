using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private Transform _laserPrefab;
    [SerializeField]
    private Transform _tripleshotPrefab;
    [SerializeField]
    private float _cooldown = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private int _score;
    private UIManager _uimanager;
    [SerializeField]
    private AudioClip _laserShot;
    [SerializeField]
    private AudioClip _powerUpAudioClip;
    private AudioSource _playerAudioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-0.02f, -3.73f, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _playerAudioSource = GetComponent<AudioSource>();

        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null");
        }

        if(_uimanager == null)
        {
            Debug.LogError("The UI Manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape is pressed");
            Application.Quit();
        }

    }


    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 navigation = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(_speed * Time.deltaTime * navigation);
        
        if(_isSpeedActive == false)
        {
            _speed = 5f;
        }

        else
        {
            _speed = 10f;
        }

        if (transform.position.y <= -3.74f)
        {
            transform.position = new Vector3(transform.position.x, -3.74f, 0);
        }

        else if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }

        if (transform.position.x <= -9f)
        {
            transform.position = new Vector3(9f, transform.position.y, 0);
        }

        else if (transform.position.x >= 9f)
        {
            transform.position = new Vector3(-9f, transform.position.y, 0);
        }

    }

    void FireLaser()
    {

        _canFire = Time.time + _cooldown;

        //Debug.Log("Space key is being pressed");
        if (_isTripleShotActive == false)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        
        }

        else
        {
            Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);
        }

        _playerAudioSource.clip = _laserShot;

        _playerAudioSource.Play();

    }

    public void Damage()
    {
        if(_isShieldActive == true)
        {
            _shieldVisualizer.SetActive(false);
            _isShieldActive = false;
            return;
        }

        else
        {
            _lives--;
            if(_lives == 2)
            {
                _rightEngine.SetActive(true);
            }
            else
            {
                _leftEngine.SetActive(true);
            }
            _uimanager.UpdateLives(_lives);
        }
        

        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            _uimanager.GameOver();
            _uimanager.RestartGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("EnemyLaser"))
        {
            Damage();
            Destroy(other.gameObject);
        }
    }

    public void AddPoint(int point)
    {
        _score += point;
        _uimanager.UpdateScore(_score);

    }

    public void TripleShotActivation()
    {
        _playerAudioSource.clip = _powerUpAudioClip;
        _playerAudioSource.Play();
        _isTripleShotActive = true;
        StartCoroutine(TripleShotRoutine());
    }

    public void SpeedActivation()
    {
        _playerAudioSource.clip = _powerUpAudioClip;
        _playerAudioSource.Play();
        _isSpeedActive = true;
        StartCoroutine(SpeedRoutine());
    }

    public void ShieldActivision()
    {
        _playerAudioSource.clip = _powerUpAudioClip;
        _playerAudioSource.Play();
        _isShieldActive = true;
        StartCoroutine(ShieldRoutine());
    }

    IEnumerator TripleShotRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
                 
    }

    IEnumerator SpeedRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedActive = false;
    }

    IEnumerator ShieldRoutine()
    {
        _shieldVisualizer.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        _isShieldActive = false;
        _shieldVisualizer.SetActive(false);
    }
}
