using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    [SerializeField] private Button undoButton;
    [SerializeField] private TextMeshProUGUI undoButtonText;
    
    [SerializeField] private Button redoButton;
    [SerializeField] private TextMeshProUGUI redoButtonText;

    public interface ICommand
    {
        void Execute();
        void Undo();
        void Redo();
    }

    public static CommandManager Instance { get; private set; }

    private readonly Stack<ICommand> m_UndoCommandsBuffer = new();
    private readonly Stack<ICommand> m_RedoCommandsBuffer = new();

    private void Awake()
    {
        Instance = this;
        UpdateUndoButton();
        UpdateRedoButton();
    }

    public void AddCommand(ICommand command)
    {
        if (command == null)
        {
            Debug.LogWarning("No Command to add");
            return;
        }
        
        command.Execute();
        m_UndoCommandsBuffer.Push(command);

        UpdateUndoButton();
    }

    private void UpdateUndoButton()
    {
        int undoCommandsBufferCount = m_UndoCommandsBuffer.Count;
        undoButton.interactable = undoCommandsBufferCount > 0;
        undoButtonText.text = undoCommandsBufferCount > 0 
            ? $"Undo ({undoCommandsBufferCount})" 
            : "Undo";
    }

    private void UpdateRedoButton()
    {
        int redoCommandsBufferCount = m_RedoCommandsBuffer.Count;
        redoButton.interactable = redoCommandsBufferCount > 0;
        redoButtonText.text = redoCommandsBufferCount > 0 
            ? $"Redo ({redoCommandsBufferCount})" 
            : "Redo";
    }

    public void Undo()
    {
        if (m_UndoCommandsBuffer.TryPop(out var command) == false)
        {
            Debug.Log("No commands left to Undo;");
            return;
        }
        
        command.Undo();
        m_RedoCommandsBuffer.Push(command);
        UpdateUndoButton();
        UpdateRedoButton();
    }

    public void Redo()
    {
        if (m_RedoCommandsBuffer.TryPop(out var command) == false)
        {
            Debug.Log("No commands left to Redo");
            return;
        }

        command.Redo();
        m_UndoCommandsBuffer.Push(command);
        UpdateUndoButton();
        UpdateRedoButton();
    }
}
