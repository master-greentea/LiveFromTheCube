using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAppScript : MonoBehaviour
{
    public Vector3 pointB;
    public Vector3 pointA;
    public Transform box;

    IEnumerator Start() {
        var point = transform.position;

        while (true) {
            yield return StartCoroutine(MoveObject(transform, pointA, pointB, point, 3.0f));
            yield return StartCoroutine(MoveObject(transform, pointB, pointA, point, 3.0f));
        }
    }

    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, Vector3 point, float time) {


        if (gameObject.tag == ("Left Hand")) {
            var i = 0.0f;
            var rate = 3.0f / time;
            while (i < 1.0f) {
                i += Time.deltaTime * rate;
                thisTransform.position = Vector3.Lerp(startPos, endPos, i);
                yield return null;
            }
        } else {
            var i = 0.0f;
            var rate = 3.0f / time;
            while (i < 1.0f) {
                i += Time.deltaTime * rate;
                thisTransform.position = Vector3.Lerp(endPos, startPos, i);
                yield return null;
            }
        }

    }
}
