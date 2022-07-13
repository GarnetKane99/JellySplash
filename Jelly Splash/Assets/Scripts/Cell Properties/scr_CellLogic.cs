using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Derives from the scr_CellSetup class
public class scr_CellLogic : scr_CellSetup
{
    private void Start()
    {
        //Subscribe to input event
        scr_UserInput.GetInput += GetUserInput;
        scr_UserInput.RemoveInput += RemoveInput;
    }

    public void GetUserInput(Vector2 MousePosition)
    {
        scr_CellOwner FoundCell = FoundObject(MousePosition);

        //Check if valid piece is selected
        if (FoundCell != null && !b_UpdatingBoard)
        {
            //Set up initial pieces so that it is known which cell is selected first
            if (StartingPiece == int.MaxValue)
            {
                SetupFirstPiece(FoundCell);
                UpdateSelectedCells();
            }
            else
            {
                //Find next vector coordinate
                NextVectorCoordinate = FoundCell.CellData.TrueCoordinate;

                //Check and make sure that the next vector coordinate is within the bounds (i.e. within surrounding tiles of previous tile)
                if (Vector2.SqrMagnitude(NextVectorCoordinate - StartingVectorCoordinate) <= 2)
                {
                    ConnectPieces(ManagerInstance.m_BoardLayout[(int)FoundCell.transform.position.x + (ManagerInstance.m_BoardWidth / 2),
                    (int)FoundCell.transform.position.y + (ManagerInstance.m_BoardHeight / 2)], FoundCell);
                }
            }
        }
    }

    //If input is not detected, then it will unselect pieces if necessary and board will go back to default state
    public void RemoveInput()
    {
        if (ManagerInstance.m_CurrentSelected >= ManagerInstance.m_MinSelected)
        {
            StartCoroutine(this.UpdateBoard());
        }
        else if (ManagerInstance.m_CurrentSelected > 0)
        {
            DeselectBoard();
        }

        ResetBoard();
    }

    #region Board Visualisation Methods
    //Board Update state
    public IEnumerator UpdateBoard()
    {
        b_UpdatingBoard = true;

        scr_BoardCheck.ins.StartUpdating();

        int multiplier = 0;
        for (int i = 0; i < FoundCoordinates.Count; i++)
        {
            //Score multiplier handler
            if (i % 4 == 0)
            {
                multiplier++;
            }
            ManagerInstance.CurrentScore += ManagerInstance.ScoreIncrease + (multiplier * ManagerInstance.ScoreMultiplier);

            scr_CellOwner cellToUpdate = CellScripts[FoundCoordinates[i].x, FoundCoordinates[i].y]; //Find the cell to update (these are in order of selection)
            cellToUpdate.CellData.Anim.Play("Pop"); //play specific pop animation
            yield return new WaitForSeconds(t_TimeBetweenPops); //coroutine to pause visuals briefly just for effect
        }

        //Invoke("FinishUpdate", 0.5f);
        StartCoroutine(FinishUpdate());
    }

    //Update cells in order now
    private IEnumerator FinishUpdate()
    {
        ReorganiseList();   //First thing to do is ensure that the found coordinates are in order to ensure pieces are being updated correctly and getting correct data

        int startX = CellScripts[FoundCoordinates[0].x, FoundCoordinates[0].y].CellData.TrueCoordinate.x;
        int newX;
        List<scr_CellOwner> CellsToUpdate = new List<scr_CellOwner>();

        for (int i = 0; i < FoundCoordinates.Count; i++)    //Loop through all pieces (this is updated continuously)
        {
            scr_CellOwner CellFound = CellScripts[FoundCoordinates[i].x, FoundCoordinates[i].y];    //create reference to current cell
            if (CellFound)
            {
                //ShowMatrix(); *debugging stuff*
                newX = CellFound.CellData.TrueCoordinate.x; //This is necessary for the visual updates and the pieces falling animations
                if (newX != startX) //check if current x looping position is equal to the new x looping position
                {
                    StartCoroutine(FinishX(CellsToUpdate)); //it'll be assumed that the pieces from here don't have any possible further movements and will just be randomised
                    startX = newX;  //make start x = to current x
                    CellsToUpdate = new List<scr_CellOwner>();  //create new instance of list
                }
                CellsToUpdate.Add(CellFound);   //add current position

                if (CellFound.CellData.CurrentCellValue != CellFound.CellData.NextCell.CellData.CurrentCellValue)   //check if current piece isn't equal to piece directly vertical (1 unit)
                {
                    UpdateVerticalPiece(CellFound, i, FoundCoordinates[i].x, FoundCoordinates[i].y);
                    CellsToUpdate.Remove(CellFound);
                    yield return new WaitForSeconds(t_TimeBetweenTransition);
                }
                else
                {
                    scr_CellOwner NextCell = CellFound.CellData.NextCell;
                    for (int j = FoundCoordinates[i].y + 1; j < ManagerInstance.m_BoardLayout.GetLength(1); j++)    //loop through remaining y's in coordinate to see if there's any possible movement
                    {
                        if (FoundNextVertical(NextCell)) //will return true if a piece that is possible is found
                        {
                            UpdateNextVerticalPiece(CellFound, NextCell, FoundCoordinates[i].x, FoundCoordinates[i].y);
                            CellsToUpdate.Remove(CellFound);
                            yield return new WaitForSeconds(t_TimeBetweenTransition);
                            break;
                        }
                        NextCell = NextCell.CellData.NextCell;  //otherwise will look into next cells next cell reference
                    }
                }
            }
        }

        StartCoroutine(UpdateRemainingPieces());    //Any final highlighted cells (cells that are later updated to 10) will be called in this method to ensure that they are updated and randomised appropriately
    }

    //Basically organise found cells list using bubble sort so that it is now in order so that updating the cells is easier
    private void ReorganiseList()
    {
        Vector2Int heldPos;

        for (int i = 0; i < FoundCoordinates.Count - 1; i++)
        {
            for (int j = 0; j < FoundCoordinates.Count - i - 1; j++)
            {
                if (CellScripts[FoundCoordinates[j].x, FoundCoordinates[j].y].CellData.PositionValue > CellScripts[FoundCoordinates[j + 1].x, FoundCoordinates[j + 1].y].CellData.PositionValue)
                {
                    heldPos = FoundCoordinates[j + 1];
                    FoundCoordinates[j + 1] = FoundCoordinates[j];
                    FoundCoordinates[j] = heldPos;
                }
            }
        }
    }

    //this is to update the currently found cells to be random values (comments will be same as UpdateRemainingPieces coroutine)
    private IEnumerator FinishX(List<scr_CellOwner> CellsToUpdate)
    {
        for (int x = 0; x < CellsToUpdate.Count; x++)
        {
            ManagerInstance.m_BoardLayout[CellsToUpdate[x].CellData.TrueCoordinate.x, CellsToUpdate[x].CellData.TrueCoordinate.y] = Random.Range(ManagerInstance.m_MinObject, ManagerInstance.m_MaxObject);
            CellsToUpdate[x].CellData.CurrentCellValue = ManagerInstance.m_BoardLayout[CellsToUpdate[x].CellData.TrueCoordinate.x, CellsToUpdate[x].CellData.TrueCoordinate.y];

            AnimatorOverrideController overrideController = new AnimatorOverrideController(CellsToUpdate[x].CellData.Anim.runtimeAnimatorController);
            CellsToUpdate[x].CellData.Anim.runtimeAnimatorController = overrideController;

            AnimationClip IdleClip = ManagerInstance.g_AnimClips[ManagerInstance.m_BoardLayout[CellsToUpdate[x].CellData.TrueCoordinate.x, CellsToUpdate[x].CellData.TrueCoordinate.y]].Anims[0];
            AnimationClip PopClip = ManagerInstance.g_AnimClips[ManagerInstance.m_BoardLayout[CellsToUpdate[x].CellData.TrueCoordinate.x, CellsToUpdate[x].CellData.TrueCoordinate.y]].Anims[1];
            AnimationClip WakeClip = ManagerInstance.g_AnimClips[ManagerInstance.m_BoardLayout[CellsToUpdate[x].CellData.TrueCoordinate.x, CellsToUpdate[x].CellData.TrueCoordinate.y]].Anims[2];
            CellsToUpdate[x].CellData.IdleClip = IdleClip;
            CellsToUpdate[x].CellData.PopClip = PopClip;
            CellsToUpdate[x].CellData.WakeClip = WakeClip;

            overrideController["BeeIdle"] = IdleClip;
            overrideController["BeePop"] = PopClip;
            overrideController["BeeWake"] = WakeClip;
            CellsToUpdate[x].CellData.Anim.Play("Idle");
            yield return new WaitForSeconds(t_TimeBetweenTransition);
        }
    }

    //Method Call
    private void UpdateVerticalPiece(scr_CellOwner PieceToUpdate, int i, int x, int y)
    {
        ManagerInstance.m_BoardLayout[x, y] = ManagerInstance.m_BoardLayout[x, y + 1];  //Make current board position equal to the position above

        UpdateAnimClips(PieceToUpdate, PieceToUpdate.CellData.NextCell);

        PieceToUpdate.CellData.CurrentCellValue = PieceToUpdate.CellData.NextCell.CellData.CurrentCellValue; //Update the cell value to be equal to above cell value as well
        PieceToUpdate.CellData.NextCell.CellData.CurrentCellValue = ManagerInstance.m_HighlightedCell;    //Get the next cell data from current piece and replace that value as a highlighted cell

        PieceToUpdate.CellData.NextCell.CellData.Anim.Play("Empty");

        ManagerInstance.m_BoardLayout[x, y + 1] = ManagerInstance.m_HighlightedCell;    //Also ensure that the board data is updated so that the cell above is viewed as a highlighted cell
        FoundCoordinates.Insert(i + 1, new Vector2Int(x, y + 1));
    }

    //Method call
    private bool FoundNextVertical(scr_CellOwner NextPiece)
    {
        if (NextPiece.CellData.CurrentCellValue != ManagerInstance.m_HighlightedCell)   //returns true if given piece current cell value isn't equal to a highlighted cell
        {
            return true;
        }

        return false;
    }

    //Method Call
    private void UpdateNextVerticalPiece(scr_CellOwner CurrentPiece, scr_CellOwner NextPiece, int x, int y)
    {
        ManagerInstance.m_BoardLayout[x, y] = NextPiece.CellData.CurrentCellValue;  //Make current board position equal to next identified vertical cell value

        UpdateAnimClips(CurrentPiece, NextPiece);

        CurrentPiece.CellData.CurrentCellValue = NextPiece.CellData.CurrentCellValue;   //Update the cell value to next identified vertical cell value
        if (!FoundCoordinates.Contains(NextPiece.CellData.TrueCoordinate))
        {
            FoundCoordinates.Add(NextPiece.CellData.TrueCoordinate);
        }
        ManagerInstance.m_BoardLayout[NextPiece.CellData.TrueCoordinate.x, NextPiece.CellData.TrueCoordinate.y] = ManagerInstance.m_HighlightedCell;    //Update board layout based on next pieces true coordinates
        NextPiece.CellData.CurrentCellValue = ManagerInstance.m_HighlightedCell;    //Make next identified piece equal to a highlighted cell so it doesn't get grabbed again
        NextPiece.CellData.Anim.Play("Empty");
        ReorganiseList();
    }

    //Final call to update all remaining pieces
    private IEnumerator UpdateRemainingPieces()
    {
        for (int i = 0; i < FoundCoordinates.Count; i++)    //Loop through remaining coordinates
        {
            if (ManagerInstance.m_BoardLayout[FoundCoordinates[i].x, FoundCoordinates[i].y] == ManagerInstance.m_HighlightedCell)   //Any coordinates found that are still coming up as highlighted will be selected and updated
            {
                ManagerInstance.m_BoardLayout[FoundCoordinates[i].x, FoundCoordinates[i].y] = Random.Range(ManagerInstance.m_MinObject, ManagerInstance.m_MaxObject);   //make into random cell
                CellScripts[FoundCoordinates[i].x, FoundCoordinates[i].y].CellData.CurrentCellValue = ManagerInstance.m_BoardLayout[FoundCoordinates[i].x, FoundCoordinates[i].y];  //make cell data cell value equal to above

                AnimatorOverrideController overrideController = new AnimatorOverrideController(CellScripts[FoundCoordinates[i].x, FoundCoordinates[i].y].CellData.Anim.runtimeAnimatorController);  //access animator
                CellScripts[FoundCoordinates[i].x, FoundCoordinates[i].y].CellData.Anim.runtimeAnimatorController = overrideController; //give cell new animator

                AnimationClip IdleClip = ManagerInstance.g_AnimClips[ManagerInstance.m_BoardLayout[FoundCoordinates[i].x, FoundCoordinates[i].y]].Anims[0]; //create references to appropriate animations
                AnimationClip PopClip = ManagerInstance.g_AnimClips[ManagerInstance.m_BoardLayout[FoundCoordinates[i].x, FoundCoordinates[i].y]].Anims[1];
                AnimationClip WakeClip = ManagerInstance.g_AnimClips[ManagerInstance.m_BoardLayout[FoundCoordinates[i].x, FoundCoordinates[i].y]].Anims[2];

                CellScripts[FoundCoordinates[i].x, FoundCoordinates[i].y].CellData.IdleClip = IdleClip; //provide cell with appropriate animation references
                CellScripts[FoundCoordinates[i].x, FoundCoordinates[i].y].CellData.PopClip = PopClip;
                CellScripts[FoundCoordinates[i].x, FoundCoordinates[i].y].CellData.WakeClip = WakeClip;

                overrideController["BeeIdle"] = IdleClip;   //ensure all names are accurate
                overrideController["BeePop"] = PopClip;
                overrideController["BeeWake"] = WakeClip;

                CellScripts[FoundCoordinates[i].x, FoundCoordinates[i].y].CellData.Anim.Play("Idle");   //play by default the idling animation

                yield return new WaitForSeconds(0.1f);
            }
        }

        //ShowMatrix();
        b_UpdatingBoard = false;
        scr_BoardCheck.ins.FinishUpdating();
    }

    //Method call (Same as above)
    private void UpdateAnimClips(scr_CellOwner Current, scr_CellOwner New)
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController(Current.CellData.Anim.runtimeAnimatorController);
        Current.CellData.Anim.runtimeAnimatorController = overrideController;

        AnimationClip IdleClip = ManagerInstance.g_AnimClips[ManagerInstance.m_BoardLayout[New.CellData.TrueCoordinate.x, New.CellData.TrueCoordinate.y]].Anims[0];
        AnimationClip PopClip = ManagerInstance.g_AnimClips[ManagerInstance.m_BoardLayout[New.CellData.TrueCoordinate.x, New.CellData.TrueCoordinate.y]].Anims[1];
        AnimationClip WakeClip = ManagerInstance.g_AnimClips[ManagerInstance.m_BoardLayout[New.CellData.TrueCoordinate.x, New.CellData.TrueCoordinate.y]].Anims[2];

        Current.CellData.IdleClip = IdleClip;
        Current.CellData.PopClip = PopClip;
        Current.CellData.WakeClip = WakeClip;

        overrideController["BeeIdle"] = IdleClip;
        overrideController["BeePop"] = PopClip;
        overrideController["BeeWake"] = WakeClip;
        Current.CellData.Anim.Play("Idle");
    }

    #endregion

    #region Connecting Pieces
    private void SetupFirstPiece(scr_CellOwner FoundObject)
    {
        StartingPiece = FoundObject.CellData.CurrentCellValue;
        StartingVectorCoordinate = FoundObject.CellData.TrueCoordinate;
        FoundObject.CellData.Anim.Play("Awake");
        //Visual update
        FoundObject.transform.localScale = new Vector3(1, 1, 1);
        //Add to list of coordinates so that we don't go over the same coordinate
        FoundCoordinates.Add(StartingVectorCoordinate);

        FoundObject.CellData.CurrentCellValue = ManagerInstance.m_HighlightedCell;
    }

    //Connect the pieces
    private void ConnectPieces(int StartingPieceValue, scr_CellOwner NextObject)
    {
        NextPiece = StartingPieceValue;

        if (FoundCoordinates.Count > 2)
        {
            if (NextVectorCoordinate == FoundCoordinates[FoundCoordinates.Count - 2])
            {
                ManagerInstance.m_BoardLayout[FoundCoordinates[FoundCoordinates.Count-1].x, FoundCoordinates[FoundCoordinates.Count-1].y] = StartingPiece;
                CellScripts[FoundCoordinates[FoundCoordinates.Count-1].x, FoundCoordinates[FoundCoordinates.Count-1].y].CellData.CurrentCellValue = ManagerInstance.m_BoardLayout[FoundCoordinates[FoundCoordinates.Count-1].x, FoundCoordinates[FoundCoordinates.Count-1].y];
                CellScripts[FoundCoordinates[FoundCoordinates.Count-1].x, FoundCoordinates[FoundCoordinates.Count-1].y].transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                CellScripts[FoundCoordinates[FoundCoordinates.Count-1].x, FoundCoordinates[FoundCoordinates.Count-1].y].CellData.Anim.Play("Idle");
                NextVectorCoordinate = FoundCoordinates[FoundCoordinates.Count-2];
                StartingVectorCoordinate = NextVectorCoordinate;
                FoundCoordinates.Remove(FoundCoordinates[FoundCoordinates.Count-1]);
                ManagerInstance.m_CurrentSelected--;
                return;
            }
        }

        if (StartingPiece != NextPiece)
        {
            return; //Make it so that they can't be selected
        }

        //Make sure that the next vector coordinate hasn't been used yet to avoid backtracking
        for (int i = 0; i < FoundCoordinates.Count; i++)
        {
            if (NextVectorCoordinate == FoundCoordinates[i])
            {
                return;
            }
        }

        FoundCoordinates.Add(NextVectorCoordinate); //If gotten to this point then it will add the next coordinate piece so that it doesn't check it again
        StartingVectorCoordinate = NextVectorCoordinate;    //Make starting vector (in reality just current vector) equal to next vector coordinate
        NextObject.transform.localScale = new Vector3(1, 1, 1);
        //NextObject.CellData.FoundSprite.sprite = NextObject.CellData.AwakeSprite;
        NextObject.CellData.Anim.Play("Awake");
        NextObject.CellData.CurrentCellValue = ManagerInstance.m_HighlightedCell;
        UpdateSelectedCells();  //Update selected cells will
    }

    private void UpdateSelectedCells()
    {
        ManagerInstance.m_CurrentSelected++;    //Update current selected count
        ManagerInstance.m_BoardLayout[StartingVectorCoordinate.x, StartingVectorCoordinate.y] = ManagerInstance.m_HighlightedCell;  //Update board data
    }

    private void DeselectBoard()
    {
        for (int x = 0; x < FoundCoordinates.Count; x++)
        {
            ManagerInstance.m_BoardLayout[FoundCoordinates[x].x, FoundCoordinates[x].y] = StartingPiece;    //Make selected pieces back to original piece
            CellScripts[FoundCoordinates[x].x, FoundCoordinates[x].y].CellData.CurrentCellValue = ManagerInstance.m_BoardLayout[FoundCoordinates[x].x, FoundCoordinates[x].y];  //ensure cell value is also original value
        }
    }

    private void ResetBoard()
    {
        for (int x = 0; x < FoundCoordinates.Count; x++)
        {
            CellScripts[FoundCoordinates[x].x, FoundCoordinates[x].y].CellData.CurrentCellValue = ManagerInstance.m_BoardLayout[FoundCoordinates[x].x, FoundCoordinates[x].y];  //ensure cell value matches board value
            CellScripts[FoundCoordinates[x].x, FoundCoordinates[x].y].transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);  //reset transform scale
            if (!b_UpdatingBoard)
            {
                CellScripts[FoundCoordinates[x].x, FoundCoordinates[x].y].CellData.Anim.Play("Idle");   //reset animation
            }
        }

        ManagerInstance.m_CurrentSelected = 0;
        StartingPiece = int.MaxValue;
        NextPiece = int.MaxValue;

        StartingVectorCoordinate = new Vector2Int(int.MaxValue, int.MaxValue);
        NextVectorCoordinate = new Vector2Int(int.MaxValue, int.MaxValue);

        if (!b_UpdatingBoard)
        {
            FoundCoordinates = new List<Vector2Int>();
        }
    }

    private scr_CellOwner FoundObject(Vector2 FoundMousePos)
    {
        for (int x = 0; x < CellScripts.GetLength(0); x++)
        {
            for (int y = 0; y < CellScripts.GetLength(1); y++)
            {
                if (CellScripts[x, y])  //not null
                {
                    if (Vector2.SqrMagnitude((Vector2)CellScripts[x, y].transform.position - FoundMousePos) <= sqrRadius)
                    {
                        return CellScripts[x, y];
                    }
                }
            }
        }

        return null;
    }
    #endregion
}
