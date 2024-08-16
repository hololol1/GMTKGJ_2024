using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    //[SerializeField] GameObject puzzlePiece;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] Quaternion targetRotation;
    [SerializeField] float allowedErrorMarginPosition = 5f;
    [SerializeField] float allowedErrorMarginRotation = 5f;
    //[SerializeField] Transform targetPosition ; //make this a variable range
    [SerializeField] bool atTargetPosition = false;
    [SerializeField] bool atTargetRotation = false;
    [SerializeField] bool atTarget = false;


    //check if the object is at the pos it's supposed to be
    private void Start()
    {
        //targetPosition.position = piecePosition;
        //targetPosition.rotation = pieceRotation;
    }

    private void Update()
    {
        atTarget = CheckIfAtTarget();
        atTargetPosition = CheckIfAtTargetPosition();
        atTargetRotation = CheckIfAtTargetRotation();
    }
    public bool CheckIfAtTarget()
    {
        //Method 2 using sqrmagnitude & Quaternion.angle
        //Use this one if you need more acurate result and control on objects
        //modify the 0.1f paramters for more accuracy and filtering
        Debug.Log(this.transform.rotation);
        Debug.Log(targetRotation);

        if ((this.transform.position - targetPosition).sqrMagnitude < allowedErrorMarginPosition &&
            Quaternion.Angle(this.transform.rotation.normalized, targetRotation) < allowedErrorMarginRotation)//Do confirm here if the vectors & quaternions are able to use equal comparer & result is satisfactory to your needs
        {
            this.GetComponent<Renderer>().material.color = Color.green;
            return true;

        }
        this.GetComponent<Renderer>().material.color = Color.gray;
        return false;

    }

    public bool CheckIfAtTargetPosition()
    {
        if ((this.transform.position - targetPosition).sqrMagnitude < allowedErrorMarginPosition )
        {
            return true;
        }
        return false;
    }

    public bool CheckIfAtTargetRotation()
    {
        if (Quaternion.Angle(this.transform.rotation.normalized, targetRotation) < allowedErrorMarginRotation)
        {
            return true;
        }

        return false;
    }

}
