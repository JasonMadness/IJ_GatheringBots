using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private MouseClickHandler _mouse;
    [SerializeField] private Vector2 _minBounds;
    [SerializeField] private Vector2 _maxBounds;

    private HomeBase _selectedBase;

    private void OnEnable()
    {
        _mouse.LeftClicked += OnLeftClick;
        _mouse.RightClicked += OnRightClick;
    }

    private void OnDisable()
    {
        _mouse.LeftClicked -= OnLeftClick;
        _mouse.RightClicked -= OnRightClick;
    }

    private void OnLeftClick(Vector3 point, GameObject target)
    {
        if (target.TryGetComponent(out HomeBase baseComponent))
        {
            _selectedBase = baseComponent;
            return;
        }

        _selectedBase = null;
    }

    private void OnRightClick(Vector3 point, GameObject target)
    {
        if (_selectedBase == null)
            return;

        if (IsInsideBounds(point) == false)
            return;

        if (target.TryGetComponent(out HomeBase homeBase))
        {
            if (homeBase != _selectedBase)
            {
                return;
            }
            else
            {
                _selectedBase.ClearFlag();
                return;
            }
        }

        _selectedBase.SetFlag(point);

    private bool IsInsideBounds(Vector3 point)
    {
        return point.x >= _minBounds.x && point.x <= _maxBounds.x
            && point.z >= _minBounds.y && point.z <= _maxBounds.y;
    }
}