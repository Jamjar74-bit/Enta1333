using System.Collections;
using UnityEngine;

public class CoroutineExample : MonoBehaviour
{
    //function1();
    //function2();

    //[SerializeField]
    IEnumerator SampleCoroutine(float f, string s)
    {
        yield return new WaitForSeconds(f);
        Debug.Log("Coroutine Finished" + s);

    }

    private void Start()
    {
        StartCoroutine(SampleCoroutine(2f, "#1"));
        StartCoroutine(SampleCoroutine(1f, "#2"));
    }


   





}
