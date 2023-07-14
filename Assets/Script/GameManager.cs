using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public int sunNum;

    public GameObject bornParent;

    public GameObject zombiePrefab;

    public float createZombieTime;

    private int zOrderIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        UIManager.instance.InitUI();

        CreateZombie();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSunNum(int changeNum)
    {
        sunNum += changeNum;
        if (sunNum <= 0)
        {
            sunNum = 0;
        }

        UIManager.instance.UpdateUI();
    }

    public void CreateZombie()
    {
        //print("create method");
        StartCoroutine(DelayCreateZombie());
    }

    IEnumerator DelayCreateZombie()
    {
        // await
        //print("await");
        yield return new WaitForSeconds(createZombieTime);

        // create zombie
        //print("create zb");
        GameObject zombie = Instantiate(zombiePrefab);
        int index = Random.Range(0, 5);
        Transform zombieLine = bornParent.transform.Find("born" + index.ToString());
        zombie.transform.parent = zombieLine;
        zombie.transform.localPosition = Vector3.zero;
        zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
        zOrderIndex += 1;

        // start timer
        StartCoroutine(DelayCreateZombie());
    }
}
