using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGraph : MonoBehaviour
{
    GameObject[] points;
    public GameObject point;
    public GameObject lerpGO;
    public GameObject slerpGO;
    public GameObject slerpText;
    public GameObject easeInOutGO;
    public GameObject smoothStepGO;
    public GameObject smoothStepText;
    public GameObject smoothDampGO;
    public GameObject smoothDampText;
    public float smoothDampTime = 5f;
    public bool showSlerp = false;
    public bool showEaseInOut = false;
    public bool showSmoothStep = false;
    public bool showSmoothDamp = false;

    // Start is called before the first frame update
    void Start()
    {
        int pointCount = 100;

        points = new GameObject[pointCount];

        for (int i=0; i<pointCount; i++)
        {
            points[i] = Instantiate(point, new Vector3(i / 100f, 0, 0), Quaternion.identity);
            float y = 0.5f * Mathf.Sin(i / (float)pointCount*Mathf.PI);
            float x = 0.5f * Mathf.Cos(i / (float)pointCount * Mathf.PI);
            Vector3 pos = new Vector3(x, y, 0);
            points[i].transform.position = pos;

        }

        StartCoroutine("LerpAndSlerp");
    }


    // Update is called once per frame
    void Update()
    {
        slerpGO.SetActive(showSlerp);
        slerpText.SetActive(showSlerp);
        easeInOutGO.SetActive(showEaseInOut);
        smoothStepGO.SetActive(showSmoothStep);
        smoothStepText.SetActive(showSmoothStep);
        smoothDampGO.SetActive(showSmoothDamp);
        smoothDampText.SetActive(showSmoothDamp);
    }

    IEnumerator LerpAndSlerp()
    {
        float targetTravelTime = 5f;
        float timeTravelled = 0;
        Vector3 lerpStart = lerpGO.transform.position;
        Vector3 lerpEnd = new Vector3(lerpStart.x + 1f, lerpStart.y, lerpStart.z);

        Vector3 slerpStart = new Vector3(-0.5f, 0.01f, 0);
        Vector3 slerpEnd = new Vector3(0.5f, 0.01f, 0);

        Vector3 slerpOrigin = slerpGO.transform.position;
        Vector3 easeInOutOrigin = easeInOutGO.transform.position;

        Vector3 smoothStepStart = smoothStepGO.transform.position;
        Vector3 smoothStepEnd = new Vector3(smoothStepStart.x + 1f, smoothStepStart.y, smoothStepStart.z);

        Vector3 smoothDampStart = smoothDampGO.transform.position;
        Vector3 smoothDampEnd = new Vector3(smoothDampStart.x + 1f, smoothDampStart.y, smoothDampStart.z);
        Vector3 smoothDampVelocity = Vector3.zero;
        Vector3 smoothDamp = smoothDampStart;


        while (true)
        {
            float pctTravelled = 0;
            Vector3 lerp;
            Vector3 slerp;
            Vector3 easeInOutSlerp;
            Vector3 smoothStep;

            timeTravelled += Time.deltaTime;
            pctTravelled = timeTravelled / targetTravelTime;

            lerp = Vector3.Lerp(lerpStart, lerpEnd, pctTravelled);
            lerpGO.transform.position = lerp;

            slerp = Vector3.Slerp(slerpStart, slerpEnd, pctTravelled);
            slerp += slerpOrigin - slerpStart; //Move the effect back to where the object was and remove the beginning magnitude.
            slerpGO.transform.position = slerp;

            easeInOutSlerp = Vector3.Slerp(slerpStart, slerpEnd, pctTravelled);
            easeInOutSlerp += easeInOutOrigin - slerpStart;
            easeInOutSlerp.y = easeInOutOrigin.y;
            easeInOutGO.transform.position = easeInOutSlerp;

            smoothStep = smoothStepStart;
            smoothStep.x = Mathf.SmoothStep(smoothStepStart.x, smoothStepEnd.x, pctTravelled);
            smoothStepGO.transform.position = smoothStep;

            smoothDamp = Vector3.SmoothDamp(smoothDamp, smoothDampEnd, ref smoothDampVelocity, smoothDampTime);
            smoothDampGO.transform.position = smoothDamp;

            if (timeTravelled > targetTravelTime)
            {
                timeTravelled = 0;
                smoothDamp = smoothDampStart;
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }
}
