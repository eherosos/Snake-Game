using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public static class ShakeUI// : MonoBehaviour
{
	public static IEnumerator Shake (Transform obj, float duration, float magnitude)
    {
        Vector3 originalPos = obj.localPosition;
        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            obj.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        obj.localPosition = originalPos;
	}
}
