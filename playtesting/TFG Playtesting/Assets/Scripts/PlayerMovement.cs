using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    private float speed;
    public float moveSpeed = 20.0f;
    public float dashSpeed = 80.0f;

    private bool dashing = false;
    private bool canDash = true;
    private float dashCDTimer = 0f;
    public float dashCD = 1.0f;
    private float dashTimer = 0f;
    public float dashDuration = 0.2f;
    
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float maxHealth = 100f;
    public float playerHealth;
    private bool damaged = false;
    public float damageCD = 0.1f;
    private float damageCDTimer;

    private Vector3 spawnPoint;

    private void Start()
    {
        spawnPoint = this.gameObject.transform.position;
        playerHealth = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        if(dashCDTimer > 0) dashCDTimer -= Time.deltaTime;
        else canDash = true;

        if(dashTimer > 0) dashTimer -= Time.deltaTime;
        else dashing = false;

        if(damageCDTimer > 0) damageCDTimer -= Time.deltaTime;
        else damaged = false;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if(dashing) speed = dashSpeed;
        else speed = moveSpeed;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (canDash && Input.GetKeyDown(KeyCode.Space))
        {
            StartDash();
        }

        if (playerHealth <= 0f) Restart();
    }

    private void StartDash()
    {
        dashing = true;
        canDash = false;
        dashTimer = dashDuration;
        dashCDTimer = dashCD;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            Destroy(col.gameObject);
            if (!damaged) DoDamage();
        }
    }

    private void DoDamage()
    {
        damaged = true;
        damageCDTimer = damageCD;
        playerHealth -= 10f;
    }

    void Restart()
    {
        this.gameObject.transform.position = spawnPoint;
        playerHealth = maxHealth;        
    }

}
