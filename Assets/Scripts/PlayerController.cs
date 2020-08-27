using AudioVisualizer;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public bool hasPowerup;
    public bool hasDebuff;
    private bool deathEffectSpawned;
    public float enemyForce = 5;
    public float livesRemaining = 3;
    //private GameObject focalPoint;

    private Rigidbody rig;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;
    public GameObject dissolveWhenDeadPrefab;
    private GameObject dissolveSphere;
    public GameObject playerPrefab;
    private float initialDrag = .05f;
    private Coroutine storedPowerupCoroutine = null;
    private Coroutine storedDeathCoroutine = null;
    private Vector3 playerStartPos;
    private MeshRenderer playerMr;




    //TODO debuff countdown reoutine
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);

    }
    IEnumerator RespawnPlayerRoutine()
    {
        yield return new WaitForSeconds(2);
        transform.position = playerStartPos;
        dissolveSphere.transform.position = playerStartPos;
        yield return new WaitForSeconds(2);
        dissolveSphere.gameObject.SetActive(false);
        deathEffectSpawned = false;
        playerMr.enabled = true;
        rig.isKinematic = false;
        Time.timeScale = 1.0f;
        storedDeathCoroutine = null;
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
        playerStartPos = rig.transform.position;
        playerMr = GetComponent<MeshRenderer>();
        dissolveSphere = Instantiate(dissolveWhenDeadPrefab);
        dissolveSphere.gameObject.SetActive(false);


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
        float xPos = rig.transform.position.x;
        powerupIndicator.transform.position = transform.position + new Vector3(0, 0, 0);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            if (storedPowerupCoroutine != null)
            {
                StopCoroutine(storedPowerupCoroutine);
            }
            storedPowerupCoroutine = StartCoroutine(PowerupCountdownRoutine());
        }
        if (xPos < -6 || xPos > 6)
        {
            Vector3 playerDeathPos = rig.transform.position;
            rig.isKinematic = true;
            playerMr.enabled = false;
            powerupIndicator.gameObject.SetActive(false);


            if (deathEffectSpawned == false)
            {
                dissolveSphere.transform.position = playerDeathPos;
                //Instantiate(dissolveWhenDead, playerDeathPos, dissolveWhenDead.transform.rotation);
                dissolveSphere.gameObject.SetActive(true);
                deathEffectSpawned = true;
            }
            if (livesRemaining > 0 && storedDeathCoroutine == null)
            {
                storedDeathCoroutine = StartCoroutine(RespawnPlayerRoutine());
                livesRemaining--;
            }
            else
            {
                Debug.Log("Game Over");
            }
        }
    }
}
