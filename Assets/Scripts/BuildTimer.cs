using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildTimer : MonoBehaviour
{
    public delegate void EndedTimer();
    public EndedTimer endedTimerEvent;
    [SerializeField] private GameObject timePanel;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private float currentDurationTimer;

    private Coroutine timerCoroutine;
    public void StartTimer(float time)
    {
        timePanel.SetActive(true);
        currentDurationTimer = time;
        timerCoroutine = StartCoroutine(Timer());
    }
    public void CancelTimer()
    {
        StopCoroutine(timerCoroutine);
        timePanel.SetActive(false);
    }
    public void EndTimer()
    {
        StopCoroutine(timerCoroutine);
        timePanel.SetActive(false);
        endedTimerEvent?.Invoke();
    }
    private IEnumerator Timer()
    {
        while (true)
        {
            if (currentDurationTimer == 0)
            {
                EndTimer();
            }
            currentDurationTimer--;
            timeText.text = currentDurationTimer.ToString();
            yield return new WaitForSeconds(1);
        }
    }
}
