using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserControl : MonoBehaviour
{
    enum State
    {
        SelectingUnit,
        MoveUnit
    }

    public GameObject SelectorPrefab;
    public GameObject MoveDisplayPrefab;

    private State m_CurrentState;
    
    private GameObject m_Selector;
    private Unit m_SelectedUnit = null;

    private Vector3Int[] m_MovableCells;

    private int m_DisplayedMoveDisplay;
    private List<GameObject> m_MoveDisplayPool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentState = State.SelectingUnit;
        
        m_Selector = Instantiate(SelectorPrefab);
        m_Selector.SetActive(false);
        
        //we can't get more than HxW move display so instantiate enough in the pool
        int count = Gameboard.Instance.Height * Gameboard.Instance.Width;
        for (int i = 0; i < count; ++i)
        {
            var o = Instantiate(MoveDisplayPrefab);
            o.SetActive(false);
            m_MoveDisplayPool.Add(o);
        }

        m_DisplayedMoveDisplay = 0;
        
        m_MovableCells = new Vector3Int[count];
    }

    // Update is called once per frame
    void Update()
    {
        //We still have an animation underway, we can't interact yet.
        if(Gameboard.Instance.AnimationSystem.IsAnimating)
            return;
        
        switch (m_CurrentState)
        {
            case State.SelectingUnit:
                if (Input.GetMouseButtonUp(0))
                {
                    CheckUnitToSelect();
                }

                break;
            case State.MoveUnit:
                if (Input.GetMouseButtonUp(0))
                {
                    MoveUnit();
                }
                break;
        }
    }

    void DeselectUnit()
    {
        m_SelectedUnit = null;
        m_Selector.gameObject.SetActive(false);
        CleanMoveIndicator(0, m_DisplayedMoveDisplay);
        m_DisplayedMoveDisplay = 0;
    }
    

    void CheckUnitToSelect()
    {
        if (Gameboard.Instance.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
            out Vector3Int clickedCell))
        {
            var unit = Gameboard.Instance.GetUnit(clickedCell);
            
            if (unit != null && unit.Side == Gameboard.Instance.CurrentTeam)
                m_SelectedUnit = unit;
            else
                m_SelectedUnit = null;
        }
        else
        {
            m_SelectedUnit = null;
        }
        
        if (m_SelectedUnit != null)
        {
            var gameboard = Gameboard.Instance;
            m_Selector.SetActive(true);
            m_Selector.transform.position = m_SelectedUnit.transform.position;
            
            int count = m_SelectedUnit.GetMoveCells(m_MovableCells, gameboard);
            for (int i = 0; i < count; i++)
            {
                m_MoveDisplayPool[i].SetActive(true);
                m_MoveDisplayPool[i].transform.position = gameboard.Grid.GetCellCenterWorld(m_MovableCells[i]);
            }

            //if the previous display was bigger than this one, this will loop over the extra display to disable
            //but if the current count is larger than the previous one, this will simply skip over the loop
            CleanMoveIndicator(count, m_DisplayedMoveDisplay);

            m_DisplayedMoveDisplay = count;
            m_CurrentState = State.MoveUnit;
        }
        else
        {
            DeselectUnit();
        }
    }
    
    void CleanMoveIndicator(int lowerBound, int upperBound)
    {
        for (int i = lowerBound; i < upperBound; ++i)
        {
            m_MoveDisplayPool[i].SetActive(false);
        }
    }

    void MoveUnit()
    {
        //We use the Raycast function of the Gameboard which will output in clickedCell which cell was clicked. 
        if (Gameboard.Instance.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
            out Vector3Int clickedCell))
        {
            //m_MovableCells contains all the cells our currently selected unit can move to.
            //So we check if the cell we just clicked is part of that list.
            if (m_MovableCells.Contains(clickedCell))
            {
                var unit = Gameboard.Instance.GetUnit(clickedCell);

                //only if there is no unit on the target cell do we move there
                if (unit == null)
                {
                    MoveCommand cmd = new MoveCommand(m_SelectedUnit.CurrentCell, clickedCell);
                    CommandManager.Instance.AddCommand(cmd);
                }
            }
        }

        DeselectUnit();
        m_CurrentState = State.SelectingUnit;
    }
}
