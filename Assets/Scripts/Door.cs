using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Transform   t_leftDoor;
    [SerializeField]    
    private Transform   t_rightDoor;
    [Range(0, 10)]      
    [SerializeField]    
    private int         f_timeMax;
    [Range(0, 2)]       
    [SerializeField]    
    private float       f_speed;
    [SerializeField]    
    private float       f_distance;
    private Vector3     l_closePos;
    private Vector3     l_openPos;
    private Vector3     r_closePos;
    private Vector3     r_openPos;
    private float       f_timeNow = 0;
    private bool        b_isOpened = false;

    private void Start()
    {
        DOTween.Init(false, false, null);

        l_closePos   = t_leftDoor.localPosition;
        l_openPos    = t_leftDoor.localPosition;
        l_openPos.z += f_distance;

        r_closePos   = t_rightDoor.localPosition;
        r_openPos    = t_rightDoor.localPosition;
        r_openPos.z -= f_distance;
    }

    private void Update()
    {
        // Check state of door
        if (b_isOpened)
        {
            if (f_timeNow >= f_timeMax)
            {
                b_isOpened = false;
            }
            else
            {
                f_timeNow += Time.deltaTime;
            }
        } 
        else
        {
            f_timeNow = 0;
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        t_leftDoor.DOLocalMove(l_openPos, f_speed);
        t_rightDoor.DOLocalMove(r_openPos, f_speed);
        b_isOpened = true;
    }

    public void CloseDoor()
    {
        // TODO: Check if (other)collider is overlapping while closing
        t_leftDoor.DOLocalMove(l_closePos, f_speed);
        t_rightDoor.DOLocalMove(r_closePos, f_speed);
        b_isOpened = false;
    }

    public bool IsOpened()
    {
        return b_isOpened;
    }
}
