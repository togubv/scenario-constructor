using UnityEngine;
using UnityEngine.InputSystem;

namespace SC
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform cameraTrans;
        [SerializeField] private float _mouseSensitivity = 1f;
        [SerializeField] private float _movespeed = 1f;

        private InputReader _inputReader;

        private float _cameraRotationX = 0f;
        private float _cameraRotationY = 0f;
        private bool _isStopped;

        public void Init(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        private void Update()
        {
            CameraRotate();
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void CameraRotate()
        {
            float mouseX = Mouse.current.delta.x.ReadValue() * _mouseSensitivity;
            float mouseY = Mouse.current.delta.y.ReadValue() * _mouseSensitivity;

            _cameraRotationY += mouseX;
            transform.rotation = Quaternion.Euler(0f, _cameraRotationY, 0f);

            _cameraRotationX -= mouseY;
            _cameraRotationX = Mathf.Clamp(_cameraRotationX, -80f, 80f);

            cameraTrans.localRotation = Quaternion.Euler(_cameraRotationX, 0f, 0f);
        }

        private void Movement()
        {
            if (_inputReader.axisInput != Vector2.zero)
            {
                if (_isStopped)
                {
                    _isStopped = false;
                }

                var movement = _inputReader.axisInput.y * _movespeed * transform.forward +
                    _inputReader.axisInput.x * _movespeed * transform.right;

                rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
            }
            else
            {
                if (_isStopped == false)
                {
                    _isStopped = true;
                    rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
                }
            }
        }
    }
}