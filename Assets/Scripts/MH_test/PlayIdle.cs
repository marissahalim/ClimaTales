using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayIdle : MonoBehaviour
{
    public MapLoaderCirc mapLoaderCirc;
    public KnobController years;
    public KnobController months;

    public HistDataTypeSelector dataTypeSelector;

    public bool onlyYears; // loop only through the years
    public bool onlyMonths; // loop only through the months

    public string idleDataType;

    public Button playIdleButton;
    public float idleTime = 5f;
    public float toNextMap = 1f;
    private float lastInputTime;
    private bool isIdle = false;
    private int idleCounter = 0; // Counter for idle time

    private Coroutine idleCounterCoroutine;

    void Start()
    {
        lastInputTime = Time.time;
        StartCoroutine(CheckIdleState());
        playIdleButton.onClick.AddListener(GoIdle);
    }

    void Update()
    {
        if (Input.anyKeyDown || Input.touchCount > 0)
        {
            ResetIdleTimer();
        }
    }

    IEnumerator CheckIdleState()
    {
        while (true)
        {
            if (!isIdle && Time.time - lastInputTime >= idleTime)
            {
                GoIdle();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void GoIdle()
    {
        if (!isIdle)
        {
            isIdle = true;
            idleCounter = 0; // Reset counter
            Debug.Log("Idle state activated");

            // dataTypeSelector.SelectDataTypeByName(idleDataType);

            // Start looping through the maps
            idleCounterCoroutine = StartCoroutine(IncrementIdleCounter());
        }
    }

    IEnumerator IncrementIdleCounter()
    {
        while (isIdle)
        {
            // Loop through all years of month X 
            if (onlyYears && !onlyMonths)
            {
                years.SetValue(years.minValue + idleCounter);
                idleCounter++;
                // reset the idle counter once it reaches the end
                if (years.minValue + idleCounter > years.maxValue)
                {
                    idleCounter = 0;
                }
                yield return new WaitForSeconds(toNextMap);
            }

            // Loop through all months of year X 
            if (onlyMonths && !onlyYears)
            {
                months.SetValue(months.minValue + idleCounter);
                idleCounter++;
                // reset the idle counter once it reaches the end
                if (months.minValue + idleCounter > months.maxValue)
                {
                    idleCounter = 0;
                }

                yield return new WaitForSeconds(toNextMap);
            }

            // Loop through every month of every year
            if (onlyYears && onlyMonths)
            {
                int yearCounter = 0;
                int monthCounter = 0;

                while (isIdle) // Keep looping while idle
                {
                    if (years.minValue + yearCounter >= years.maxValue)
                    {
                        yearCounter = 0; // Reset years when max is reached
                    }

                    if (monthCounter >= 12)
                    {
                        monthCounter = 0; // Reset months after Dec
                        yearCounter++; // Move to next year
                    }

                    months.SetValue(months.minValue + monthCounter);
                    years.SetValue(years.minValue + yearCounter);

                    monthCounter++;

                    yield return new WaitForSeconds(toNextMap);
                }
            }
        }
    }

    void ResetIdleTimer()
    {
        if (isIdle)
        {
            isIdle = false;
            Debug.Log("Idle state reset");

            // mapLoaderCirc.SwitchWindows();

            // Stop the idle counter coroutine
            if (idleCounterCoroutine != null)
            {
                StopCoroutine(idleCounterCoroutine);
            }
        }
        lastInputTime = Time.time;
    }
}
