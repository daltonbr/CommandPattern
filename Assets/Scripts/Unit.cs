using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public enum Team
    {
        White,
        Black
    }
    
    public Vector3Int CurrentCell
    {
        get => m_CurrentCell;
        set => m_CurrentCell = value;
    }

    public Team Side;
    
    protected Vector3Int m_CurrentCell;
    
    // Start is called before the first frame update
    void Start()
    {
        m_CurrentCell = Gameboard.Instance.GetClosestCell(transform.position);
        Gameboard.Instance.SetUnit(m_CurrentCell, this);
        transform.position = Gameboard.Instance.Grid.GetCellCenterWorld(m_CurrentCell);
    }

    public abstract int GetMoveCells(Vector3Int[] result, Gameboard board);
}
