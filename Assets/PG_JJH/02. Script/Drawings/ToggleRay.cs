using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Toggle between the direct and ray interactor if the direct interactor isn't touching any objects
/// Should be placed on a ray interactor
/// </summary>
[RequireComponent(typeof(XRRayInteractor))]
public class ToggleRay : MonoBehaviour
{
    [Tooltip("Switch even if an object is selected")]
    public bool forceToggle = false;

    [Tooltip("The direct interactor that's switched to")]
    public XRDirectInteractor directInteractor = null;

    [SerializeField] private XRRayInteractor rayInteractor = null;
    private bool isSwitched = false;

    private void Awake()
    {
        // 라이트 핸드는 인스펙터창에 참조되어 있음. 
        rayInteractor = GetComponent<XRRayInteractor>();
    }

    private void Start()
    {
        SwitchInteractors(false);
    }

    public void ActivateRay()
    {
        Debug.Log("ActivateRay called");
        SwitchInteractors(true);
    }

    public void DeactivateRay()
    {
        Debug.Log("DeactivateRay called");
        if (isSwitched)
            SwitchInteractors(false);
    }

    private void SwitchInteractors(bool value)
    {
        Debug.Log("SwitchInteractors called with value: " + value);
        isSwitched = value;
        rayInteractor.enabled = value;
        directInteractor.enabled = !value;
        Debug.Log("Ray Interactor enabled: " + rayInteractor.enabled);
        Debug.Log("Direct Interactor enabled: " + directInteractor.enabled);

        // Line Renderer 상태 디버깅
        LineRenderer lineRenderer = rayInteractor.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            Debug.Log("Line Renderer enabled state before: " + lineRenderer.enabled);
            lineRenderer.enabled = value;
            Debug.Log("Line Renderer enabled state after: " + lineRenderer.enabled);
        }
        else
        {
            Debug.LogWarning("Line Renderer not found on the Ray Interactor.");
        }
    }
}