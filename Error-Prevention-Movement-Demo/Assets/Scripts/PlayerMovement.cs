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
        [SerializeField] private float _distToGround;

        [Header("Jumping Variables")] 
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravityScale = -9.81f;
        [SerializeField] private Vector3 _playerVelocity;

        [Header("Jump Buffer Variables")] 
        [SerializeField] private float jumpBufferLength = 0.1f;
        [SerializeField] private float _jumpBufferCount;

        [Header("Camera Rotation Variables")]
        [SerializeField] private float _rotationSpeed = 10f;


        // Start is called before the first frame update
        void Start()
        {
            _distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
            _characterControllerComponent = GetComponent<CharacterController>();
            Cursor.visible = false;
        }

        void FixedUpdate()
        {
            _isGrounded = CheckGrounded();
        }

        // Update is called once per frame
        void Update()
        {
            JumpBuffer();
            ResetVelocity();
            RotatePlayer();
            MovePlayer();
            ApplyJumpAndGravity();
        }

        bool CheckGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.3f);
        }

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

        void ApplyJumpAndGravity()
        {
            if (_jumpBufferCount > 0 && _playerVelocity.y < 1 && CheckGrounded())
            {
                _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityScale);
                _jumpBufferCount = 0;
            }

            _playerVelocity.y += _gravityScale * Time.deltaTime;
            _characterControllerComponent.Move(_playerVelocity * Time.deltaTime);
        }

        void ResetVelocity()
        {
            if (CheckGrounded() && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }
            if (CheckGrounded() && _playerVelocity.z > 0)
            {
                _playerVelocity.z = 0f;
            }
        }

        void RotatePlayer()
        {
            float horizontal = Input.GetAxisRaw("Mouse X") * _rotationSpeed;
            transform.Rotate(0, horizontal, 0);
        }

        void JumpBuffer()
        {
            if (Input.GetButtonDown("Jump"))
            {
                _jumpBufferCount = jumpBufferLength;
            }
            else
            {
                _jumpBufferCount -= Time.deltaTime;
            }
        }

        void CoyoteJump()
        {

        }
    }
}
