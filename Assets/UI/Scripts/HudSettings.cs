using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudSettings : MonoBehaviour {

    public float MenuInclinaison;

    public float PopupRadius;
    public float MenuRadius;

    public float HudHeight;

    public float RotationHeight;

    public int MaxMenuWindow;

    public float hudFollowDeadZoneDistance;
    public float hudRotationDeadZoneAngle;
    public float hudMovementPrecision;
    public float hudRotationPrecision;
    public float hudMovementLerpCoef;
    public float hudRotationLerpCoef;

    public Transform TargetToFollow;

    public Transform Popup;

    public GameObject RotationBar;

    public Transform MenusContainer;

}
