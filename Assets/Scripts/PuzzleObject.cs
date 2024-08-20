using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] Vector3 displacement = new Vector3 (0,0,0);

    [SerializeField] public bool hasAlternateSolution;

    //bool lerpActive = false;


    //check if the object is at the pos it's supposed to be
    private void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Target")[0].transform;
        displacement = target.position;
        //targetPosition.position = piecePosition;
        //targetPosition.rotation = pieceRotation;

    }

    private void Update()
    {
        // transform.GetComponent<PuzzleMover>().StartCoroutine()
        //StartCoroutine(transform.GetComponent<PuzzleMover>().MoveToComplete(targets[0].targetPosition,5));
 

        atTarget = CheckIfAtTarget();
        atTargetPosition = CheckIfAtTargetPosition();
        atTargetRotation = CheckIfAtTargetRotation();



        //transform.GetComponent<PuzzleMover>().MoveToComplete(targets[0].adjustedTargetPosition, targets[0].targetRotation, 2);
    }

    private void FixedUpdate()
    {
        
    }

    [System.Serializable]
    public class Target
    {
        [SerializeField] public Vector3 targetPosition;
        [SerializeField] public Vector3 targetRotationVector;

        [Header("These auto adjust so don't worry")]
        //public string TheseAutoAdjustDontWorry;
        [Space]
        [SerializeField] public Vector3 adjustedTargetPosition;
        [SerializeField] public Quaternion targetRotation;

  

    }

    [SerializeField]
    public Target[] targets =
    {
        new Target()
    };


    [SerializeField]
    public Target[] alternateTargets =
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

            //POS displacement - adjust target
            //targets[x].adjustedTargetPosition.z = displacement.z + targets[x].targetPosition.z;
            targets[x].adjustedTargetPosition = displacement + targets[x].targetPosition;
            targets[x].targetRotation = Quaternion.Euler(targets[x].targetRotationVector.x, targets[x].targetRotationVector.y, targets[x].targetRotationVector.z);



            if ((this.transform.position - targets[x].adjustedTargetPosition).sqrMagnitude < allowedErrorMarginPosition &&
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
            targets[x].adjustedTargetPosition.z = displacement.z + targets[x].targetPosition.z;

            if ((this.transform.position - targets[x].adjustedTargetPosition).sqrMagnitude < allowedErrorMarginPosition)
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


    public bool CheckIfAtAlternateTarget()
    {
        bool isTrue = false;

        if (hasAlternateSolution)
        {
            
            for (int x = 0; x < alternateTargets.Length; x++)
            {


                //POS displacement - adjust target
                //alternateTargets[x].adjustedTargetPosition.z = displacement.z + alternateTargets[x].targetPosition.z;
                alternateTargets[x].adjustedTargetPosition = displacement + alternateTargets[x].targetPosition;
                alternateTargets[x].targetRotation = Quaternion.Euler(alternateTargets[x].targetRotationVector.x, alternateTargets[x].targetRotationVector.y, alternateTargets[x].targetRotationVector.z);


                Debug.Log(Quaternion.Angle(this.transform.rotation.normalized, alternateTargets[x].targetRotation.normalized));

            if ((this.transform.position - alternateTargets[x].adjustedTargetPosition).sqrMagnitude < allowedErrorMarginPosition &&
                Quaternion.Angle(this.transform.rotation.normalized, alternateTargets[x].targetRotation) < allowedErrorMarginRotation)
                {
                    if (debugColor)
                    {
                        this.GetComponent<Renderer>().material.color = Color.red;
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

        }

        return isTrue;

    }


}






