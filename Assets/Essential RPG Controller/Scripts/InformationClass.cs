using UnityEngine;
using System.Collections;

[System.Serializable]
public class CameraData
{
    public float RotationSpeed;

    public float VInput;
    public float HInput;

    public float CameraReset;

    public bool Autorun;
    public Animator Animator;
    public Transform Character;
}

 [System.Serializable]
 public class PlayerData
 {
     public float MovementInput, DirectionInput;
     public float RotationSpeed;
     public bool Jump;
     public bool Autorun;
     public Animator Animator;
     public float RotationInput;
     public Transform Character;
 }
