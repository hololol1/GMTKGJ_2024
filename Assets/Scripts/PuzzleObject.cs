using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PuzzleObject : MonoBehaviour
{
    //[SerializeField] GameObject puzzlePiece;

    //[SerializeField] public List<Target> targets = new List<Target>();


    //[SerializeField] Vector3 targetPosition;
    //[SerializeField] Quaternion targetRotation;




    [SerializeField] float allowedErrorMarginPosition = 5f;
    [SerializeField] float allowedErrorMarginRotation = 5f;


    //[SerializeField] Transform targetPosition ; //make this a variable range
    [SerializeField] bool atTargetPosition = false;
    [SerializeField] bool atTargetRotation = false;
    [SerializeField] bool atTarget = false;

    [SerializeField] bool debugColor = true;

    [SerializeField] Transform target;



    //check if the object is at the pos it's supposed to be
    private void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Target")[0].transform;
        //targetPosition.position = piecePosition;
        //targetPosition.rotation = pieceRotation;

    }

    private void Update()
    {
        atTarget = CheckIfAtTarget();
        atTargetPosition = CheckIfAtTargetPosition();
        atTargetRotation = CheckIfAtTargetRotation();
    }


    [System.Serializable]
    public class Target
    {
        [SerializeField] public Vector3 targetPosition;
        [SerializeField] public Quaternion targetRotation;
    }

    [SerializeField]
    public Target[] targets =
    {
        new Target()
    };


    public bool CheckIfAtTarget()
    {
        //Debug.Log(this.transform.rotation);
        //Debug.Log(targetRotation);

        bool isTrue = false;

        for (int x = 0; x < targets.Length; x++)
        {
            targets[x].targetPosition.z = target.position.z;
            if ((this.transform.position - targets[x].targetPosition).sqrMagnitude < allowedErrorMarginPosition &&
            Quaternion.Angle(this.transform.rotation.normalized, targets[x].targetRotation) < allowedErrorMarginRotation)
            {
                if (debugColor)
                {
                    this.GetComponent<Renderer>().material.color = Color.green;
                }

                isTrue = true;
            }

        }
        if (!isTrue)
        {
            if (debugColor)
            {
                this.GetComponent<Renderer>().material.color = Color.gray;
            }
        }

        return isTrue;

    }

    public bool CheckIfAtTargetPosition()
    {
        bool isTrue = false;

        for (int x = 0; x < targets.Length; x++)
        {
            if ((this.transform.position - targets[x].targetPosition).sqrMagnitude < allowedErrorMarginPosition)
            {
                isTrue = true;
            }

        }
        return isTrue;

    }

    public bool CheckIfAtTargetRotation()
    {
        bool isTrue = false;

        for (int x = 0; x < targets.Length; x++)
        {
            if (Quaternion.Angle(this.transform.rotation.normalized, targets[x].targetRotation) < allowedErrorMarginRotation)
            {
                isTrue = true;
            }

        }
        return isTrue;
    }



}






