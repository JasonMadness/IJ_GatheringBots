using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    [SerializeField] private float _moveSpeed = 25f;

    private void Update()
    {
        float horizontalInput = Input.GetAxis(Horizontal);
        float verticalInput = Input.GetAxis(Vertical);
        Vector3 cameraDirection = GetCameraDirection();

        Vector3 movement = new Vector3(horizontalInput * cameraDirection.x, 0f, verticalInput * cameraDirection.z) * _moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    private Vector3 GetCameraDirection()
    {
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = transform.right;
        right.y = 0f;
        right.Normalize();
        return new Vector3(right.x, 0f, forward.z);
    }
}
