using UnityEngine;
using UnityEngine.InputSystem;

namespace SC
{
    public class InputReader
    {
        public Vector2 axisInput { get; private set; }

        private PlayerInput _playerInput;

        public void Init()
        {
            _playerInput = new PlayerInput();
            _playerInput.Enable();
            AddHandlers();
        }

        public void Dispose()
        {
            RemoveHandlers();
        }

        private void AddHandlers()
        {
            _playerInput.Player.Move.performed += OnMove;
            _playerInput.Player.Move.canceled += OnMoveCanceled;
        }

        private void RemoveHandlers()
        {
            _playerInput.Player.Move.performed -= OnMove;
            _playerInput.Player.Move.canceled -= OnMoveCanceled;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            axisInput = ctx.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            axisInput = ctx.ReadValue<Vector2>();
        }
    }
}