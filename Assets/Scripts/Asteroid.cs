using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotatespeed;
    [SerializeField]
    private Transform _explosionPrefab;
    private SpawnManager _spawnManger;


    // Start is called before the first frame update
    void Start()
    {
        _spawnManger = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();
 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        transform.Rotate(_rotatespeed * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManger.StartSpawning();
            Destroy(this.gameObject,0.25f);
  
        }
            
    }
}
