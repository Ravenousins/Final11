using UnityEngine;
using UnityEngine.EventSystems;

public class TurretController : MonoBehaviour
{
    public PanelControl panelControl;
    public Turret turret;

    private bool turretClickedThisFrame;

    private void Update()
    {
        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the click was not on a UI element
            if (!EventSystem.current.IsPointerOverGameObject() && !turretClickedThisFrame)
            {
                if (panelControl != null)
                {
                    panelControl.HidePanel();
                }
                else
                {
                    Debug.Log("");
                }
            }
            turretClickedThisFrame = false;
        }
    }



private void OnMouseDown()
    {
        // Check if the click was not on a UI element
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (panelControl != null && turret != null)
            {
                Debug.Log("TurretWasClicked");
                panelControl.ShowPanel();
                turret.UpdateUI();
                turretClickedThisFrame = true;
            }
        }
    }
}
