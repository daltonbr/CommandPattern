using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    [SerializeField] private Button m_UndoButton;
    [SerializeField] private TextMeshProUGUI m_TextButton;

    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public static CommandManager Instance { get; private set; }

    private Stack<ICommand> m_CommandsBuffer = new Stack<ICommand>();

    private void Awake()
    {
        Instance = this;
        UpdateUndoButton();
    }

    public void AddCommand(ICommand command)
    {
        if (command == null)
        {
            Debug.LogWarning("No Command to add");
            return;
        }
        
        command.Execute();
        m_CommandsBuffer.Push(command);

        UpdateUndoButton();
    }

    private void UpdateUndoButton()
    {
        int commandsBufferCount = m_CommandsBuffer.Count;
        m_UndoButton.interactable = commandsBufferCount > 0;
        m_TextButton.text = commandsBufferCount > 0 
            ? $"Undo ({commandsBufferCount})" 
            : "Undo";
    }

    public void Undo()
    {
        if (m_CommandsBuffer.TryPop(out var command) == false)
        {
            Debug.Log("No commands left to Undo;");
            return;
        }
        
        command.Undo();
        UpdateUndoButton();
    }
}
