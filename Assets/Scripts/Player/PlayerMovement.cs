using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private new GameObject camera;
    [SerializeField] public GameObject weaponGrip;
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameObject sight;
    private new Rigidbody rigidbody;
    private Camera cameraComponent;
    private FollowPlayer cameraFollow;
    private float horizontalInput;
    private float verticalInput;
    
    void Start()
    {
        GetTheComponents();
        cameraFollow.player = gameObject;
    }

    
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        SetVelocity();
        cameraFollow.updatePosition();
        SetRotation();
        PlaceSight();
    }

    public Vector3 GetShootPoint()
    {
        Ray ray = cameraComponent.ScreenPointToRay(Input.mousePosition);
        Plane shootingPlane = new Plane(Vector3.up, transform.position);
        shootingPlane.Raycast(ray, out float enter);
        Vector3 lookAtPoint = ray.GetPoint(enter);
        return lookAtPoint;
    }


    private void SetVelocity()
    {
        rigidbody.velocity = FindVelocity();
    }

    private void SetRotation()
    {
        Vector3 lookAtPoint = GetShootPoint();
        Vector3 forwardDirection = lookAtPoint - transform.position;
        //Vector3 forwardDirection = lookAtPoint - weaponGrip.transform.position;
        transform.rotation = Quaternion.LookRotation(forwardDirection);
    }

    private Vector3 FindVelocity()
    {
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        if (direction.magnitude > 1)
            direction.Normalize();
        Vector3 velocity = speed * direction;
        return velocity;
    }


    private void GetTheComponents()
    {
        rigidbody = GetComponent<Rigidbody>();
        cameraComponent = camera.GetComponent<Camera>();
        cameraFollow = camera.GetComponent<FollowPlayer>();
    }

    private void PlaceSight()
    {
        if (sight != null)
        {
            sight.transform.position = GetShootPoint();
        }
    }
}
