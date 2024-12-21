using Unity.VisualScripting;
using UnityEngine;

public class Run : MonoBehaviour
{
    [SerializeField] private Walk walk;
    [SerializeField] private float multiply;

    public void DoRun()
    {
        walk.SetMultiply(multiply);
    }
}
