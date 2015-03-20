using System.Runtime.InteropServices;
using UnityEngine;

public class MoveCursor : MonoBehaviour 
{
    [Range(0.0f, 110.0f)]
    public float newX, newY;
    void Start ()
    {
        SetCursorPos(ref newX,  ref newY);
    }

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(ref float X, ref float Y);
    //[DllImport("user32.dll")]
    //public static extern bool GetCursorPos(out Point pos);
}
