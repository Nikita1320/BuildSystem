using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public delegate void TickTimer();
    private TickTimer tickTimerEvent;
    public TickTimer TickTimerEvent => tickTimerEvent;
    void Start()
    {
        //Timer();
    }

    private IEnumerator Timer()
    {
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(1);
            i++;
            if (i == 5)
            {
                //LoadTimeToFileForKnowWhenPlayerExitGame
            }
            tickTimerEvent?.Invoke();
        }
    }
}
