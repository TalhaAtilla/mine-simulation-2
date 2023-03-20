using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cineMachine;
    
    [SerializeField]
    private bool useEdgeScrolling = false;
    
    [SerializeField] 
    private bool usedDragPan = false;

    [SerializeField]
    private float fieldOfViewMax=50;

    [SerializeField]
    private float fieldOfViewMin=10;
    [SerializeField]
    private float followOffSetMin=5f;
    [SerializeField]
    private float followOffSetMax=50f;


    private bool dragPanMoveActive;
    private Vector2 lastMousePosition;
    private float targetFieldOfView=50f;
    private Vector3 followOffSet;

    private void Awake() 
    {
        followOffSet=cineMachine.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset; 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        HandleCameraMovement();
        HandleCameraRotation();
        if(usedDragPan)
        {
            HandleCameraMovementDragPan();
        }
        if(useEdgeScrolling)
        {
            HandleCameraMovementEdgeScrolling();

        }
        HandleCameraZoomFieldOfView();
        HandleCameraZoomMoveForward();
    }

    private void HandleCameraRotation()
    {
        float rotateDir= 0f;
        if (Input.GetKey (KeyCode.E)) rotateDir= +1f;
        if (Input.GetKey (KeyCode.Q)) rotateDir= -1f;
        float rotateSpeed= 100f;
        transform.eulerAngles += new Vector3(0, rotateDir* rotateSpeed* Time.deltaTime, 0);
    }

    private void HandleCameraMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);
        if (Input.GetKey (KeyCode.W)) inputDir.z = +1f;
        if (Input.GetKey (KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey (KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey (KeyCode.D)) inputDir.x = +1f;

        
        Vector3 moveDir= transform.forward * inputDir.z + transform.right * inputDir.x;
        float moveSpeed = 50f;
        transform.position += moveDir* moveSpeed * Time.deltaTime;

        
    }

    private void HandleCameraMovementEdgeScrolling()
    {
        
        Vector3 inputDir = new Vector3(0, 0, 0);
        
        int edgeScrollSize = 20;
        if (Input.mousePosition.x < edgeScrollSize) {
            inputDir.x = -1f;
        }
        if (Input.mousePosition.y < edgeScrollSize) {
            inputDir.z = -1f;
        }
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) {
            inputDir.x = +1f;
        }
        if (Input.mousePosition.y > Screen.height-edgeScrollSize){
            inputDir.z = +1f; 
        }
        
        
        Vector3 moveDir= transform.forward * inputDir.z + transform.right * inputDir.x;
        float moveSpeed = 50f;
        transform.position += moveDir* moveSpeed * Time.deltaTime;
    }
    private void HandleCameraMovementDragPan()
    {
        
        Vector3 inputDir = new Vector3(0, 0, 0);
        if (Input.GetMouseButtonDown(1)) 
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp (1)) 
        {
            dragPanMoveActive = false;
        
        }

        if (dragPanMoveActive) 
        {
            Vector2 mouseMovementDelta = (Vector2) Input.mousePosition - lastMousePosition;
            float dragPanSpeed = 2f;
            inputDir.x = mouseMovementDelta.x* dragPanSpeed;
            inputDir.z = mouseMovementDelta.y* dragPanSpeed;
            lastMousePosition = Input.mousePosition;
            
        }
        
        Vector3 moveDir= transform.forward * inputDir.z + transform.right * inputDir.x;
        float moveSpeed = 50f;
        transform.position += moveDir* moveSpeed * Time.deltaTime;
    }

    private void HandleCameraZoomFieldOfView()
    {
        if(Input.mouseScrollDelta.y>0)
        {
            targetFieldOfView-=5;

        }
        if(Input.mouseScrollDelta.y<0)
        {
            targetFieldOfView+=5;

        }
        targetFieldOfView=Mathf.Clamp(targetFieldOfView,fieldOfViewMin,fieldOfViewMax);
        float zoomSpeed=10f;
        cineMachine.m_Lens.FieldOfView=Mathf.Lerp(cineMachine.m_Lens.FieldOfView,targetFieldOfView,Time.deltaTime*zoomSpeed);
        
    }

    private void HandleCameraZoomMoveForward()
    {
        Vector3 zoomDir=followOffSet.normalized;
        if(Input.mouseScrollDelta.y>0)
        {
            followOffSet+=zoomDir;
        }
        if(Input.mouseScrollDelta.y<0)
        {
            followOffSet-=zoomDir;
        }
        if(followOffSet.magnitude<followOffSetMin)
        {
            followOffSet=zoomDir*followOffSetMin;
        }
        if(followOffSet.magnitude>followOffSetMax)
        {
            followOffSet=zoomDir*followOffSetMax;
        }


        float zoomSpeed=10f;
        cineMachine.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset=Vector3.Lerp(followOffSet=cineMachine.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset,followOffSet,Time.deltaTime*zoomSpeed);
        
       
    }
}
