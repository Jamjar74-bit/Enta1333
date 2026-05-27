using System.Collections;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    private Light _lightComponent;

    //define an enum to represent discrete light states
    private enum LightState
    {
        ON,        //0
        OFF,       //1
        FLICKERING //2
    }

    [SerializeField]
    private LightState _currentState;
    [SerializeField]
    private LightState _defaultState;

    private void Start()
    {
        _lightComponent = GetComponent<Light>();
        _currentState = _defaultState;
        ChangeState(_currentState);
    }

    private void ChangeState(LightState newState)
    {
        Debug.Log("Change State to: " + newState.ToString());
        StopAllCoroutines();

        switch (newState)
        {
            case LightState.ON:
                StartCoroutine(OnState());
                break;
            case LightState.OFF:
                StartCoroutine(OffState());
                break;
            case LightState.FLICKERING:
                StartCoroutine(FlickeringState());
                break;
        }
    }

    IEnumerator OnState()
    {
        Debug.Log("ON State: Enter");
        _lightComponent.enabled = true;
        _lightComponent.intensity = 4f;

        while (_currentState == LightState.ON)
        {
            yield return null;
            if (Input.GetMouseButton(0))
            {
                _currentState = LightState.OFF;
            }
        }

        Debug.Log("ON State: Exit");
        ChangeState(_currentState);
    }

    IEnumerator OffState()
    {
        Debug.Log("OFF State: Enter");
        _lightComponent.enabled = false;

        while (_currentState == LightState.OFF)
        {
            yield return null;
            if (Input.GetMouseButton(0))
            {
                _currentState = LightState.ON;
            }
        }

        Debug.Log("OFF State: Exit");
        ChangeState(_currentState);
    }

    IEnumerator FlickeringState()
    {
        Debug.Log("FLICKERING State: Enter");

        while (_currentState == LightState.FLICKERING)
        {

            if (Random.value < 0.5f)
            {
                float flickerDuration = Random.Range(3f, 5f);
                _lightComponent.enabled = true;
                _lightComponent.intensity = Random.Range(2f, 5f);
                yield return new WaitForSeconds(flickerDuration);

                _lightComponent.enabled = false;
                yield return new WaitForSeconds(Random.Range(3f, 5f));


                if (Input.GetMouseButton(0))
                {
                    _currentState = LightState.OFF;
                }
            }

            Debug.Log("FLICKERING State: Exit");
            ChangeState(_currentState);
        }
    }
}