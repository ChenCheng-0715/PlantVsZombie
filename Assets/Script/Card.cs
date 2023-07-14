using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    public GameObject objectPrefab;

    private GameObject curGameObject;

    private GameObject darkBg;

    private GameObject progressBar;

    public float waitTime;

    public int useSun;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        darkBg = transform.Find("dark").gameObject;
        progressBar = transform.Find("progress").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        UpdateProgress();
        UpdateDarkBg();
    }

    void UpdateProgress()
    {
        float per = Mathf.Clamp(timer / waitTime, 0, 1);
        progressBar.GetComponent<Image>().fillAmount = 1 - per;
    }


    void UpdateDarkBg()
    {
        
        if (progressBar.GetComponent<Image>().fillAmount == 0 && GameManager.instance.sunNum >= useSun)
        {
            darkBg.SetActive(false);
        }
        else
        {
            darkBg.SetActive(true);
        }
    }

    public void OnBeginDrag(BaseEventData data)
    {
        // if dark bg is active, means unable to use plant
        if (darkBg.activeSelf)
        {
            return;
        }
        
        PointerEventData pointerEventData = data as PointerEventData;
        curGameObject = Instantiate(objectPrefab);
        curGameObject.transform.position = TranslateScreenToWorld(pointerEventData.position);
    }


    public void OnDrag(BaseEventData data)
    {
        if (curGameObject == null)
        {
            return;
        }
        PointerEventData pointerEventData = data as PointerEventData;
        curGameObject.transform.position = TranslateScreenToWorld(pointerEventData.position);
    }


    public void OnEndDrag(BaseEventData data)
    {
        
        if (curGameObject == null)
        {
            return;
        }
        PointerEventData pointerEventData = data as PointerEventData;
        // get mouth curser position collider object
        Collider2D[] col = Physics2D.OverlapPointAll(TranslateScreenToWorld(pointerEventData.position));
        // loop collider object
        foreach (Collider2D c in col)
        {
            // if object is land, and no plant on land
            if (c.tag == "Land" && c.transform.childCount == 0)
            {
                // add current object to land child object
                curGameObject.transform.parent = c.transform;
                curGameObject.transform.localPosition = Vector3.zero;
                curGameObject.GetComponent<Plant>().SetPlantStart();
                // reset default
                curGameObject = null;
                // finish planting, consume sun used
                GameManager.instance.ChangeSunNum(-useSun);
                timer = 0;

                break;
            }
        }

        // if no land avail, and current object still exist, destroy it
        if (curGameObject != null)
        {
            GameObject.Destroy(curGameObject);
            curGameObject = null;
        }
    }

    public static Vector3 TranslateScreenToWorld(Vector3 position)
    {
        Vector3 cameraTranslatePos = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(cameraTranslatePos.x, cameraTranslatePos.y, 0);
    }
}
