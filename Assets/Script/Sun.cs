using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

    public float duration;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        // click on sun, UI sun fly to sun num and disappear
        GameObject.Destroy(gameObject);

        // click on sun, add sun num 25
        GameManager.instance.ChangeSunNum(25);
    }
}
