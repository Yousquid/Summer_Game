using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class Utils 
{
    public static void DisableAfterSeconds(GameObject target, float delay)
    {
        CoroutineRunner.Instance.StartCoroutine(DisableRoutine(target, delay));
    }

    private static IEnumerator DisableRoutine(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null)
            target.SetActive(false);
    }
}
