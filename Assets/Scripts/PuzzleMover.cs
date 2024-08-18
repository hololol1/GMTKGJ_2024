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
    Vector3 startMousePosition;



    Plane plane =new Plane(Vector3.down, -2);

    private Vector3 startPosition;
    private Vector3 objPosition;

    [SerializeField] Collider movementSpace;
    [SerializeField] Collider movementSpaceTargetNow;

    public bool selected = false;

    public LayerMask layerMask;

    public LayerMask layerMaskSelf;

    Rigidbody rb;



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
        //Rotation();
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
            //DragObject();
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
        //left or right mouse button are pressed down 
        //Select the object if there is one and set velocity to 0
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
        {
            SelectObject();

            //stop all movement when the mouse is clicked and an this object selected
            if (selected)
            {
                //Stop Moving/Translating
                rb.velocity = Vector3.zero;

                //Stop rotating
                rb.angularVelocity = Vector3.zero;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
            //when left mouse down move object
            dragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {

            //when left mouse up no longer move object
            dragging = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            SelectObject();
            //when right down start rotating
            rotating = true;

            #region OldCode
            //save starting mouse pos
            //startMousePositionX = Input.mousePosition.x;
            //startMousePositionY = Input.mousePosition.y;
            //startMousePosition = Input.mousePosition;
            #endregion OldCode
        }

        if (Input.GetMouseButtonUp(1))
        {
            //no longer rotating manually
            rotating = false;
            //apply rotation according to prior movement amount
            rb.AddTorque((Input.GetAxis("Mouse Y") * dragAmount * Time.deltaTime), -(Input.GetAxis("Mouse X") * dragAmount * Time.deltaTime), 0);

            #region OldCode

            //Vector3 mouseMovDistance = (startMousePosition - Input.mousePosition);
            //mouseMovDistance = new Vector3(mouseMovDistance.y, mouseMovDistance.x, mouseMovDistance.z);
            //compare starting mouse pos to current pos to determine amount of rotation
            // float mouseMovement = Vector3.Distance(startMousePosition, Input.mousePosition);

            //rb.AddTorque(mouseMovDistance * dragAmount * Time.deltaTime);
            // rb.AddTorque(Vector3.down * mouseMovementX * dragAmount * Time.deltaTime);
            //rb.AddTorque(Vector3.right * mouseMovementY * dragAmount * Time.deltaTime);

            //mouseMovementX = 0;
            //mouseMovementY = 0;
            //startMousePositionX = 0;
            //startMousePositionY = 0;

            #endregion OldCode

        }

        //as long as this object is selected
        if (selected)
        {

            //if rotating do rotation
            if (rotating)
            {
                Rotation();
                /*
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
                */
            }

            //if dragging move object
            if (dragging)
            {
                DragObject();
            }

        }
    } 


    public void Rotation()
    {

        /*
        float keyX = Input.GetAxisRaw("Horizontal");
        float keyY = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        float angleSpeed = rotationSpeed * Time.deltaTime;

        Quaternion keyRotation = Quaternion.AngleAxis(angleSpeed * keyX, Vector3.up) * Quaternion.AngleAxis(angleSpeed * keyY, Vector3.right);
        Quaternion mouseRotation = Quaternion.AngleAxis(angleSpeed * mouseX, this.transform.up) * Quaternion.AngleAxis(angleSpeed * mouseY, this.transform.right);

        transform.rotation = keyRotation * mouseRotation * transform.rotation;
        */

        transform.Rotate((Input.GetAxis("Mouse Y")* rotationSpeed *Time.deltaTime), -(Input.GetAxis("Mouse X")* rotationSpeed * Time.deltaTime), 0 , Space.World);
    
    }





    public void SelectObject()
    {

        Vector3 mousePosition = Input.mousePosition;
        Ray objRayPosition = Camera.main.ScreenPointToRay(mousePosition);

        //raycast on selectable layer
        if (Physics.Raycast(objRayPosition, out RaycastHit hitData, Mathf.Infinity, layerMaskSelf, QueryTriggerInteraction.Collide))
        {
            if (hitData.transform == transform)              
            {
                Debug.Log("Selection Raycast hit " + hitData.transform);
                Debug.Log("trying to select");
                //deselect old
                if (GameManager.instance.selectedObject != null)
                {
                    if (GameManager.instance.selectedObject.TryGetComponent<PuzzleMover>(out PuzzleMover oldSelection))
                    {
                        oldSelection.selected = false;                      
                        GameManager.instance.selectedObjectCollider.enabled = true;

                    }
                }
                //select new
                Debug.Log("Selected " + transform);
                GameManager.instance.selectedObject = this.transform;
                GameManager.instance.selectedObjectCollider = this.transform.GetComponent<Collider>();

                this.selected = true;

            }
            //if nothing to select
            else
            {
               if (selected)
                {
                    Debug.Log("Deselcted " + transform);
                    GameManager.instance.selectedObjectCollider.enabled = true;
                    GameManager.instance.selectedObject = null;
                    selected = false;
                    this.transform.GetComponent<Collider>();
                }
            }
        }
        else
        {
            if (selected)
            {
                Debug.Log("Deselcted " + transform);
                GameManager.instance.selectedObjectCollider.enabled = true;
                GameManager.instance.selectedObject = null;
                selected = false;
                this.transform.GetComponent<Collider>();
            }
        }
    }

    private void DeselectObject()
    {

    }

    private void DragObject()
    {
        GameManager.instance.selectedObjectCollider.enabled = false;
        //Debug.Log("tryingDrag");
        
        //col.enabled = false;

        Vector3 mousePosition = Input.mousePosition;
        Ray objRayPosition = Camera.main.ScreenPointToRay(mousePosition);
    

        //if (Physics.Raycast(objRayPosition, out RaycastHit hitData, layerMask))
        if (Physics.Raycast(objRayPosition, out RaycastHit hitData, Mathf.Infinity,layerMask, QueryTriggerInteraction.Collide))
        {
            
            
            //Debug.Log(hitData.transform.gameObject);
            //Debug.Log("onjpos"+ hitData.point);
            //objPosition = hitData.point;
            if (hitData.collider.tag == "Target")
            {
                objPosition = hitData.point;
                //objPosition = hitData.point;
            }

        }
        //objPosition = Vector3.ClampMagnitude(objPosition, 100);
        //objPosition.z = startPosition.z;


        //POS!

        transform.position = objPosition;


        //transform.position = new Vector3(objPosition.x, objPosition.y, objPosition.z);
        //Debug.Log(transform.position);
        GameManager.instance.selectedObjectCollider.enabled = true;
        //col.enabled = true;
    }
}
