using System;
using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _clickableLayers;

    public event Action<Vector3, GameObject> LeftClicked;
    public event Action<Vector3, GameObject> RightClicked;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            RaycastAndInvoke(LeftClicked);

        if (Input.GetMouseButtonDown(1))
            RaycastAndInvoke(RightClicked);
    }

    private void RaycastAndInvoke(Action<Vector3, GameObject> callback)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _clickableLayers) == false)
            return;

        callback?.Invoke(hit.point, hit.collider.gameObject);
    }
}