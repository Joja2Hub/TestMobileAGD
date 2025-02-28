using System.Collections;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Vector3 targetPosition; 
    public float moveDuration = 2f;

    private Vector3 startPosition;
    private bool isMoving = false;

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        if (isMoving) yield break;

        isMoving = true;

        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; 
        isMoving = false;
    }
}