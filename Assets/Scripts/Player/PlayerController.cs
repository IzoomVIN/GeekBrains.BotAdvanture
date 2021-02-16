using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(
        typeof(Rigidbody), 
        typeof(Collider), 
        typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject scripts;

        private Animator _animator;
        private PlayerManager _manager;
        private Rigidbody _rigidBody;

        private Quaternion _rotation;
        private bool _onPlace;
        private bool _animMove;
        private float _animSpeed;
    
        private const string
            IsMoveName = "isMove",
            JumpName = "isJump",
            MoveSpeedName = "MoveSpeed";
    
    
        void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _manager = scripts.GetComponent<PlayerManager>();
        }

        private void FixedUpdate()
        {
            if (_onPlace)
            {
                MovementLogic();
                RotationLogic();
                JumpLogic();
            }
        }
    
        private void JumpLogic()
        {
            bool jump = _manager.Jump != 0;

            if (jump)
            {
                Vector3 jumpForce = Vector3.up * _manager.JumpSpeed;
                _rigidBody.AddForce(jumpForce);
            }
        }

        // Calculate speed and bool for move animation and set it to animator of Player 
        private void MovementLogic()
        {
            _animSpeed = _manager.Run != 0f ? _manager.RunCoefficient : 1;
            _animMove = (_manager.HorizontalMove != 0f || _manager.VerticalMove != 0f);
        
            _animator.SetFloat(MoveSpeedName, _animSpeed);
            _animator.SetBool(IsMoveName, _animMove);
        }

        // Calculate quaternion for rotation of Player
        private void RotationLogic()
        {
            // Rotate to Camera rotation
            var quatTo = _manager.StartRotation * Quaternion.AngleAxis(_manager.MouseX, Vector3.up);
            quatTo.Normalize();

            // Additional rotation dependent of control (vector) 
            var addRotateX = _manager.VerticalMove != 0
                ? _manager.HorizontalMove * _manager.VerticalMove
                : _manager.HorizontalMove;
        
            // Turning the additional rotation Vector into Quaternion
            var addRotate = Quaternion.FromToRotation(
                Vector3.forward, 
                new Vector3(
                    addRotateX, 
                    0, 
                    Math.Abs(_manager.VerticalMove)
                ).normalized
            );
        
            // Smooth rotation of the Quaternion from the current direction to the desired
            _rotation = Quaternion.Slerp(
                transform.rotation, 
                quatTo * addRotate, 
                Time.fixedDeltaTime * _manager.RotationSpeed
            );
        }
    
        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("Place"))
                _onPlace = true;
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Place"))
                _onPlace = false;
        }

        // Set Movement and Rotation from 'MovementLogic' into RigitBody
        private void OnAnimatorMove()
        {
            var magnitude = _animator.deltaPosition.magnitude;

            var directionSpeed = Math.Abs(_manager.VerticalMove) + Math.Abs(_manager.HorizontalMove);
            if (_manager.VerticalMove != 0) directionSpeed *= _manager.VerticalMove;
            
            var speedVector = new Vector3(0f, 0f, directionSpeed);
            speedVector.Normalize();
            
            var move = _rigidBody.position + _rotation * speedVector * magnitude;
        
            _rigidBody.MovePosition(move);
            if (_animMove) 
                _rigidBody.MoveRotation(_rotation);
        }
    }
}
