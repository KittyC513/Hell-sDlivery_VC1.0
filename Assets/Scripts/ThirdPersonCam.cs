using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using static ThirdPersonMovement;

public class ThirdPersonCam : MonoBehaviour
{

    [Header("Orientation References")]
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public Transform orientation;
    [SerializeField]
    private float rotationSpeed;

    [Header("Movement References")]
    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference jumpControl;
    [SerializeField]
    private InputActionReference runControl;
    [SerializeField]
    private InputActionReference aimControl;

    [Header("Movement")]
    Vector3 move;
    [SerializeField]
    private float moveSpeed;

    [Header("Ground Check")]
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private float playerHeight;

    [Header("Camera Control")]
    public CameraStyle currentStyle;

    [SerializeField]
    private GameObject thirdPersonCam;
    [SerializeField]
    private GameObject combatCam;
    [SerializeField]
    private GameObject topDownCam;
    [SerializeField]
    public GameObject aimCursor;

    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }

    // we need to set up five player state
    public enum MovementState
    {
        freeze,
        walking,
        running,
        grappling,
        air
    }
    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        runControl.action.Enable();
        aimControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        runControl.action.Disable();
        aimControl.action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        //set the cursor to invisible and be locked
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        //freeze the rigidbody's rotation
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;


    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        MoveControl();
        CameraControl();
        
    }

    private void FixedUpdate()
    {
        Drag(20);
        SpeedControl();
    }

    #region Player Movement
    void MoveControl()
    {
        //calculate the direction from player.cam to player
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // access player input value
        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        move = new Vector3(movement.x, 0, movement.y);

        //do the rotation
        Vector3 inputDir = orientation.forward * move.z + orientation.right * move.x;
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, -inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        //do the movement
        rb.AddForce(-inputDir.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    #endregion

    #region Ground Check and Drag 
    void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.01f, groundLayer);
        Debug.Log("isGrounded" + isGrounded);
    }

    void Drag(float groundDrag)
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

    }
    #endregion
    
    #region Camera Control
    void CameraControl()
    {
        //switch camera by press button
        if (aimControl.action.IsPressed())
        {
            SwitchCameraStyle(CameraStyle.Combat);
            Debug.Log("combat camera");
        }
        else
        {
            SwitchCameraStyle(CameraStyle.Basic);
            Debug.Log("basic camera");
        }
    }
    void SwitchCameraStyle(CameraStyle newStyle)
    {
        //we want to switch the camera during the gameplay
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);
        aimCursor.SetActive(false);

        if (newStyle == CameraStyle.Basic)
        {
            thirdPersonCam.SetActive(true);
        }

        if (newStyle == CameraStyle.Combat)
        {
            combatCam.SetActive(true);
            aimCursor.SetActive(true);
        }

        if (newStyle == CameraStyle.Topdown)
        {
            topDownCam.SetActive(true);
        }

        currentStyle = newStyle;
    }
    #endregion
}

