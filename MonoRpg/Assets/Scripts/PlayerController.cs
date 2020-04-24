using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    #region Public

    public LayerMask movementMask;
    public Interactable focus;

    #endregion Public

    #region Local

    private Camera _cam;
    private PlayerMotor _motor;

    #endregion Local

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        // Interact
        if (Input.GetMouseButton(0))
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100))
            {
                var interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                    _motor.MoveToPoint(interactable.transform.position);
                }
            }
        }

        // Move
        if (Input.GetMouseButton(1))
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100, movementMask))
            {
                _motor.MoveToPoint(hit.point);
                RemoveFocus();
            }
        }
    }

    private void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null) focus.OnDefocused();

            focus = newFocus;
            _motor.FollowTarget(newFocus);
        }

        newFocus.OnFocused(transform);
    }

    private void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }

        focus = null;
        _motor.StopFollowingTarget();
    }
}