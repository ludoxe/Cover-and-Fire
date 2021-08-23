using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{

    public static bool FingerUp;
    public static bool FingerDown;

    public static bool FingerHold;

    public static bool SwipeUp;
    public static bool SwipeDown;

    public static bool SwipeLeft;
    public static bool SwipeRight;


    public static bool ControlLockerAll = false;
    public static bool ControlLockerActions = false;

    public void SetInputTouch(string TouchName)
    {
        switch (TouchName)
        {
            case "SwipeUp":
                {
                    StartCoroutine(ChangeBool());
                    return;

                    IEnumerator ChangeBool()
                    {
                        SwipeUp = true;
                        yield return new WaitForEndOfFrame();
                        SwipeUp = false;
                    }

                }
            case "SwipeDown":
                {
                    StartCoroutine(ChangeBool());
                    return;

                    IEnumerator ChangeBool()
                    {
                        SwipeDown = true;
                        yield return new WaitForEndOfFrame();
                        SwipeDown = false;
                    }
                }
            case "SwipeLeft":
                {
                    StartCoroutine(ChangeBool());
                    return;

                    IEnumerator ChangeBool()
                    {
                        SwipeLeft = true;
                        yield return new WaitForEndOfFrame();
                        SwipeLeft = false;
                    }

                }
            case "SwipeRight":
                {
                    StartCoroutine(ChangeBool());
                    return;

                    IEnumerator ChangeBool()
                    {
                        SwipeRight = true;
                        yield return new WaitForEndOfFrame();
                        SwipeRight = false;
                    }
                }
            case "FingerUp":
                {
                    StartCoroutine(ChangeBool());
                    return;

                    IEnumerator ChangeBool()
                    {
                        FingerUp = true;
                        yield return new WaitForEndOfFrame();
                        FingerUp = false;
                    }
                }
            case "FingerDown":
                {
                    StartCoroutine(ChangeBool());
                    return;

                    IEnumerator ChangeBool()
                    {
                        FingerDown = true;
                        yield return new WaitForEndOfFrame();
                        FingerDown = false;
                    }
                }
            case "FingerOld":
                {
                    StartCoroutine(ChangeBool());
                    return;


                    IEnumerator ChangeBool()
                    {
                        FingerHold = true;
                        while (FingerUp == false) yield return null;
                        FingerHold = false;
                        yield return null;
                    }
                }

            default:

                Debug.LogError(TouchName + " is not valid");
                return;
        }
    }

    public static void SetLockControlAll(bool myBool)
    {
        ControlLockerAll = myBool;
    }
    public static void SetLockControlActions(bool myBool)
    {
        ControlLockerActions = myBool;
    }
}
