using UnityEngine;

namespace Player
{
    public class PlayerAimState : PlayerStateBase
    {
        public static readonly int isAim = Animator.StringToHash("isAiming");
        private static readonly int InputX = Animator.StringToHash("InputX");
        private static readonly int InputY = Animator.StringToHash("InputY");

        public PlayerAimState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            player.animator.SetBool(isAim, true);
            
            RotateCamera();
            // TODO : 카메라 줌 인, 조준 UI 활성화 등
        }

        public override void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                player.Shoot();
                return;
            }

            if (Input.GetMouseButtonUp(1))
            {
                float x = Input.GetAxisRaw("Horizontal");
                float y = Input.GetAxisRaw("Vertical");

                if (Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f)
                {
                    stateMachine.ChangeState(player.GetState<PlayerMoveState>());
                }
                else
                {
                    stateMachine.ChangeState(player.GetState<PlayerIdleState>());
                }

                return;
            }
            RotateCamera();
            HandleMovement();
        }

        private void RotateCamera()
        {
            Vector3 targetForward = player.cameraTransform.forward;
            targetForward.y = 0;
            if (targetForward == Vector3.zero) return;
            
            Quaternion targetRotation = Quaternion.LookRotation(targetForward);
            player.cameraTransform.rotation = Quaternion.Slerp(
                player.cameraTransform.rotation,
                targetRotation,
                player.turnSpeed * Time.deltaTime
            );
        }

        private void HandleMovement()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            
            if (Mathf.Abs(x) < 0.1f && Mathf.Abs(z) < 0.1f)
            {
                player.animator.SetFloat(InputX, 0, 0.1f, Time.deltaTime);
                player.animator.SetFloat(InputY, 0, 0.1f, Time.deltaTime);
                return;
            }
            
            Vector3 camForward = player.cameraTransform.forward;
            Vector3 camRight = player.cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = (camForward * z + camRight * x).normalized;
            
            player.characterController.Move(player.aimMoveSpeed * Time.deltaTime * moveDir);
            
            player.animator.SetFloat(InputX, x, 0.1f, Time.deltaTime);
            player.animator.SetFloat(InputY, z, 0.1f, Time.deltaTime);
        }

        public override void Exit()
        {
            player.animator.SetBool(isAim, false);
            
            // TODO : 카메라 줌 아웃, 조준 UI 비활성화 등
        }
    }
}