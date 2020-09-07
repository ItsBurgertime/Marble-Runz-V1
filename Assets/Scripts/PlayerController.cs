using AudioVisualizer;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public bool hasPowerup;
    private bool deathEffectSpawned;
    public float enemyForce = 5;
    public float livesRemaining = 3;
    //private GameObject focalPoint;
    public float extraGravity;
    public AudioClip enemyImpactWithPowerup;
    public AudioClip enemyImpactWithoutPowerup;

    public AudioClip activatePowerup;
    //public AudioClip rollingOnTrack;


    private Rigidbody rig;
    private bool abilityOffCooldown =  true;
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
    private AudioSource playerAudio;
    private AudioSource audioSource;

    public GameObject gameOverUI;

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
    private void Start()
    {

        rig = GetComponent<Rigidbody>();
        playerStartPos = rig.transform.position;
        playerMr = transform.GetChild(1).GetComponent<MeshRenderer>();
        dissolveSphere = Instantiate(dissolveWhenDeadPrefab);
        dissolveSphere.gameObject.SetActive(false);
        playerAudio = GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1;


    }

    //TODO debuff countdown reoutine
    IEnumerator PowerupCountdownRoutine()
    {
        abilityOffCooldown = false;
        yield return new WaitForSeconds(3);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        abilityOffCooldown = true;
    }
    IEnumerator RespawnPlayerRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = playerStartPos;
        dissolveSphere.transform.position = playerStartPos;
        yield return new WaitForSeconds(2);
        dissolveSphere.gameObject.SetActive(false);
        deathEffectSpawned = false;
        playerMr.enabled = true;
        rig.isKinematic = false;
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
                playerAudio.PlayOneShot(enemyImpactWithPowerup, 1.0f);
            }
            else // if no powerup, hit enemy with normal strength
            {
                playerRigidbody.AddForce(playerAway * powerupStrength, ForceMode.Impulse);
                playerAudio.PlayOneShot(enemyImpactWithoutPowerup, 1.0f);
            }

        }
        if(other.gameObject.CompareTag("Finish"))
        {
            storedDeathCoroutine = StartCoroutine(RespawnPlayerRoutine());
            livesRemaining--;
            Debug.Log("colission with finish");
        }
    }


    private void Update()
    {

        // Ensures the extra gravity is only added when we're moving down
        if (rig.velocity.y < 0)
        {
            Vector3 l_newVelocity = rig.velocity;
            l_newVelocity.y -= extraGravity;    // add the extra gravity every frame we're moving down
            rig.velocity = l_newVelocity;
        }

        float leftInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.D))

        {
            Vector3 bounce = new Vector3(-2, 0, 0);
            rig.AddForce(bounce * speed, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 bounce = new Vector3(2, 0, 0);
            rig.AddForce(bounce * speed, ForceMode.Impulse);
        }
        float xPos = rig.transform.position.x;

        powerupIndicator.transform.position = transform.position + new Vector3(0, 0, 0);
        if (Input.GetKeyDown(KeyCode.Space) && abilityOffCooldown == true)
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            playerAudio.PlayOneShot(activatePowerup, 1.0f);
            if (storedPowerupCoroutine != null  )
            {
                StopCoroutine(storedPowerupCoroutine);
            }
            storedPowerupCoroutine = StartCoroutine(PowerupCountdownRoutine());
        }


        if (xPos < -10|| xPos > 10)
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

            if (livesRemaining == 0)
            {

                GameOver();
            }
        }
    }
}
