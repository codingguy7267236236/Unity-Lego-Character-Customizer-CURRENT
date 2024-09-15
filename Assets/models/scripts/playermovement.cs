using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public GameObject currentCharacter;

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    public float maxSpeed = 12f;
    float speed;

    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundmask;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool isgrounded;
    bool running;

    character character;
    
    // Start is called before the first frame update
    void Start()
    {
        character = transform.GetComponent<character>();
        running = false;
        speed = maxSpeed;
        controller = transform.GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        // getting the figure
        currentCharacter = character.figure;
        // animator thing
        Animator animate = currentCharacter.GetComponent<Animator>();
        isgrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);
        if(isgrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            if(running != true)
            {
                animate.SetBool("walking", true);
                speed = maxSpeed;
            }
            else
            {
                animate.SetBool("running", true);
                speed = maxSpeed * 3;
            }
            //atan2 calculates an angle between x and y but x goes first for clockwise
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }
        else
        {
            if(running != true)
            {
                animate.SetBool("walking", false);
            }
            else
            {
                animate.SetBool("running", false);
            }
        }

        

        //checks playing jumping and is grounded
        if(Input.GetButtonDown("Jump") && isgrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(running == false)
            {
                running = true;
                animate.SetBool("walking", false);
            }

            else
            {
                running = false;
                animate.SetBool("running", false);
            }
        }


    }

    


    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void opencustomer()
    {
        pauseMenuUI.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void closeCustomiser()
    {
        pauseMenuUI.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void spaceTravel()
    {
        navigation.loadscene("space2");
    }
}
