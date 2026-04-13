using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform right;
    public Transform down;
    public Transform left;
    public Transform up;
    public Text timer;
    
    public GameOverScreen GameOverScreen;

    private bool inside = false;
    private float timeLeft = 60.0f;
    private float stepTimer = 0f;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        timer.text = "Time Limit: 60s";

        if (ThemesAndSounds.Instance == null) {
            GameObject audioObj = new GameObject("ThemesAndSounds");
            audioObj.AddComponent<ThemesAndSounds>();
        }

        // Dynamically instantiate the hazard spawner
        if (FindObjectOfType<ObstacleSpawner>() == null) {
            GameObject spawnerObj = new GameObject("ObstacleSpawner");
            spawnerObj.AddComponent<ObstacleSpawner>();
        }

        // Try to find the existing 3D humanoid robot animator to make it walk realistically
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = false;

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            isWalking = true;
        }
        if (Input.GetKey(KeyCode.S))
        { 
            transform.Translate(-1 * Vector3.forward * Time.deltaTime * speed);
            isWalking = true;
        }
        if (Input.GetKey(KeyCode.A)) { 
            transform.Rotate(0, -0.5f, 0);
            isWalking = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0.5f, 0);
            isWalking = true;
        }

        if (_animator != null) {
            // Send the walking speed to the robot animator!
            _animator.SetFloat("Speed", isWalking ? speed : 0f);
            _animator.SetFloat("MotionSpeed", 1f);
        }

        if (isWalking) {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f) {
                if (ThemesAndSounds.Instance != null) ThemesAndSounds.Instance.PlayStep();
                stepTimer = 0.4f;
            }
        } else {
            stepTimer = 0f;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ThemesAndSounds.Instance != null) ThemesAndSounds.Instance.PlayJump();
            if (_animator != null) _animator.SetBool("Jump", true);
        }
        
        // Basic reset for jump trigger on the robot
        if (!Input.GetKey(KeyCode.Space) && _animator != null) {
            _animator.SetBool("Jump", false);
        }

        if(inside) {
            timeLeft -= Time.deltaTime;
            int seconds = (int)timeLeft;
            timer.text = "Time Left: " + seconds + "s";
            if (timeLeft <= 0) {
                GameOver();
            }
        }
        else {
            timer.text = "Time Limit: 60s";
            timeLeft = 60.0f;
        }

        if(right.position.x > transform.position.x && down.position.z < transform.position.z && left.position.x < transform.position.x && up.position.z > transform.position.z )
        {
            if(!inside) {
                Debug.Log("start");
                if (ThemesAndSounds.Instance != null) ThemesAndSounds.Instance.PlayCoin();
            }
            inside = true;
        }
        else {
            if(inside && up.position.z < transform.position.z) {
                Debug.Log("game over");
                GameOver();
            }
            inside = false;
            timeLeft = 60.0f;
        }
    }

    public void GameOver() {
        if (ThemesAndSounds.Instance != null) ThemesAndSounds.Instance.PlayWin();
        GameOverScreen.Setup((int)timeLeft);
    }
}
