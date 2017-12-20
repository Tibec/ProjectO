using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudPosition : MonoBehaviour {

    private HudSettings settings;
    private HudManager manager;

    private bool isMoving;
    // Use this for initialization
    void Start () {
        settings = GetComponent<HudSettings>();
        manager = GetComponent<HudManager>();
        isMoving = false;
}

// Update is called once per frame
void Update () {

        if (settings.TargetToFollow != null && !isMoving)
        {
            Vector3 newPosition = settings.TargetToFollow.position + Vector3.up * settings.HudHeight;
            if (Vector3.Distance(newPosition, transform.position) >= settings.hudFollowDeadZoneDistance)
            {
                isMoving = true;
            }
            
            if (manager.OpenedMenu.Count == 1)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(transform.localEulerAngles.y, settings.TargetToFollow.localEulerAngles.y)) >= settings.hudRotationDeadZoneAngle)
                    isMoving = true;
            }
            else if (manager.OpenedMenu.Count > 1)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(settings.RotationBar.transform.localEulerAngles.y, settings.TargetToFollow.localEulerAngles.y)) >= settings.hudRotationDeadZoneAngle)
                    isMoving = true;
                else if (settings.RotationBar.transform.localPosition == Vector3.zero)
                    isMoving = true;
            }
        }
        else if(settings.TargetToFollow != null && isMoving)
        {
            // Compute & move : menus
            Vector3 targetPosition = settings.TargetToFollow.position + Vector3.up * settings.HudHeight;
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, settings.hudMovementLerpCoef);
            transform.position = newPosition;

            // compute & move : rotation
            float targetRotation = settings.TargetToFollow.localEulerAngles.y % 360f;
            float newRotation = 0f;
            if (manager.OpenedMenu.Count == 1)
            {
                newRotation = Mathf.LerpAngle(transform.localEulerAngles.y, targetRotation, settings.hudRotationLerpCoef);
                transform.localEulerAngles = Vector3.up * newRotation;
            }
            else if (manager.OpenedMenu.Count > 1)
            {
                newRotation = Mathf.LerpAngle(settings.RotationBar.transform.localEulerAngles.y, targetRotation, settings.hudRotationLerpCoef);
                settings.RotationBar.transform.localEulerAngles = Vector3.up * newRotation;

                settings.RotationBar.transform.localPosition = ComputeRotationBarPosition(newRotation);
                settings.RotationBar.transform.localRotation = ComputeRotationBarRotation(newRotation);
            }

            // Check if we should continue
            if (Vector3.Distance(targetPosition, newPosition) <= settings.hudMovementPrecision
                && Mathf.Abs(Mathf.DeltaAngle(targetRotation, newRotation)) <= settings.hudRotationPrecision)
            {
                isMoving = false;
            }
        }

    }

    private Vector3 ComputeRotationBarPosition(float targetAngle)
    {
        Vector3 v = new Vector3();
        float angle = targetAngle * Mathf.PI / 180.0f;
        v.x = settings.MenuRadius * Mathf.Sin(angle);
        v.y = settings.RotationHeight;
        v.z = settings.MenuRadius * Mathf.Cos(angle);
        return v;
    }

    private Quaternion ComputeRotationBarRotation(float targetAngle)
    {
        return Quaternion.Euler(0, targetAngle, 0);
    }

    public Vector3 ComputeMenuPosition()
    {
        return settings.TargetToFollow.position + Vector3.up * settings.HudHeight;
    }

    private Vector3 ComputePopupPosition()
    {
        Vector3 v = new Vector3();
        float angle = settings.TargetToFollow.rotation.eulerAngles.y * Mathf.PI / 180.0f;
        v.x = settings.PopupRadius * Mathf.Sin(angle);
        v.z = settings.PopupRadius * Mathf.Cos(angle);
        return v;
    }

}
