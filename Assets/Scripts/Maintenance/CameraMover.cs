using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    [Header("Границы движения камеры")]
    [SerializeField] private float _maxZ = 160f;
    [SerializeField] private float _minZ = -250f;
    [SerializeField] private float _boundaryX = 200f;

    [SerializeField] private float _moveSpeed = 50f;

    private void Update()
    {
        float horizontalInput = Input.GetAxis(Horizontal);
        float verticalInput = Input.GetAxis(Vertical);
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * _moveSpeed * Time.deltaTime;

        transform.Translate(movement, Space.World);

        float clampedX = Mathf.Clamp(transform.position.x, -_boundaryX, _boundaryX);
        float clampedZ = Mathf.Clamp(transform.position.z, _minZ, _maxZ);

        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }
}
