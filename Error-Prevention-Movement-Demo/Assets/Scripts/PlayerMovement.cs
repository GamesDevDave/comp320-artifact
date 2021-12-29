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



        // Start is called before the first frame update
        void Start()
        {
            _distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
            _characterControllerComponent = GetComponent<CharacterController>();
        }

        void FixedUpdate()
        {
            _isGrounded = CheckGrounded();
        }

        // Update is called once per frame
        void Update()
        {
            MovePlayer();
            ApplyJumpAndGravity();
            ResetVelocity();
        }

        bool CheckGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.1f);
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
                _moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            }


            _moveVector = transform.TransformDirection(_moveVector);
            _characterControllerComponent.Move(_moveVector * Time.deltaTime * _playerSpeed);
        }

        void ApplyJumpAndGravity()
        {
            if (_playerVelocity.y < 1 && Input.GetButtonDown("Jump") && CheckGrounded())
            {
                _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityScale);
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
    }
}
