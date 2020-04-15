using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class doc : MonoBehaviour
{
    private float jumpForce = 10.0f;
    private float moveForce = 2f;
    private float maxSpeed = 8f;
    private float centerOfScreen;
    public bool isGrounded,moveLeft,moveRight;
    public bool isRunning = false;
    public Func<bool> onDocOutOFGroundListner;
    public Func<bool> onDocKilledCoronaListner;
    public DateTime waitTime = DateTime.Now;
    public DateTime virusTime = DateTime.Now;
    private Rigidbody2D rb;
    private Color virusColor;
    [SerializeField]
    Virus virus;


    public AudioSource jumpSound;

    public void callStart()
    {
        Start();
    }
    void Start()
    {
        jumpSound = GetComponent<AudioSource>();
        centerOfScreen = Screen.width / 2;
        rb = GetComponent<Rigidbody2D>();
        waitTime = DateTime.Now.AddSeconds(3);
        virusTime = DateTime.Now.AddSeconds(3);
        virusColor = virus.gameObject.GetComponent<Renderer>().material.color;
        var position = transform.localPosition;
        position.x = 0;
        position.y = 0;
        transform.localPosition = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        virusTime = DateTime.Now.AddSeconds(3);
        onDocKilledCoronaListner?.Invoke();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
        if (isRunning)
        {
            
            // If we have collided with the platform
            if (other.gameObject.name == "ground")
            {
                isGrounded = true;
                waitTime = DateTime.Now.AddSeconds(3);
               
            }
            else
            {
                print("collition detected : "+ other.gameObject.name);
            }
        }
    }

    void Update()
    {
        
        if (isRunning )
        {
            var timeleft = virusTime.Second - DateTime.Now.Second;
            var alpha = timeleft / 3f;
            if (alpha < 0)
            {
                onDocOutOFGroundListner.Invoke();
                isRunning = false;
                return;
            }
                
            //print(alpha);
            virusColor.a = alpha;
            virus.gameObject.GetComponent<Renderer>().material.color = virusColor;
            if (onDocOutOFGroundListner != null && waitTime < DateTime.Now)
            {
                onDocOutOFGroundListner.Invoke();
                isRunning = false;
                return;
            }
            if (Input.touchSupported && Input.touches.Length > 0 && Math.Abs(rb.velocity.x) < maxSpeed)
            {
                var touch = Input.GetTouch(0);
                if (touch.position.x > centerOfScreen)
                {
                    rb.AddForce(Vector3.right * moveForce, ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(Vector3.left * moveForce, ForceMode2D.Impulse);

                }
            }

            if (Math.Abs(rb.velocity.x) < maxSpeed)
            {
                
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    rb.AddForce(Vector3.right * moveForce, ForceMode2D.Impulse);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    
                    rb.AddForce(Vector3.left * moveForce, ForceMode2D.Impulse);
                }
            }

            if (isGrounded)
            {
                jumpSound.Play();
                // Add some force to our Rigidbody
                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }
        }
        
    }
}
