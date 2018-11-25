using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Interpolate
{

    /* 
     * User is expected to pass in their own measure of time as first var, in 
     * order for this function to be used with update.
     */
    public float PerformLerp(ref float elapsed, float a, float b, float runtime)
    {
        elapsed += Time.deltaTime;

        if (elapsed >= runtime)
        {
            return b;
        }
        else
        {
            return Mathf.Lerp(a, b, elapsed / runtime);
        }

    }
    #region ProduceCurve

    /* ProduceCurve(T[] horizontal, T[] vertical, int size)
     * 
     * Returns an animation curve based on arrays representing horizontal and
     * vertical axes. Return null if there is a difference in length in the
     * input arrays.
     * 
     */
    public AnimationCurve ProduceCurve(float[] horizontal, float[] vertical, int points)
    {

        if (horizontal.Length == points && vertical.Length == points)
        {
            Keyframe[] kf = new Keyframe[points];

            for (int i = 0; i < points; i++)
            {
                kf[i] = new Keyframe(horizontal[i], vertical[i]); // Keyframe(time, value, intangent,outtangent,inweight,outweight
            }
            AnimationCurve curve = new AnimationCurve(kf);


            return curve;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Mismatch of arrays using ProduceCurve ");
#endif
            return null;
        }
    }


    public AnimationCurve ProduceCurve(int[] horizontal, int[] vertical, int points)
    {

        if (horizontal.Length == points && vertical.Length == points)
        {
            Keyframe[] kf = new Keyframe[points];

            for (int i = 0; i < points; i++)
            {
                kf[i] = new Keyframe(horizontal[i], vertical[i]);
            }
            AnimationCurve curve = new AnimationCurve(kf);


            return curve;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Mismatch of arrays using ProduceCurve ");
#endif
            return null;
        }
    }
    #endregion

    #region coroutine
    /* 
     * Expected inputDelegate is: x => outputVar = x
     * 
     * Lerps. Overridden to support Float, Color, Vector2, Vector3, Vector4
     * 
     * Source: 
     * https://forum.unity.com/threads/passing-ref-variable-to-coroutine.379640/
     * https://forum.unity.com/threads/generic-coroutines-and-actions.378154/
     */

    // Float
    public static IEnumerator FloatCoroutine(Action<float> inputDelegate, 
                                             float start, float target, float runtime) {
        float nextValue = 0f;
        float startTime = Time.time;
        float endTime = startTime + runtime;

        /* Should result in slightly faster calculation during while loop than
         * division each iteration */
        float runtimeReciprocal = 1 / runtime;

        while (Time.time < endTime) {

            nextValue = Mathf.Lerp(start, target, (Time.time - startTime)* runtimeReciprocal);
            inputDelegate(nextValue);
            yield return null;
        }

        nextValue = target;
        inputDelegate(nextValue);
    }

    // Color
    public static IEnumerator ColorCoroutine(Action<Color> inputDelegate, 
                                             Color start, Color target, float runtime)
    {
        Color nextValue = new Color();

        float startTime = Time.time;
        float endTime = startTime + runtime;

        /* Should result in slightly faster calculation during while loop than
         * division each iteration */
        float runtimeReciprocal = 1 / runtime;

        while (Time.time < endTime)
        {
            nextValue = Color.Lerp(start, target, (Time.time - startTime) * runtimeReciprocal);
            inputDelegate(nextValue);
            yield return null;
        }

        nextValue = target;
        inputDelegate(nextValue);
    }

    // Vector2
    public static IEnumerator Vector2Coroutine(Action<Vector2> inputDelegate, 
                                               Vector2 start, Vector3 target, float runtime)
    {
        Vector2 nextValue = new Vector2();

        float startTime = Time.time;
        float endTime = startTime + runtime;

        /* Should result in slightly faster calculation during while loop than
         * division each iteration */
        float runtimeReciprocal = 1 / runtime;

        while (Time.time < endTime)
        {
            nextValue = Vector2.Lerp(start, target, (Time.time - startTime) * runtimeReciprocal);
            inputDelegate(nextValue);
            yield return null;
        }

        nextValue = target;
        inputDelegate(nextValue);
    }

    // Vector3
    public static IEnumerator Vector3Coroutine(Action<Vector3> inputDelegate, 
                                               Vector3 start, Vector3 target, float runtime)
    {
        Vector3 nextValue = new Vector3();

        float startTime = Time.time;
        float endTime = startTime + runtime;

        /* Should result in slightly faster calculation during while loop than
         * division each iteration */
        float runtimeReciprocal = 1 / runtime;

        while (Time.time < endTime)
        {
            nextValue = Vector3.Lerp(start, target, (Time.time - startTime) * runtimeReciprocal);
            inputDelegate(nextValue);
            yield return null;
        }

        nextValue = target;
        inputDelegate(nextValue);
    }

    // Vector4
    public static IEnumerator Vector4Coroutine(Action<Vector4> inputDelegate, 
                                               Vector4 start, Vector4 target, float runtime)
    {
        Vector4 nextValue = new Vector4();

        float startTime = Time.time;
        float endTime = startTime + runtime;

        /* Should result in slightly faster calculation during while loop than
         * division each iteration */
        float runtimeReciprocal = 1 / runtime;

        while (Time.time < endTime)
        {
            nextValue = Vector4.Lerp(start, target, (Time.time - startTime) * runtimeReciprocal);
            inputDelegate(nextValue);
            yield return null;
        }

        nextValue = target;
        inputDelegate(nextValue);
    }

    #endregion
}
