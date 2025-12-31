using System.Collections;
using UnityEngine;

namespace Room
{
    public class DoorComponent : MonoBehaviour
    {
        [SerializeField] private GameObject doorPos;
        private Coroutine rotateCoroutine;
        [SerializeField] private float rotationSpeed = 360f;
        [SerializeField] private bool isRight = false;
        

        public void RotateDoor(float angle)
        {
            if (doorPos is null) doorPos = transform.gameObject;
            if(rotateCoroutine is not null) StopCoroutine(rotateCoroutine);

            rotateCoroutine = StartCoroutine(RotateCoroutine(angle));
        }
        
        private IEnumerator RotateCoroutine(float targetAngle)
        {
            if (isRight) targetAngle = -targetAngle;
            Quaternion startRotation = doorPos.transform.localRotation;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
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