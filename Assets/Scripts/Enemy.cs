using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private float _cooldown = -1f;
    private float _firerate;
    [SerializeField]
    private AudioClip _explosion;
    private AudioSource _enemyAudioSource;
    [SerializeField]
    private Transform _enemyLaserPrefab;
    [SerializeField]
    private Transform _enemyExplosionPrefab;

    private Player _player;

    private Animator _destroyAnim;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>().GetComponent<Player>();
        //_player = GameObject.Find("Player").GetComponent<Player>();
        _destroyAnim = GetComponent<Animator>();
        _enemyAudioSource = FindObjectOfType<AudioManager>().GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }

        FireLaser();
    }

    private void FireLaser()
    {
        if(Time.time > _cooldown)
        {
            _firerate = Random.Range(3f, 7f);
            _cooldown = _firerate + Time.time;
            Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);

        }

    }

    IEnumerator FireLaserRoutine()
    {
        yield return new WaitForSeconds((Random.Range(5, 10)));
        Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // my method -> other.transform.GetComponent<Player>().Damage();
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            _speed = 0;
            //_destroyAnim.SetTrigger("OnEnemyDeath");

            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            
            

            //_enemyAudioSource.clip = _explosion;
            //_enemyAudioSource.Play();


        }

        if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddPoint(10);
            }

            _speed = 0;
            //_destroyAnim.SetTrigger("OnEnemyDeath");

            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            

            //_enemyAudioSource.clip = _explosion;
            //_enemyAudioSource.Play();

            Destroy(GetComponent<Collider2D>());

            Destroy(this.gameObject);


        }

    }
}
