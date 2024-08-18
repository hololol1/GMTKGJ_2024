using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager instance;

    public int currentPuzzle = 0;
    [SerializeField] GameObject puzzleContainer;
    [SerializeField] bool currentPuzzleComplete = false;
    [SerializeField] public List<GameObject> placedPuzzlePieces = new List<GameObject>();

    public Transform selectedObject;
    public Collider selectedObjectCollider;

    [SerializeField] Transform target;

    private void Awake()
    {
        #region Singleton
        //Start Singleton
        if (instance != null)
        {
            Debug.LogWarning("More than one Instance of Inventory found!");
            return;
        }
        instance = this;
        //End Singleton
        #endregion Singleton
    }

        // Start is called before the first frame update
        void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Target")[0].transform;
        LoadPuzzle(0);
    }

    // Update is called once per frame
    void Update()
    {
        
        currentPuzzleComplete = CheckIfComplete();
        if (currentPuzzleComplete)
        {
            StartCoroutine(WaitWinTimer(2));
           // NextPuzzle();
        }
    }

    [System.Serializable]
    public class Puzzle
    {
        [SerializeField] public GameObject[] puzzlePieces;


    }

    [SerializeField]
    public Puzzle[] puzzle =
    {
        new Puzzle()
    };

    public void LoadPuzzle(int puzzleNumber)
    {
        Debug.Log("loading Puzzle " + puzzleNumber);
        if (puzzle.Length <= puzzleNumber)
        {
            Debug.Log("YouWon?");
            //you won?
            return;
        }
        else { 


        foreach (GameObject puzzlePiece in placedPuzzlePieces)
        {
            Destroy(puzzlePiece);

        }
        placedPuzzlePieces.Clear();

        //initalize Puzzle
        for (int x = 0; x < puzzle[puzzleNumber].puzzlePieces.Length; x++)
        {
            placedPuzzlePieces.Add(Instantiate(puzzle[puzzleNumber].puzzlePieces[x], puzzleContainer.transform));

            foreach (GameObject puzzlePiece in placedPuzzlePieces)
            {
                puzzlePiece.transform.position = new Vector3 (puzzlePiece.transform.position.x, puzzlePiece.transform.position.y, target.position.z);
            }
            //Instantiate(puzzle[puzzleNumber].puzzlePieces[x],puzzleContainer.transform);

            //put this into a list
        }
        currentPuzzle = puzzleNumber;
            //Instantiate(puzzle[puzzleNumber].

        }
    }

    public void NextPuzzle()
    {
        LoadPuzzle(currentPuzzle+1);
        Debug.Log("PuzzleComplete, loading Puzzle " + currentPuzzle);
    }

    public bool CheckIfComplete()
    {
        bool solved = true;

        //if list is empty
        if(puzzleContainer.transform.childCount == 0) { return false; }

        foreach(GameObject puzzlePiece in placedPuzzlePieces)
        {
            if (!puzzlePiece.GetComponent<PuzzleObject>().CheckIfAtTarget())
            {
                solved = false;
                //Debug.Log("not solved");
            }
        }
        /*
        foreach (Transform child in puzzleContainer.transform)
        {
            //catch this
            //Debug.Log(child.GetComponent<PuzzleObject>().CheckIfAtTarget());



            if (!child.GetComponent<PuzzleObject>().CheckIfAtTarget())
            {
                solved = false;
                Debug.Log("not solved");
            }
        }
        */
        return solved;
    }

    IEnumerator WaitWinTimer (int waitTime)
    {
        //here is the code before
        print(Time.time);
        yield return new WaitForSeconds(waitTime);
        //here is the code after
        print(Time.time);
        NextPuzzle();
    }


}
