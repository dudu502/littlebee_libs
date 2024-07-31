using System.Collections;
using UnityEngine;

public class ClickPoint : MonoBehaviour
{
    [SerializeField] GameObject Point;
    [SerializeField] float FallingSpeed;
    private Coroutine coroutine;
    void Start()
    {
        
    }
    void OnEnable()
    {
        Cleanup();
        Point.transform.localPosition = Vector3.zero;
        coroutine = StartCoroutine(__DelayDisable());
    }

    void Cleanup()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
    IEnumerator __DelayDisable()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        Cleanup();
    }
    // Update is called once per frame
    void Update()
    {
        Point.transform.localPosition -= new Vector3(0, FallingSpeed, 0);
    }
}
