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
    [SerializeField] public List<GameObject> placedAdditionalObjects = new List<GameObject>();

    [SerializeField] GameObject puzzleCompleteUI;
    [SerializeField] GameObject puzzleCompleteAlternateUI;

    public Transform selectedObject;
    public Collider selectedObjectCollider;

    [SerializeField] bool wonTheGame = false;

    [SerializeField] bool wonTheGameAndUI = false;

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

        if (puzzleContainer == null)
        {
            Debug.LogWarning("GameManager missing Puzzle Container!");
        }

        if (puzzleCompleteUI == null)
        {
            Debug.LogWarning("GameManager missing Puzzle Complete UI!");
        }

        if (puzzleCompleteAlternateUI == null)
        {
            Debug.LogWarning("GameManager missing Puzzle Complete Alternate UI!");
        }

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
        if (!currentPuzzleComplete)
        {
            if (CheckIfComplete())
            {
                currentPuzzleComplete = true;

                foreach (GameObject puzzlePiece in placedPuzzlePieces)
                {
                    Debug.Log("Completed this puzle and started lerping");
                    puzzlePiece.GetComponent<PuzzleMover>().pauseAllMovement = true;
                    puzzlePiece.GetComponent<PuzzleMover>().lerpActive = true;

                }
            }

        }



        bool complete = CheckIfComplete();


        if (wonTheGame && !wonTheGameAndUI)
        {
            Debug.Log("won the game");
            foreach (GameObject puzzlePiece in placedPuzzlePieces)
            {
                puzzlePiece.GetComponent<PuzzleMover>().pauseAllMovement = true;
                //puzzlePiece.GetComponent<PuzzleMover>().lerpActive = true;

            }


            wonTheGameAndUI = true;

            StartCoroutine(WaitWinGameTimerMenu(1));

        }

        if (!wonTheGame)
        {

            if (complete)
            {
                Debug.Log("Completed This Puzzle");
                foreach (GameObject puzzlePiece in placedPuzzlePieces)
                {
                    puzzlePiece.GetComponent<PuzzleMover>().pauseAllMovement = true;
                    //puzzlePiece.GetComponent<PuzzleMover>().lerpActive = true;
                }


                //open UI
                if (puzzleCompleteUI != null)
                {
                    if(puzzleCompleteUI.activeSelf == false)
                    {
                        StartCoroutine(WaitWinTimerMenu(1));
                    }
                    //puzzleCompleteUI.SetActive(true);

                }
                else
                {
                    StartCoroutine(WaitWinTimer(3));
                }
            }


            if (CheckIfAlternateComplete())
            {
                Debug.Log("Completed AltSolution");

                foreach (GameObject puzzlePiece in placedPuzzlePieces)
                {
                    puzzlePiece.GetComponent<PuzzleMover>().pauseAllMovement = true;
                }


                //open UI
                if (puzzleCompleteAlternateUI != null)
                {
                    puzzleCompleteAlternateUI.SetActive(true);
                }
                else
                {
                    StartCoroutine(WaitWinTimer(3));
                }
            }
        }

    }

    [System.Serializable]
    public class Puzzle
    {
        [SerializeField] public GameObject[] puzzlePieces;

        [SerializeField] public GameObject[] AdditionalObjects;
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
            wonTheGame = true;
            return;
        }
        else { 

        //destroy old puzzle
        foreach (GameObject puzzlePiece in placedPuzzlePieces)
        {
            Destroy(puzzlePiece);

        }
            foreach (GameObject additionalObject in placedAdditionalObjects)
            {
                Destroy(additionalObject);

            }

            placedAdditionalObjects.Clear();
            placedPuzzlePieces.Clear();

            //initalize Puzzle

            for (int x = 0; x < puzzle[puzzleNumber].puzzlePieces.Length; x++)
            {
                placedPuzzlePieces.Add(Instantiate(puzzle[puzzleNumber].puzzlePieces[x], new Vector3(puzzle[puzzleNumber].puzzlePieces[x].transform.position.x, puzzle[puzzleNumber].puzzlePieces[x].transform.position.y, puzzle[puzzleNumber].puzzlePieces[x].transform.position.z + target.position.z + puzzle[puzzleNumber].puzzlePieces[x].GetComponent<PuzzleObject>().targets[0].targetPosition.z), puzzle[puzzleNumber].puzzlePieces[x].transform.rotation, puzzleContainer.transform));

            }

            for (int x = 0; x < puzzle[puzzleNumber].AdditionalObjects.Length; x++)
            {
                placedAdditionalObjects.Add(Instantiate(puzzle[puzzleNumber].AdditionalObjects[x], GameManager.instance.transform));

            }
            currentPuzzle = puzzleNumber;
            //Instantiate(puzzle[puzzleNumber].

        }

        foreach (GameObject puzzlePiece in placedPuzzlePieces)
        {
            puzzlePiece.GetComponent<PuzzleMover>().pauseAllMovement = false;

        }
    }

    public void NextPuzzle()
    {
        LoadPuzzle(currentPuzzle+1);
        Debug.Log("PuzzleComplete, loading Puzzle " + currentPuzzle);
        if (puzzleCompleteUI.activeSelf)
        {
            puzzleCompleteUI.SetActive(false);
        }
        puzzleCompleteUI.SetActive(false);

        foreach (GameObject puzzlePiece in placedPuzzlePieces)
        {
            puzzlePiece.GetComponent<PuzzleMover>().lerpActive = false;
        }
        currentPuzzleComplete = false;

        Debug.Log("deactivated UI");

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
        if (solved)
        {
            //check if you won the game
            
            if (currentPuzzle >= puzzle.Length-1)
            {
                Debug.Log("YouWon?");
                //you won?
                wonTheGame = true;

 
            }
        }


        return solved;
    }

    public bool CheckIfAlternateComplete()
    {
        bool solved = true;

        //if list is empty
        if (puzzleContainer.transform.childCount == 0) { return false; }

        foreach (GameObject puzzlePiece in placedPuzzlePieces)
        {
            if (!puzzlePiece.GetComponent<PuzzleObject>().CheckIfAtAlternateTarget())
            {
                solved = false;
                //Debug.Log("not solved");
            }
        }
        if (solved)
        {
            //check if you won the game

            if (currentPuzzle >= puzzle.Length-1)
            {
                Debug.Log("YouWon?");
                //you won?
                wonTheGame = true;

            }
        }

        return solved;
    }

    IEnumerator WaitWinTimer (int waitTime)
    {
        //here is the code before
        //print(Time.time);
        yield return new WaitForSeconds(waitTime);
        //here is the code after
        //print(Time.time);
        NextPuzzle();
    }

    IEnumerator WaitWinTimerMenu(int waitTime)
    {
        //here is the code before
        //print(Time.time);
        yield return new WaitForSeconds(waitTime);
        //here is the code after
        //print(Time.time);
        Debug.Log("activated UI");
        puzzleCompleteUI.SetActive(true);
        //NextPuzzle();
    }

    IEnumerator WaitWinGameTimerMenu(int waitTime)
    {
        //here is the code before
        //print(Time.time);
        yield return new WaitForSeconds(waitTime);
        //here is the code after
        //print(Time.time);
        Debug.Log("activated aternateUI");
        puzzleCompleteAlternateUI.SetActive(true);
        //NextPuzzle();
    }


}
