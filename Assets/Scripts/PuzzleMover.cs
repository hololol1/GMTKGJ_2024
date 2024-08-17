using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleMover : MonoBehaviour
{
    [SerializeField] bool horizontalMovement = false;
    [SerializeField] float rotationSpeed =1f;
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float dragAmount = 10f;
    bool dragging = false;
    bool rotating = false;
    private float startMousePositionX;
    private float startMousePositionY;
    float mouseMovementX;
    float mouseMovementY;
    Plane plane =new Plane(Vector3.down, -2);
    private Vector3 startPosition;
    private Vector3 objPosition;

    [SerializeField] Collider movementSpace;
    [SerializeField] Collider movementSpaceTargetNow;

    public bool selected = false;

    public LayerMask layerMask;


    Rigidbody rb;
    //check if object true when an object is moved


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = this.transform.position;
        plane = new Plane(Vector3.down, startPosition);
        if (horizontalMovement)
        {
            //plane = new Plane(Vector3.right, startPosition.x);           
        }
    }

    // Update is called once per frame
    void Update()
    {
        //SelectObject();
        MoveObject();
        //DragObject();
    }

    private void FixedUpdate()
    {
        //DragObject();
    }

    void OnMouseDrag()
    {
        if (selected)
        {
            DragObject();
        }


        /*
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z + transform.position.z);
        mousePosition = Input.mousePosition;
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Ray objRayPosition = Camera.main.ScreenPointToRay(mousePosition);

        if (plane.Raycast(objRayPosition, out float distance))
        {
            objPosition = objRayPosition.GetPoint(distance);
            objPosition = Vector3.ClampMagnitude(objPosition, 100);
        }
        //objPosition.z = startPosition.z;
        transform.position = objPosition;
        /*

        Vector3 mousePosition = Input.mousePosition;
        Ray objRayPosition = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(objRayPosition, out RaycastHit hitData))
        {
            objPosition = hitData.point;

        }
        //objPosition = Vector3.ClampMagnitude(objPosition, 100);
        //objPosition.z = startPosition.z;
        transform.position = objPosition;
        /*

        /*
        Vector3 mousePosition = Input.mousePosition;
        //mousePosition.z = Camera.main.nearClipPlane +1;
        mousePosition.z = Camera.main.nearClipPlane + 1;
        objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //objPosition.z = startPosition.z;
        transform.position = objPosition;
        */

        /*
        Vector3 mousePosition = Input.mousePosition;
        Ray objRayPosition = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(objRayPosition, out RaycastHit hitData))
        {
            Debug.Log(hitData);
            objPosition = hitData.point;
            if (hitData.collider == movementSpace)
            {
                objPosition = hitData.point;
            }


        }
        //objPosition = Vector3.ClampMagnitude(objPosition, 100);
        //objPosition.z = startPosition.z;
        transform.position = objPosition;
        */
        dragging = true;
       

    }

    public void MoveObject()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
        {
            SelectObject();
            //Stop Moving/Translating
            rb.velocity = Vector3.zero;

            //Stop rotating
            rb.angularVelocity = Vector3.zero;
        }

        if (selected)
        {
            
        

        //DragObject();


        if (Input.GetMouseButtonDown(0))
        {
            //GameManager.instance.selectedObject.TryGetComponent<>
            //DragObject();
            dragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }



        if (Input.GetMouseButtonDown(1))
        {
            rotating = true;
            startMousePositionX = Input.mousePosition.x;
            startMousePositionY = Input.mousePosition.y;
        }

        if (Input.GetMouseButtonUp(1))
        {
            rotating = false;

            rb.AddTorque(Vector3.down * mouseMovementX * dragAmount * Time.deltaTime);
            rb.AddTorque(Vector3.right * mouseMovementY * dragAmount * Time.deltaTime);
        }

        if (rotating)
        {
            float currentMousePositionX = Input.mousePosition.x;
            float currentMousePositionY = Input.mousePosition.y;
            mouseMovementX = currentMousePositionX - startMousePositionX;
            mouseMovementY = currentMousePositionY - startMousePositionY;

            if(Math.Abs(mouseMovementX) > Math.Abs(mouseMovementY))
            {
                transform.Rotate(Vector3.up, -mouseMovementX * rotationSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                transform.Rotate(Vector3.right, mouseMovementY * rotationSpeed * Time.deltaTime, Space.World);
            }
            }
        }

    } 

    public void SelectObject()
    {

        //Collider col = this.transform.GetComponent<Collider>();
        //col.enabled = false;
        Vector3 mousePosition = Input.mousePosition;
        Ray objRayPosition = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(objRayPosition, out RaycastHit hitData))
        {
            if(hitData.collider == transform.GetComponent<Collider>())
            {

                if (GameManager.instance.selectedObject != null)
                {
                    if (GameManager.instance.selectedObject.TryGetComponent<PuzzleMover>(out PuzzleMover oldSelection))
                    {
                        oldSelection.selected = false;                      
                        GameManager.instance.selectedObjectCollider.enabled = true;
                    }
                }
                GameManager.instance.selectedObject = this.transform;
                GameManager.instance.selectedObjectCollider= this.transform.GetComponent<Collider>();

                this.selected = true;

            }
        }

    }


    private void DragObject()
    {
        GameManager.instance.selectedObjectCollider.enabled = false;
        Debug.Log("tryingDrag");
        
        //col.enabled = false;

        Vector3 mousePosition = Input.mousePosition;
        Ray objRayPosition = Camera.main.ScreenPointToRay(mousePosition);
    

        //if (Physics.Raycast(objRayPosition, out RaycastHit hitData, layerMask))
        if (Physics.Raycast(objRayPosition, out RaycastHit hitData))
        {
            
            
            Debug.Log(hitData.transform.gameObject);
            Debug.Log("onjpos"+ hitData.point);
            //objPosition = hitData.point;
            if (hitData.collider.tag == "Target")
            {
                objPosition = hitData.point;
                //objPosition = hitData.point;
            }

        }
        //objPosition = Vector3.ClampMagnitude(objPosition, 100);
        //objPosition.z = startPosition.z;
        transform.position = objPosition;
        //transform.position = new Vector3(objPosition.x, objPosition.y, objPosition.z);
        Debug.Log(transform.position);
        GameManager.instance.selectedObjectCollider.enabled = true;
        //col.enabled = true;
    }
}
