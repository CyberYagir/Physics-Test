using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActionType
{
    Action,
    Submenu,
    Window,
    Spawn
}

public interface IActionable
{
    public ActionType ActionType { get; }
    public void Action();

}
