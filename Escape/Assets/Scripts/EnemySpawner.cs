using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField]
    private int scoreMileStone = 100;
    private int mileStoneIncreaser = 100;

    public static EnemySpawner instance;

    [SerializeField]
    private Transform[] spawnPos;
    [SerializeField]
    private int policeCarRequired;

    private int lastSpawnPos;

    private int currentPoliceCar;
    //player
    private GameObject target;

    public int CurrentPoliceCar{ get { return currentPoliceCar; } set { currentPoliceCar = value; }}


	void Awake () {
        if (instance == null)
            instance = this;
	}
	
	// Update is called once per frame
	void Update () {

        if (GUIManager.instance.GameStarted == false || GUIManager.instance.GameOver == true)
            return;
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        MileStoneIncreaser();
        if (currentPoliceCar < policeCarRequired)
        {
            SpawnPoliceCar();
        }
	}

    void SpawnPoliceCar()
    {
        GameObject policeCar = ObjectPooling.instance.GetPooledObject("Enemy");
        int r = Random.Range(0, spawnPos.Length);
        while (lastSpawnPos == r)
            r = Random.Range(0, spawnPos.Length);

        policeCar.transform.position = new Vector3(spawnPos[r].position.x, 0, spawnPos[r].position.z);
        policeCar.SetActive(true);
        policeCar.GetComponent<Damage>().DefaultSettings();
        currentPoliceCar++;
    }

    void MileStoneIncreaser()
    {
        if(GUIManager.instance.Score >= scoreMileStone)
        {
            scoreMileStone += mileStoneIncreaser;

            if (policeCarRequired < 8)
            {
                policeCarRequired++;
            }
        }
    }
}
