using UnityEngine;

public class MoveCommand : CommandManager.ICommand
{
    private readonly Vector3Int m_From;
    private readonly Vector3Int m_To;
    private Unit m_MovingUnit;

    public MoveCommand(Vector3Int start, Vector3Int end) 
    {
        m_From = start;
        m_To = end;
        m_MovingUnit = Gameboard.Instance.GetUnit(m_From);
        // TODO: throw an error if Unit is null?
    }
    
    public void Execute()
    {
        if (m_MovingUnit == null)
        {
            Debug.LogWarning("Can't execute MoveCommand");
            return;
        }
        
        Debug.Log($"MoveCommand from: {m_From} to: {m_To}");
        Gameboard.Instance.MoveUnit(m_MovingUnit, m_To);
        Gameboard.Instance.SwitchTeam();
    }

    public void Undo()
    {
        if (m_MovingUnit == null)
        {
            Debug.LogWarning("Can't undo MoveCommand");
            return;
        }
        
        Debug.Log($"Undo MoveCommand: from: {m_From} to: {m_To}");
        Gameboard.Instance.MoveUnit(m_MovingUnit, m_From);
        Gameboard.Instance.SwitchTeam();
    }

    public void Redo() => Execute();
}
