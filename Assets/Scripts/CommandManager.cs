using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public interface ICommand
    {
        void Execute();
    }

    public static CommandManager Instance { get; private set; }

    private Stack<ICommand> m_CommandsBuffer = new Stack<ICommand>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddCommand(ICommand command)
    {
        command.Execute();
        m_CommandsBuffer.Push(command);
    }
}
