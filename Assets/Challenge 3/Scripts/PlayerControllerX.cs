using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver = false;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;

    private RepeatBackgroundX repeatBackgroundXscript;
    private Vector3 topBound;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        repeatBackgroundXscript = GameObject.Find("Background").GetComponent<RepeatBackgroundX>();
        

    }

    // Update is called once per frame
    void Update()
    {
        topBound = new Vector3(-3, repeatBackgroundXscript.topBound1, 0);
        
        // While space is pressed and player is low enough, float up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver && transform.position.y < repeatBackgroundXscript.topBound1)
        {            
            // Apply a small upward force at the start of the game
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }

        //The condition to set maximum player position on y axis when it exceed player view
        if (transform.position.y > repeatBackgroundXscript.topBound1)
        {
            transform.position = topBound;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

        //bounce player as it collides with ground and not gameover
        else if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
            playerAudio.PlayOneShot(bounceSound, 1.0f); 
        }

    }

}
