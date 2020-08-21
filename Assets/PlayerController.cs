using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public bool hasPowerup;
    public bool hasDebuff;
    public float enemyForce = 5;
    //private GameObject focalPoint;

    private Rigidbody rig;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;
    private float initialDrag = .05f;
    private Coroutine storedPowerupCoroutine = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            if (storedPowerupCoroutine != null)
            {
                StopCoroutine(storedPowerupCoroutine);
            }
            storedPowerupCoroutine = StartCoroutine(PowerupCountdownRoutine());
        }
        // if (other.CompareTag("Debuff"))
        //{
        //    hasDebuff = true;
        //    rig.mag = rig.drag + 1.0f;
        //    //TODO Debuff indicator
        //    //powerupIndicator.gameObject.SetActive(true);
        //   // Destroy(other.gameObject);
        //    StartCoroutine(PowerupCountdownRoutine());

        //}
    }


    //TODO debuff countdown reoutine
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Rigidbody playerRigidbody = gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;
            Vector3 playerAway = transform.position - other.gameObject.transform.position;

            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength
            {
                playerRigidbody.AddForce(playerAway * powerupStrength, ForceMode.Impulse);
            }


        }
    }
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        float leftInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.D))

        {
            Vector3 bounce = new Vector3(-1, 1, 0);
            rig.AddForce(bounce * speed, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 bounce = new Vector3(1, 1, 0);
            rig.AddForce(bounce * speed, ForceMode.Impulse);
        }

        powerupIndicator.transform.position = transform.position + new Vector3(0, 0.5f, 0);
    }
}
