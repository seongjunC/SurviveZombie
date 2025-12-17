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
                Debug.Log("Shoot");
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
            player.mainCamera.gameObject.SetActive(false);
            player.aimCamera.gameObject.SetActive(true);
            Vector3 targetForward = player.aimCamera.transform.forward;
            targetForward.y = 0;
            if (targetForward == Vector3.zero) return;
            
            Quaternion targetRotation = Quaternion.LookRotation(targetForward);
            player.aimCamera.transform.rotation = Quaternion.Slerp(
                player.aimCamera.transform.rotation,
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
            
            Vector3 Direction = new Vector3(x, 0, z).normalized;
            
            Quaternion targetRotation = Quaternion.LookRotation(Direction);
            player.transform.rotation = 
                Quaternion.Slerp(
                    player.transform.rotation, 
                    targetRotation, 
                    Time.deltaTime * player.turnSpeed);
            
            player.characterController.Move(player.aimMoveSpeed * Time.deltaTime * Direction);
            
            player.animator.SetFloat(InputX, x, 0.1f, Time.deltaTime);
            player.animator.SetFloat(InputY, z, 0.1f, Time.deltaTime);
        }

        public override void Exit()
        {
            player.animator.SetBool(isAim, false);
            
            player.mainCamera.gameObject.SetActive(true);
            player.aimCamera.gameObject.SetActive(false);
            // TODO : 카메라 줌 아웃, 조준 UI 비활성화 등
        }
    }
}