using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HelixController : MonoBehaviour
{
    public GameObject helixLevelPrefab;
    public List<Stage> allStages = new List<Stage>();
    public Transform GoalTransform;
    public Transform topTransform;

    private float helixDistance;
    private List<GameObject> spawnedLevels = new List<GameObject>();
    private Vector2 lastTapPos;
    private Vector3 startRotation;


    // Start is called before the first frame update
    void Awake()
    {
       // Instantiate(helixLevelPrefab);
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (GoalTransform.localPosition.y + 0.1f);
        LoadStage(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector2 curTapPos = Input.mousePosition;
            if(lastTapPos == Vector2.zero)
            {
                lastTapPos = curTapPos;
            }

            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);
        }

        if(Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector3.zero;
        }
    }


    public void LoadStage(int stageNumber)
    {
        Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];
       
        if(stage == null)
        {
            Debug.LogError("No stage" + stageNumber + "found in all stages list.Are all stages assigned in the list?");
            return;
        }

        //  Set the new background color(on camara object)
        Camera.main.backgroundColor = allStages[stageNumber].stageBackgroundColor;

        //change the color of the ball
        FindObjectOfType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].stageballColor;

        //  Reset helix rotation
        transform.localEulerAngles = startRotation;

        //  Destroy the old levels if there are any
        foreach (GameObject go in spawnedLevels)
        {
            Destroy(go);
        }

        //  create new levels / plateforms
        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;

            //  creates level within scene
            GameObject level = Instantiate(helixLevelPrefab, transform);
            Debug.Log("Levels spawned");

            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);

            //  Disable some parts (depending on level setup) (creating the gaps)
            int partsToDisable = 12 - stage.levels[i].partCount;
            List<GameObject> disabledParts = new List<GameObject>();

            //  Debug.Log("Should disable " + partsToDisable);

            while (disabledParts.Count< partsToDisable)
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0,level.transform.childCount)).gameObject;
                if(!disabledParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                    //  Debug.Log("Disabled Part");
                }
            }


            // Mark parts as death parts
            List<GameObject> leftParts = new List<GameObject>();
            foreach(Transform  t in level.transform)
            {
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor; // Set color of part

                if (t.gameObject.activeInHierarchy)
                {
                    leftParts.Add(t.gameObject);
                }
            }

            //  Debug.Log(leftParts.Count + " parts left");


            // Creating the deathparts
            List<GameObject> deathParts = new List<GameObject>();
            while (deathParts.Count < stage.levels[i].deathPartCount)
            {
                GameObject randomPart = leftParts[(Random.Range(0,leftParts.Count))];
                if(!deathParts.Contains(randomPart))
                {
                    randomPart.gameObject.AddComponent<DeathPart>();
                    deathParts.Add(randomPart);
                   // Debug.Log("Set death part");
                }
            }
        }
    }
}
