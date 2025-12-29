using System.Collections;
using UnityEngine;

namespace Room
{
    public class DoorController : MonoBehaviour
    {
        [Header("연결된 방 정보")] 
        [SerializeField] private RoomController RoomA;
        [SerializeField] private RoomController RoomB;

        [Header("문 세팅")]
        [SerializeField] private GameObject doorPos;
        private readonly float openAngle = 90f;
        private readonly float closeAngle = 0f;
        private float rotationSpeed = 360f;
        private Coroutine rotateCoroutine;

        private bool isOpen = false;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.transform.tag);
            if (other.transform.tag is not "Player") return;
            
            if(isOpen) return;
            Vector3 Location = transform.position - other.transform.position;
            float dot = Vector3.Dot(Location, transform.forward);


            Open(dot < 0f);
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.tag is not "Player") return;
            
            if(!isOpen) return;
            
            Close();
        }

        private void Open(bool IsBack)
        {
            isOpen = true;
            
            RoomManager.Instance.EnterRoom(IsBack ? RoomA : RoomB);
            
            RotateDoor(IsBack ? -openAngle : openAngle);
        }

        private void Close()
        {
            isOpen = false;
            
            RotateDoor(closeAngle);
        }

        private void RotateDoor(float angle)
        {
            if(rotateCoroutine is not null) StopCoroutine(rotateCoroutine);

            rotateCoroutine = StartCoroutine(RotateCoroutine(angle));
        }

        private IEnumerator RotateCoroutine(float targetAngle)
        {
            
            Quaternion startRotation = doorPos.transform.localRotation;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            float angleDifference = Quaternion.Angle(startRotation, targetRotation);
            
            if (angleDifference < 0.1f)
            {
                doorPos.transform.localRotation = targetRotation;
                rotateCoroutine = null;
                yield break;
            }
            
            float duration = angleDifference / rotationSpeed;
            
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                
                doorPos.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            doorPos.transform.localRotation = targetRotation;
            rotateCoroutine = null;
        }
    }
}