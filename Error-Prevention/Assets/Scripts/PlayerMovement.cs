using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField] private CharacterController _characterControllerComponent;

        [Header("Movement Variables")]
        [SerializeField] private Vector3 _moveVector;
        [SerializeField] private float _playerSpeed;

        [Header("Grounding Variables")]
        [SerializeField] private bool _isGrounded;
        [SerializeField] private RaycastHit _hit;
        [SerializeField] private float _defaultStepOffset;

        [Header("Jumping Variables")] 
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravityScale = -9.81f;
        [SerializeField] private Vector3 _playerVelocity;

        [Header("Jump Buffer Variables")] 
        [SerializeField] public float JumpBufferLength = 0.1f;
        [SerializeField] private float _jumpBufferCount;

        [Header("Camera Rotation Variables")]
        [SerializeField] private float _rotationSpeed = 10f;

        [Header("Coyote Jump Variables")]
        [SerializeField] private float _hangCounter;
        [SerializeField] public float HangTime = 0.2f;


        // Start is called before the first frame update
        void Start()
        {
            _characterControllerComponent = GetComponent<CharacterController>(); // Get's reference to Unity character controller.
            _defaultStepOffset = _characterControllerComponent.stepOffset;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            _isGrounded = CheckGrounded();
            JumpBuffer();
            CoyoteJump();
            ResetVelocity();
            RotatePlayer();
            ResetStepOffset();
            MovePlayer();
            ApplyJumpAndGravity();
        }
        
        // Simple player movement using WASD keys.
        // Normalized to ensure player cannot have differing movement speed behaviour.
        void MovePlayer()
        {
            Debug.Log(_isGrounded);

            if (_isGrounded)
            {
                _moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            }
            else
            {
                _moveVector = new Vector3(Input.GetAxisRaw("Horizontal") /2, 0, Input.GetAxisRaw("Vertical")).normalized;
            }


            _moveVector = transform.TransformDirection(_moveVector);
            _characterControllerComponent.Move(_moveVector * Time.deltaTime * _playerSpeed);
        }

        // Ensures that the jump buffer count and hang counter are in use when applying gravity.
        void ApplyJumpAndGravity()
        {
            if (_jumpBufferCount > 0 && _playerVelocity.y < 1 && _hangCounter > 0f)
            {
                _characterControllerComponent.stepOffset = 0f;
                _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityScale);
                _jumpBufferCount = 0;
            }

            if (!_isGrounded)
            {
                _playerVelocity.y += _gravityScale * Time.deltaTime;
            }

            _characterControllerComponent.Move(_playerVelocity * Time.deltaTime);
        }


        void ResetVelocity()
        {
            if (_isGrounded && _playerVelocity.y < 1)
            {
                _playerVelocity.y = 0f;
            }
            if (_isGrounded && _playerVelocity.z > 0)
            {
                _playerVelocity.z = 0f;
            }
        }

        // Simply rotates the camera and the player.
        void RotatePlayer()
        {
            float horizontal = Input.GetAxisRaw("Mouse X") * _rotationSpeed;
            transform.Rotate(0, horizontal, 0);
        }

        // Allows the player to jump whilst they aren't technically grounded.
        void JumpBuffer()
        {
            if (Input.GetButtonDown("Jump"))
            {
                _jumpBufferCount = JumpBufferLength;
            }
            else
            {
                _jumpBufferCount -= Time.deltaTime;
            }
        }

        // Reduces the coyote jump counter while the player jumps off of a ledge.
        // If they're grounded the timer goes back to the number allocated.
        void CoyoteJump()
        {
            if (_isGrounded)
            {
                _hangCounter = HangTime;
            }
            else
            {
                _hangCounter -= Time.deltaTime;
            }
        }

        // Has a different step offset in the air to stop some glitchy steps on the floor.
        private void ResetStepOffset()
        {
            if (_isGrounded)
            {
                _characterControllerComponent.stepOffset = _defaultStepOffset;
            }
        }

        // Simple grounding checking function. Uses character controller.
        bool CheckGrounded()
        {
            return _characterControllerComponent.isGrounded;
        }
    }
}
