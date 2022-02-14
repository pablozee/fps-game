using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private float speed = 4f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpHeight = 3f;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private LayerMask groundMask;

    [SerializeField] private AudioSource footstepsAudioSource;
    [SerializeField] private float minVolume;
    [SerializeField] private float maxVolume;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;



    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift))
            speed = 8;

        if (!Input.GetKey(KeyCode.LeftShift))
            speed = 4;

        if (x > 0.01f || z > 0.01f || x < -0.01f || z < -0.01f)
        {
            if (!footstepsAudioSource.isPlaying && isGrounded)
            {
                footstepsAudioSource.volume = Random.Range(minVolume, maxVolume);
                footstepsAudioSource.pitch = Random.Range(minPitch, maxPitch);
                footstepsAudioSource.Play();
            }
        }

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
