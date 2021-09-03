using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for controlling the player's movement.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// The camera of the game. It alwaycs follows the player.
    /// </summary>
    [SerializeField] private new GameObject camera;
    /// <summary>
    /// The objects which marks the positions where the weapons should be placed (the player's hand).
    /// </summary>
    [SerializeField] public GameObject weaponGrip;
    /// <summary>
    /// The speed of the player.
    /// </summary>
    [SerializeField] private float speed = 1f;
    /// <summary>
    /// The which marks the player's aim.
    /// </summary>
    [SerializeField] private GameObject sight;
    /// <summary>
    /// The AudioSource for playing th player's sounds.
    /// </summary>
    [SerializeField] private AudioSource audioSource;
    /// <summary>
    /// The Rigidbody component of the player.
    /// </summary>
    private new Rigidbody rigidbody;
    /// <summary>
    /// The Camera component of the camera.
    /// </summary>
    private Camera cameraComponent;
    /// <summary>
    /// The FollowPlayer component of the camera.
    /// </summary>
    private FollowPlayer cameraFollow;
    /// <summary>
    /// The input in horizontal axis (left/right or a/d arrow keys)
    /// </summary>
    private float horizontalInput;
    /// <summary>
    /// The input in the vertival axis (up/down or w/s arrow keys)
    /// </summary>
    private float verticalInput;
    /// <summary>
    /// Player's animator.
    /// </summary>
    private Animator animator;
    /// <summary>
    /// The player's collider.
    /// </summary>
    Collider collider;
    /// <summary>
    /// The player's health
    /// </summary>
    Health health;
    
    /// <summary>
    /// Initializes the player
    /// </summary>
    void Start()
    {
        GetTheComponents();
        cameraFollow.player = gameObject;
        health = GetComponent<Health>();
        audioSource.Play();
        audioSource.Pause();
    }

    /// <summary>
    /// Checks the player's input and sets velocity and rotation of the player's character, position of the camera, etc.
    /// </summary>
    void Update()
    {
        if(health.IsAlive()){
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            SetVelocity();
            cameraFollow.updatePosition();
            SetRotation();
            PlaceSight();
        }
        else
        {
            animator.SetTrigger("Die");
            rigidbody.velocity = Vector3.zero;
            collider.enabled = false;
        }
    }

    /// <summary>
    /// Finds the point pointed by the mouse there the player will shoot.
    /// </summary>
    /// <returns>the point pointed by the mouse there the player will shoot</returns>
    public Vector3 GetShootPoint()
    {
        Ray ray = cameraComponent.ScreenPointToRay(Input.mousePosition);
        Plane shootingPlane = new Plane(Vector3.up, transform.position);
        shootingPlane.Raycast(ray, out float enter);
        Vector3 lookAtPoint = ray.GetPoint(enter);
        return lookAtPoint;
    }

    /// <summary>
    /// Sets the velocity of the player.
    /// </summary>
    private void SetVelocity()
    {
        rigidbody.velocity = FindVelocity();
    }

    /// <summary>
    /// Sets the rotation of the player.
    /// The rotation is set so that the player is alwys rotated towards the point returned by GetShootPoint.
    /// </summary>
    private void SetRotation()
    {
        if (Time.deltaTime > 0)
        {
            Vector3 lookAtPoint = GetShootPoint();
            Vector3 forwardDirection = lookAtPoint - transform.position;
            transform.rotation = Quaternion.LookRotation(forwardDirection);
        }
    }

    /// <summary>
    /// Calculates the velocity of the player.
    /// The direction of the velocity vector is calculated according to the horizontalInput and verticalInput,
    /// the magnitude of the vector is equal to the speed field.
    /// </summary>
    /// <returns>the velocity of the player.</returns>
    private Vector3 FindVelocity()
    {
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        if (direction.magnitude > 1)
            direction.Normalize();
        if (direction.magnitude != 0){
            animator.SetBool("isWalking", true);
            audioSource.UnPause();
        }else{
            animator.SetBool("isWalking", false);
            audioSource.Pause();
        }
        Vector3 velocity = speed * direction;
        return velocity;
    }

    /// <summary>
    /// Sets the references to the player's components.
    /// </summary>
    private void GetTheComponents()
    {
        rigidbody = GetComponent<Rigidbody>();
        cameraComponent = camera.GetComponent<Camera>();
        cameraFollow = camera.GetComponent<FollowPlayer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    /// <summary>
    /// Places the object marking the player's aim.
    /// </summary>
    private void PlaceSight()
    {
        if (sight != null)
        {
            sight.transform.position = GetShootPoint();
        }
    }
}
