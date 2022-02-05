using UnityEngine;

/// <summary>
/// Capture a opposing side team piece
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item><description>Move the active statue to the occupied cell</description></item>
/// <item><description>Move the enemy statue to the side of the game board. This will represent its capture.</description></item>
/// </list>
/// </remarks>
public class CaptureCommand : CommandManager.ICommand
{
    private readonly Vector3Int m_From;
    private readonly Vector3Int m_To;
    private readonly Unit m_CapturingUnit;
    private readonly Unit m_CaptureUnit;

    public CaptureCommand(Vector3Int start, Vector3Int end)
    {
        m_From = start;
        m_To = end;
        
        m_CapturingUnit = Gameboard.Instance.GetUnit(m_From);
        m_CaptureUnit = Gameboard.Instance.GetUnit(m_To);
        // TODO: throw an error if either Unit is null?
    }

    public void Execute()
    {
        if (m_CaptureUnit == null || m_CapturingUnit == null)
        {
            Debug.LogWarning("Can't execute CaptureCommand");
            return;
        }
        
        Debug.Log($"CaptureCommand from: {m_From} to: {m_To}");
        Gameboard.Instance.MoveUnit(m_CapturingUnit, m_To);
        Gameboard.Instance.TakeOutUnit(m_CaptureUnit);
        Gameboard.Instance.SwitchTeam();
    }

    public void Undo()
    {
        if (m_CaptureUnit == null || m_CapturingUnit == null)
        {
            Debug.LogWarning("Can't undo CaptureCommand");
            return;
        }

        Debug.Log($"Undo CaptureCommand from: {m_From} to: {m_To}");
        Gameboard.Instance.MoveUnit(m_CaptureUnit, m_To);
        Gameboard.Instance.MoveUnit(m_CapturingUnit, m_From);
        Gameboard.Instance.SwitchTeam();
    }
}
