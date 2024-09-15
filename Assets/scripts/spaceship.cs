using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class spaceship : MonoBehaviour
{
    [Header("===Ship Movement===")]
    [SerializeField] private float yawTorgue = 500f;
    [SerializeField] private float pitchTorgue = 1000f;
    [SerializeField] private float rollTorgue = 1000f;
    [SerializeField] private float thrust = 100f;
    [SerializeField] private float upThrust = 50f;
    [SerializeField] private float strafeThrust = 50f;
    [SerializeField] private float glide, verticalglide, horizontalglide = 0f;

    [SerializeField, Range(0.001f,0.999f)] private float thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)] private float updownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)] private float leftrightGlideReduction = 0.11f;


    //input values
    private float thrust1D;
    private float upDown1D;
    private float strafe1D;
    private float roll1D;
    private Vector2 pitchYaw;


    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        //roll
        rb.AddRelativeTorque(Vector3.back * roll1D * rollTorgue * Time.deltaTime);
        //pitch
        rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * pitchTorgue * Time.deltaTime);
        //yaw
        rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * yawTorgue * Time.deltaTime);

        //thrust
        if(thrust1D>0.1f || thrust1D < -0.1f)
        {
            float currentThrust=thrust;
            rb.AddRelativeForce(Vector3.forward * thrust1D * currentThrust * Time.deltaTime);
            glide = thrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.forward * glide * Time.deltaTime);
            glide *= thrustGlideReduction;
        }

        //updown
        if (upDown1D > 0.1f || upDown1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.up * upDown1D * upThrust * Time.deltaTime);
            verticalglide = thrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.up * verticalglide * Time.deltaTime);
            verticalglide *= updownGlideReduction;
        }

        //starfing
        if (strafe1D > 0.1f || strafe1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.right * strafe1D * upThrust * Time.deltaTime);
            horizontalglide = thrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.right * horizontalglide * Time.deltaTime);
            glide *= leftrightGlideReduction;
        }
    }

    #region Input Methods
    public void OnThrust(InputAction.CallbackContext context)
    {
        thrust1D = context.ReadValue<float>();
    }

    public void OnStrafe(InputAction.CallbackContext context)
    {
       strafe1D = context.ReadValue<float>();
    }

    public void OnUpDown(InputAction.CallbackContext context)
    {
        upDown1D = context.ReadValue<float>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        roll1D = context.ReadValue<float>();
    }

    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>();
    }
    #endregion
}
