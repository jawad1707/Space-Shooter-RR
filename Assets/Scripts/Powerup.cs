using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private int _powerUpId;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            switch(_powerUpId)
            {
                case 0:
                    player.TripleShotActivation();
                    break;
                case 1:
                    player.SpeedActivation();
                    break;
                case 2:
                    player.ShieldActivision();
                    break;
                default:
                    Debug.Log("This is default");
                    break;
            }

            Destroy(this.gameObject);
        }
    }
}
