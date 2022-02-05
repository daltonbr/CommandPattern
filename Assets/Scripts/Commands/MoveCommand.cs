using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : CommandManager.ICommand
{
    private Vector3Int m_From;
    private Vector3Int m_To;

    public MoveCommand(Vector3Int start, Vector3Int end) 
    {
        m_From = start;
        m_To = end;
    }
    
    public void Execute()
    {
        var unit = Gameboard.Instance.GetUnit(m_From);
        if (unit != null)
        {
            Gameboard.Instance.MoveUnit(unit, m_To);
            Gameboard.Instance.SwitchTeam();
        }
    }
}
