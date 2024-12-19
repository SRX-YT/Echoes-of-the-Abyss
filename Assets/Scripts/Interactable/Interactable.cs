using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool canEnter;
    [SerializeField] private bool canMove;
    [SerializeField] private bool canTake;

    private List<string> actions = new();

    private void Start()
    {
        if (canEnter) { actions.Add("Enter"); }
        if (canMove) { actions.Add("Move");  }
        if (canTake) { actions.Add("Take");  }
    }

    public List<string> GetActions()
    {
        return actions;
    }
}
