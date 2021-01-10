using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelScript : MonoBehaviour
{
    int maxLevel = 15;
    [SerializeField] GameObject levelPrefab;
    [SerializeField] GameObject canvas;
    
    float verticalHorizontalGap = 30;
    float heightOfCard = 260;
    float widthOfCard = 240;
    float viewPortHeight = 1600;
    float viewPortWidth = 900;
    float reservedHeight = 320;
    float reservedWidth = 300;
    int max_levels = 15;
    public void startLevel(int level)
    {
        //Debug.Log("LoadLevel" + " " + level.ToString());
        int num_patients =  0, num_failures = 0, total_time=0, lastRoom=0, score = 500;
        float spawnInterval=5;
        int waitingRoompenalty = 30;
        //Add speed
        switch(level){
            default:
                break;
            case 1:
                num_patients = 4;
                num_failures = 0;
                total_time = 2;
                lastRoom = 102;
                spawnInterval = 15;
                break;
            case 2:
                num_patients = 6;
                num_failures = 0;
                total_time = 2;
                lastRoom = 103;
                score = 1000;
                spawnInterval = 13;
                break;
            case 3:
                num_patients = 3;
                num_failures = 2;
                total_time = 3;
                lastRoom = 104;
                score = 1000;
                spawnInterval = 10;
                break;
            case 4:
                num_patients = 5;
                num_failures = 3;
                total_time = 4;
                lastRoom = 204;
                score = 1500;
                spawnInterval = 7;
                break;
            case 5:
                num_patients = 8;
                num_failures = 5;
                total_time = 6;
                lastRoom = 204;
                score = 2000;
                spawnInterval = 5;
                break;
        }
        total_time*=60;
        Helper obj = transform.GetComponent<Helper>();
        int[] failure_ins = obj.SetFailure(num_failures, total_time);
        float[] patientArray = obj.CreatePatients(num_patients,level, lastRoom);
        
        float startTime = 0, lastUpdate = 1, alpha = 1, currentTime = 0, rating = 0;
        
        int[] inventory = new int[11];
        for(int i=0;i<11;i++){
            inventory[i] = 0;
        }
        SaveData data = new SaveData(patientArray,inventory, failure_ins,lastRoom,startTime, lastUpdate, alpha, currentTime, score, rating, total_time, level, spawnInterval, waitingRoompenalty);
        string name = "level"+level.ToString();
        SaveSystem<SaveData>.SavePlayer(data, name);
        
        SceneManager.LoadScene("GameScene");
        // mainCamera.GetComponent<MainScript>().resumeGame = false;
        
        MainScript.resumeFile = name;
        // mainCamera.GetComponent<MainScript>().LoadGame(name);
        //call save, call load
    }
    private void Start()
    {
        int xLevels = Mathf.RoundToInt(viewPortWidth / reservedWidth);
        int yLevels = Mathf.RoundToInt(viewPortHeight / reservedHeight);
        
        // SaveGlobal data = SaveSystem<SaveGlobal>.LoadPlayer("global");
        // if(data==null){
        //     data = new SaveGlobal(1,0f);
        // }
        for(int i = 0; i< maxLevel; i++)
        {
            GameObject levelCard = Instantiate(levelPrefab, canvas.transform);
            int levelNumber = i + 1;
            levelCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(verticalHorizontalGap + reservedWidth * (i % xLevels), -(verticalHorizontalGap + reservedHeight * (i / xLevels)));
            // if(data.farthest_level>=levelNumber){
            //     levelCard.transform.Find("Lock").gameObject.SetActive(false);
            //     //activate level
            //     //break;
            // }
            // else{
            //     levelCard.GetComponent<Button>().interactable = false;
            //     //lock
            //     //break;
            // }
            // data.close();
            levelCard.GetComponent<Button>().onClick.AddListener(() => startLevel(levelNumber));
           
            levelCard.transform.GetChild(0).gameObject.GetComponent<Text>().text = levelNumber.ToString();
        }
        //startLevel(1);
    }
}
