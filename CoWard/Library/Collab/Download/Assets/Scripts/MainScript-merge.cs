using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainScript : MonoBehaviour
{
    public static bool resumeGame;
    public static string resumeFile;
    [SerializeField] GameObject ShopUI;
    [SerializeField] GameObject NotificationBar;
    [SerializeField] GameObject prefabPatient;
    [SerializeField] GameObject waitingRoom;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject timer;
    [SerializeField] GameObject Lobby;
    [SerializeField] GameObject Carrier;
    [SerializeField] GameObject ExitScreenScore, ExitScreenRating;

    Text timer_obj, notification, exit_score, exit_rating;
    public int total_time = 180;
    public int level;
    // int[] failure_ins = new int[num_failures];
    public float startTime, lastUpdate, alpha = 1; public int lastRoom;
    bool spawned = false;
    public bool movingPatient = false;
    [HideInInspector] public GameObject movingPatientObject;
    Vector2 initialPatientPosition;
    GameObject originalSlotObject;
    public int waitingRoom_penalty = 30;
    // [SerializeField] GameObject CommonKit;
    //Ward
    [SerializeField] GameObject prefabRoom;
    [SerializeField] GameObject wardContent;
    float hospitalHeight;
    float hospitalWidth;
    float windowHeight = 860;
    float windowWidth = 860;
    float viewPortHeight = 1600;
    float viewPortWidth = 900;
    float reservedRoomSize = 600;
    float roomSize = 500;
    float roomGap;
    int roomsPerFloor = 4;
    //int floors;
    float minZoom;
    float maxZoom = 1;
    float spawnInterval = 5;
    bool zooming = false;
    bool scrolling = false;
    bool badTouch = false;
    int num_patients, num_failures;
    Vector2 initialScrollPosition;
    Vector2 initialTouchPosition;
    Vector2 initialTouchWorldPosition;

    Vector2 initialDoubleTouchCenter;
    float initialDoubleTouchDifference;
    Vector2 initialWardRectPosition;
    float initialWardScale;
    Vector2 initialWardTouchedPosition;
    
    [SerializeField] GameObject Testing;

    Vector2 itemInitialWorldPosition;
    GameObject itemObj;
    GameObject fullItemObj;
    int max_num_failures = 2;
    bool applying;
    // int room_limit = 100, floor_limit = 10;
    int patientProperties = 21;
    GameObject activeCard = null;
    public  int[] failure_ins;
    [SerializeField]
    GameObject exit_screen, help_screen;
    [SerializeField]
    GameObject HelpButton;

    public int[] SetFailure(int num_failures){
        failure_ins = new int[num_failures];
        int offset = 15;
        for(int i=0;i<num_failures;i++){
            failure_ins[i] = Random.Range(offset,total_time-offset);
        }
        System.Array.Sort<int>(failure_ins, new System.Comparison<int>( 
                  (i1, i2) => i2.CompareTo(i1)));
        return failure_ins;
    }
    public float[] CreatePatients(int num_patients, int level){
        int[] roomdetail = new int[(lastRoom/100)*roomsPerFloor*4];
        float[] patientArray = new float[num_patients * patientProperties]; //age, gender, 5 symptoms, severity, prob_sev_inc, prob_sev_dec_base, room_multiplier, position, infected
        // Debug.Log("patients.leng" + patients.Length);
        for(int k = 0; k< num_patients; k++)
        {
            int i = patientProperties * k;
            GameObject obj = Instantiate(prefabPatient, waitingRoom.transform);
            obj.GetComponent<Patient>().Initialize(level);
            Patient curPatient = obj.GetComponent<Patient>();
            patientArray[i] = curPatient.age;
            patientArray[i + 1] = curPatient.gender;
            patientArray[i + 2] = curPatient.fever;
            patientArray[i + 3] = curPatient.cough;
            patientArray[i + 4] = curPatient.tiredness;
            patientArray[i + 5] = curPatient.chest_pain;
            patientArray[i + 6] = curPatient.breathing_difficulty;
            patientArray[i + 7] = curPatient.severity;
            patientArray[i + 8] = curPatient.prob_sev_inc;
            patientArray[i + 9] = curPatient.prob_sev_dec_base;
            patientArray[i + 10] = curPatient.room_multiplier;
            Debug.Log("curPatient.room_multiplier " + patientArray[i + 10]);
            int rv = Random.Range(0,10);
            if(rv<4){
                patientArray[i + 11] = 2; //lobby
            }
            else if(rv<5){
                patientArray[i + 11] = 3; //carrier
            }
            else 
            {
                int floor = Random.Range(1,lastRoom/100); //ward
                int room = Random.Range(1,roomsPerFloor);
                int bed = Random.Range(1,4);
                int ward_bed = 1000*floor+10*room+bed;
                while(roomdetail[ward_bed]==1 || 100*floor+room>lastRoom){
                    floor = Random.Range(1,lastRoom/100);
                    room = Random.Range(1,roomsPerFloor);
                    bed = Random.Range(1,4);
                    ward_bed = 1000*floor+10*room+bed;
                }
                patientArray[i + 11] = ward_bed;
                
                int prob = Random.Range(1,10);
                if(prob>=7){
                    patientArray[i + 15] = Time.time;
                    prob = Random.Range(1,10);
                    if(prob<=7){
                        patientArray[i + 16] = 1;
                        patientArray[i + 17] = obj.GetComponent<Patient>().test_time[0];
                    }
                    else if(prob<=9){
                        patientArray[i + 16] = 2;
                        patientArray[i + 17] = obj.GetComponent<Patient>().test_time[1];
                    }
                    else{
                        patientArray[i + 16] = 3;
                        patientArray[i + 17] = obj.GetComponent<Patient>().test_time[2];
                    }
                    
                    
                }
            }
            
            patientArray[i + 12] = curPatient.infected;
            patientArray[i + 13] = curPatient.mythreshold;
            patientArray[i + 14] = curPatient.max_coins; //change with level
            
            
            patientArray[i + 18] = curPatient.accuracy; 
            patientArray[i + 19] = curPatient.priorPositive;
            patientArray[i + 20] = curPatient.initTime;
            for (int j = 0; j < patientProperties; j++)
            {
                Debug.Log((i + j).ToString() + " " + patientArray[i + j].ToString());
            }
            Debug.Log("end loop 1");
        }

       return patientArray;
    }
    public void SaveGame(string saveName)
    {
        GameObject[]patients = GameObject.FindGameObjectsWithTag("patient");
        float[] patientArray = new float[patients.Length * patientProperties]; //age, gender, 5 symptoms, severity, prob_sev_inc, prob_sev_dec_base, room_multiplier, position, infected
        Debug.Log("patients.leng" + patients.Length);
        for(int k = 0; k< patients.Length; k++)
        {
            int i = patientProperties * k;
            Patient curPatient = patients[k].GetComponent<Patient>();
            patientArray[i] = curPatient.age;
            patientArray[i + 1] = curPatient.gender;
            patientArray[i + 2] = curPatient.fever;
            patientArray[i + 3] = curPatient.cough;
            patientArray[i + 4] = curPatient.tiredness;
            patientArray[i + 5] = curPatient.chest_pain;
            patientArray[i + 6] = curPatient.breathing_difficulty;
            patientArray[i + 7] = curPatient.severity;
            patientArray[i + 8] = curPatient.prob_sev_inc;
            patientArray[i + 9] = curPatient.prob_sev_dec_base;
            patientArray[i + 10] = curPatient.room_multiplier;
            Debug.Log("curPatient.room_multiplier " + patientArray[i + 10]);
            if (curPatient.transform.parent.gameObject.name == "WaitingRoom")
            {
                patientArray[i + 11] = 1;
            }
            else if (curPatient.transform.parent.gameObject.name == "Lobby")
            {
                patientArray[i + 11] = 2;
            }
            else if (curPatient.transform.parent.gameObject.name == "Carrier")
            {
                patientArray[i + 11] = 3;
            }else if (curPatient.transform.parent.gameObject.name.Substring(0, 4) == "Beds")
            {
                patientArray[i + 11] = int.Parse(curPatient.transform.parent.parent.gameObject.name) * 10 + curPatient.transform.parent.gameObject.name[5] - '0';
                Debug.Log("Save patientArray[i + 11] " + patientArray[i + 11]);
            }
            else
            {
                Debug.Log("Slot of buggy patient " + curPatient.transform.parent.gameObject.name);
            }
            patientArray[i + 12] = curPatient.infected;
            patientArray[i + 13] = curPatient.mythreshold;
            patientArray[i + 14] = curPatient.max_coins;
            patientArray[i + 15] = curPatient.startTestTime;
            switch (curPatient.kitName)
            {
                case "CommonKit":
                    patientArray[i + 16] = 1;
                    break;
                case "ExpensiveKit":
                    patientArray[i + 16] = 2;
                    break;
                case "DeluxeKit":
                    patientArray[i + 16] = 3;
                    break;
            }
            patientArray[i + 17] = curPatient.totalTestTime;
            patientArray[i + 18] = curPatient.accuracy;
            patientArray[i + 19] = curPatient.priorPositive;
            patientArray[i + 20] = curPatient.initTime;
            for (int j = 0; j < patientProperties; j++)
            {
                Debug.Log((i + j).ToString() + " " + patientArray[i + j].ToString());
            }
            Debug.Log("end loop 1");
        }

        int[] inventory = new int[Item.num_items];
        for (int i = 0; i < Item.num_items; i++)
        {
            inventory[i] = Store.inventory[Store.stringFromItem((Item.ItemType)i)];
        }
        SaveData data = new SaveData(patientArray, inventory, lastRoom, startTime, lastUpdate, alpha, Time.time, Attributes.score, Attributes.rating, total_time);
        SaveSystem<SaveData>.SavePlayer(data, saveName);
    }

    public void LoadGame(string filename)
    {
        SaveData data = SaveSystem<SaveData>.LoadPlayer(filename);
        lastRoom = data.lastRoom;
        alpha = data.alpha;
        lastUpdate = data.lastUpdate;
        startTime = Time.time - data.currentTime + data.startTime;
        total_time = data.totalTime;
        initializeWard();
        for (int i = 0; i < Item.num_items; i++)
        {
            Store.inventory[Store.stringFromItem((Item.ItemType)i)] = data.inventory[i];
        }
        switch(level){
            default:
                break;
            case 1:
                num_patients = 0;
                num_failures = 0;
                total_time = 2;
                break;
            case 2:
                num_patients = 2;
                num_failures = 0;
                total_time = 2;
                break;
            case 3:
                num_patients = 3;
                num_failures = 2;
                total_time = 3;
                break;
            case 4:
                num_patients = 5;
                num_failures = 3;
                total_time = 4;
                break;
            case 5:
                num_patients = 8;
                num_failures = 5;
                total_time = 6;
                break;
        }
        SetFailure(num_failures);
        ShopUI.GetComponent<Store>().initializeStore();
        timer_obj = timer.GetComponent<Text>();
        timer_obj.text = (total_time - lastUpdate).ToString();
        notification = NotificationBar.GetComponent<Text>();
        waitingRoom.GetComponent<Slot>().ForceStart();
        Lobby.GetComponent<Slot>().ForceStart();
        Carrier.GetComponent<Slot>().ForceStart();
        exit_screen.SetActive(false);
        help_screen.SetActive(false);
        Attributes.score = data.score;
        Attributes.rating = data.rating;

         HelpButton.GetComponent<Button>().onClick.AddListener(()=>ShowHelp(help_screen));
        // GameObject HelpPanel = mainCamera.transform.Find("Help").gameObject;
        // mainCamera.transform.Find("HelpButton").gameObject.GetComponent<Button>().onClick.AddListener(()=>ShowHelp(HelpPanel));



        Debug.Log(" patientArray.Length " + data.patientArray.Length);
        for (int k = 0; k< data.patientArray.Length / patientProperties; k++)
        {
            int i = patientProperties * k;
            for(int j = 0; j < patientProperties; j++)
            {
                Debug.Log((i + j).ToString() + " " + data.patientArray[i + j].ToString());
            }
            GameObject parentObj;
            if(data.patientArray[i + 11] == 1)
            {
                parentObj = waitingRoom;
            }else if (data.patientArray[i + 11] == 2)
            {
                parentObj = Lobby;
            }else if(data.patientArray[i + 11] == 3)
            {
                parentObj = Carrier;
            }
            else
            {
                Debug.Log("Load patientArray[i + 11] " + data.patientArray[i + 11]);
                //Debug.Log(wardContent.transform.GetChild(0).gameObject.name);
                //Debug.Log(wardContent.transform.childCount);
                parentObj = wardContent.transform.Find(((int)data.patientArray[i + 11] / 10).ToString()).Find("Beds_" + ((int)data.patientArray[i + 11] % 10).ToString()).gameObject;
            }
            GameObject fullPatientCurr = Instantiate(prefabPatient, parentObj.transform);
            Patient currPatientComponent = fullPatientCurr.GetComponent<Patient>();
            parentObj.GetComponent<Slot>().addPatient(fullPatientCurr);
            currPatientComponent.age = (int)data.patientArray[i];
            currPatientComponent.gender = (int)data.patientArray[i + 1];
            currPatientComponent.fever = (int)data.patientArray[i + 2];
            currPatientComponent.cough = (int)data.patientArray[i + 3];
            currPatientComponent.tiredness = (int)data.patientArray[i + 4];
            currPatientComponent.chest_pain = (int)data.patientArray[i+ 5];
            currPatientComponent.breathing_difficulty = (int)data.patientArray[i +6];
            currPatientComponent.severity = data.patientArray[i + 7];
            currPatientComponent.prob_sev_inc = data.patientArray[i + 8];
            currPatientComponent.prob_sev_dec_base = data.patientArray[i + 9];
            currPatientComponent.room_multiplier = data.patientArray[i + 10];
            Debug.Log("curPatient.room_multiplier " + data.patientArray[i + 10]);
            currPatientComponent.infected = (int)data.patientArray[i + 12];
            currPatientComponent.mythreshold = (int)data.patientArray[i + 13];
            currPatientComponent.max_coins = (int)data.patientArray[i + 14];
            currPatientComponent.startTestTime = Time.time - (data.currentTime - (int)data.patientArray[i + 15]);
            switch ((int)data.patientArray[i + 16])
            {
                case 1:
                    currPatientComponent.kitName = "CommonKit";
                    break;
                case 2:
                    currPatientComponent.kitName = "ExpensiveKit";
                    break;
                case 3:
                    currPatientComponent.kitName = "DeluxeKit";
                    break;
            }
            currPatientComponent.totalTestTime = data.patientArray[i + 17];
            currPatientComponent.accuracy = data.patientArray[i + 18];
            currPatientComponent.priorPositive = (int)data.patientArray[i + 19];
            currPatientComponent.initTime = Time.time - (data.currentTime - (int)data.patientArray[i + 20]);
            currPatientComponent.InitializeLate();
        }
    }

    public void StartGame()
    {
        startTime = Time.time;
        lastUpdate = 1;
        alpha = 1;
        lastRoom = 304;
        roomsPerFloor = 4;
        total_time = 180;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        initializeWard();
        ShopUI.GetComponent<Store>().initializeStore();
        // timer = GameObject.FindGameObjectWithTag("MainCamera");
        timer_obj = timer.GetComponent<Text>();
        exit_score = ExitScreenScore.GetComponent<Text>();
        exit_rating = ExitScreenRating.GetComponent<Text>();
        timer_obj.text = (total_time).ToString();
        notification = NotificationBar.GetComponent<Text>();
        waitingRoom.GetComponent<Slot>().ForceStart();
        Lobby.GetComponent<Slot>().ForceStart();
        Carrier.GetComponent<Slot>().ForceStart();
       //GameObject exit_screen  = GameObject.Find("ExitScreen");
        exit_screen.SetActive(false);
        help_screen.SetActive(false);

        HelpButton.GetComponent<Button>().onClick.AddListener(()=>ShowHelp(help_screen));
        
        // t.gameObject.GetComponent<Button>().onClick.AddListener(()=>ShowHelp(help_screen));

    }

    private Vector2 ScreenToRectPoint(Vector2 point)
    {
        Vector2 temp = Camera.main.ScreenToViewportPoint(point);
        return new Vector2(temp.x * viewPortWidth, temp.y * viewPortHeight);
    }

    private void initializeWard()
    {
        roomGap = (reservedRoomSize - roomSize) / 2;
        hospitalHeight = lastRoom/100 * reservedRoomSize;
        hospitalWidth =  roomsPerFloor* reservedRoomSize;
        wardContent.GetComponent<RectTransform>().sizeDelta = new Vector2(hospitalWidth, hospitalHeight);
        for (int i = 0; i < lastRoom/100; i++)
        {
            for (int j = 0; j <lastRoom%10; j++)
            {
                GameObject obj = Instantiate(prefabRoom, wardContent.transform);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(reservedRoomSize * j + roomGap, reservedRoomSize * i + roomGap);
                int ward_num = (i+1)*100+j+1;
                obj.name = ward_num.ToString();
                // Debug.Log(obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
                obj.transform.Find("RoomNo").GetComponent<Text>().text = ward_num.ToString();
                Slot[] allSlots = obj.transform.GetComponentsInChildren<Slot>();
                foreach(Slot sl in allSlots)
                {
                    sl.ForceStart();
                }
            }
        }
        minZoom = Mathf.Max(windowHeight / hospitalHeight, windowWidth / hospitalWidth);
        maxZoom = windowWidth / roomSize;

    }

    public void addRoom()
    {
        // hospitalWidth += reservedRoomSize;
        
        lastRoom += 1;
        if(lastRoom%10==5){
            hospitalHeight += reservedRoomSize;
            lastRoom = ((lastRoom/100)+1)*100+1;
            wardContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0,reservedRoomSize);
        }
        GameObject obj = Instantiate(prefabRoom, wardContent.transform);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2( (lastRoom%100-1) * reservedRoomSize + roomGap, reservedRoomSize * (lastRoom/100-1) + roomGap);
                
        minZoom = Mathf.Max(windowHeight / hospitalHeight, windowWidth / hospitalWidth);
        //maxZoom = windowWidth / roomSize;
    }
    // public void addRoomPerFloor()
    // {
    //     hospitalWidth += reservedRoomSize;
    //     wardContent.GetComponent<RectTransform>().sizeDelta += new Vector2(reservedRoomSize, 0);
    //     for (int i = 0; i < floors; i++)
    //     {
    //         GameObject obj = Instantiate(prefabRoom, wardContent.transform);
    //         obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(roomsPerFloor * reservedRoomSize + roomGap, reservedRoomSize * i + roomGap);
    //     }
    //     roomsPerFloor += 1;
    //     minZoom = Mathf.Max(windowHeight / hospitalHeight, windowWidth / hospitalWidth);
    //     //maxZoom = windowWidth / roomSize;
    // }

    // public void addFloor()
    // {
    //     hospitalHeight += reservedRoomSize;
    //     wardContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, reservedRoomSize);
    //     for (int i = 0; i < roomsPerFloor; i++)
    //     {
    //         GameObject obj = Instantiate(prefabRoom, wardContent.transform);
    //         obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(reservedRoomSize * i + roomGap, floors * reservedRoomSize + roomGap);
    //     }
    //     floors += 1;
    //     minZoom = Mathf.Max(windowHeight / hospitalHeight, windowWidth / hospitalWidth);

    // }

    void Start()
    {
        
        if (resumeGame)
        {
            LoadGame(resumeFile);
        }
        else
        {
            StartGame();
        }
        /*startTime = Time.time;
        lastUpdate = 1;
        alpha = 1;
        roomsPerFloor = 4;
        floors = 3;
        total_time = 180;
        
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        initializeWard();
        ShopUI.GetComponent<Store>().initializeStore();
        // timer = GameObject.FindGameObjectWithTag("MainCamera");
        timer_obj = timer.GetComponent<Text>();
        notification = NotificationBar.GetComponent<Text>();
        Debug.Log(notification.text);
        timer_obj.text = (total_time).ToString();*/
    }

    public void save()
    {
        SaveGame("save1");
        Application.Quit();
    }
    public IEnumerator notif(string message){
        yield return new WaitForSeconds(1f);

        //Debug.Log("debug Notif");
        notification.text = message + " !";
        yield return new WaitForSeconds(1f);
        notification.text = " ";
        yield return new WaitForSeconds(1f);
        notification.text = message + " !";
        yield return new WaitForSeconds(1f);
        notification.text = " ";
    }

    private void FixedUpdate()
    {
        if ((int)(Time.time - startTime) % spawnInterval == 0 && !spawned)
        {
            spawned = true;
            Slot waitingRoomSlot = waitingRoom.GetComponent<Slot>();
            if (waitingRoomSlot.numPatients < waitingRoomSlot.maxNumPatients)
            {
                GameObject obj = Instantiate(prefabPatient, waitingRoom.transform);
                obj.GetComponent<Patient>().Initialize(level);
                if (!waitingRoomSlot.addPatient(obj))
                {
                    Debug.Log("HI");
                }
            }
            else
            {
                StartCoroutine(mainCamera.GetComponent<Attributes>().UpdateScore(-waitingRoom_penalty, 0));
                //Debug.Log("NO");
            }
        }
        else if ((int)(Time.time - startTime) % spawnInterval > 1 && spawned)
        {
            spawned = false;
        }

        float time_inst = Time.time;
        float time_since_start = alpha * (time_inst - startTime);
        if (time_since_start >= lastUpdate)
        {
            timer_obj.text = (int.Parse(timer_obj.text) - 1).ToString();

            lastUpdate = lastUpdate + 1;
        }
        // Debug.Log(time_since_start);
        int time_elapsed = (int)(time_since_start);

        if (time_elapsed >= total_time)
        {
            exit_rating.text = "Rating:  " + ((float)GetComponent<Attributes>().GetRating()).ToString();
            exit_score.text = "Score:  " + GetComponent<Attributes>().GetScore().ToString();
            //GameObject exit_screen  = GameObject.Find("ExitScreen");
            exit_screen.SetActive(true);
            exit_screen.transform.SetAsLastSibling();

            // Application.Quit();
        }
        //Debug.Log(time_elapsed);
        if (num_failures>0 && time_elapsed == failure_ins[num_failures-1])
        {
            int room_num = Random.Range(1, lastRoom % 10);
            int floor_num = 100 * Random.Range(1, lastRoom / 100);
            int ward_num = floor_num + room_num;
            if (ward_num <= lastRoom)
            {
                int var = Random.Range(1, 10);
                if (var < 3)
                {
                    Transform req = wardContent.transform.Find(ward_num.ToString());
                    req.Find("Text").GetComponent<Text>().text = "Room Failure";

                    string message = "Room Failure in " + ward_num.ToString();
                    Debug.Log(message);
                    StartCoroutine(notif(message));
                    for (int i = 0; i < req.childCount; i++)
                    {
                        Transform bed = req.GetChild(i);
                        if (bed.tag == "Slot")
                        {
                            Transform pat = bed.GetChild(0);
                            pat.GetComponent<Patient>().prob_sev_dec_base = 0f;
                            pat.GetComponent<Patient>().prob_sev_inc = 0.2f;
                            //TO ADD
                            Debug.Log("done right!");
                        }
                    }
                }

                else
                {
                    Transform req = wardContent.transform.Find(ward_num.ToString());
                    req.Find("Text").GetComponent<Text>().text = "Equipment Failure";
                    string message = "Equipment Failure in " + ward_num.ToString();
                    Debug.Log(message);
                    StartCoroutine(notif(message));
                    for (int i = 0; i < req.childCount; i++)
                    {
                        Transform bed = req.GetChild(i);
                        if (bed.tag == "Slot")
                        {
                            Transform pat = bed.GetChild(0);
                            pat.GetComponent<Patient>().prob_sev_dec_base = 0.05f;
                            pat.GetComponent<Patient>().prob_sev_inc = 0.15f;
                            //TO ADD
                            Debug.Log("done right!");
                        }
                    }
                }
                num_failures--;
            }
        }
    }


    public void ShowHelp(GameObject panel)
    {
        panel.transform.SetAsLastSibling();
		panel.SetActive(!panel.activeSelf);
    }

    void Update()
    {
        //Debug.Log(waitingRoom.GetComponent<Slot>().numPatients);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //SaveGame("save1");
            Application.Quit();
        }

        //change the spawner to spawn at randomized intervals
        

        if (!Store.shopOn)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Vector2 rectTouchPosition = ScreenToRectPoint(touch.position);
                if (!movingPatient && !scrolling && !badTouch && !zooming)
                {

                    Collider2D[] touchedColliders = Physics2D.OverlapPointAll(touchPosition);
                    for (int i = 0; i < touchedColliders.Length; i++)
                    {
                        if (touchedColliders[i].gameObject.CompareTag("patient"))
                        {
                            movingPatient = true;
                            Testing.SetActive(true);
                            Testing.transform.SetAsLastSibling();

                            movingPatientObject = touchedColliders[i].gameObject;
                            //movingPatientObject.transform.GetComponentInChildren<Image>().maskable = false;
                            movingPatientObject.transform.Find("PatientCharacter(Clone)").GetComponent<Image>().maskable = false;
                            movingPatientObject.transform.SetAsLastSibling();
                            initialPatientPosition = movingPatientObject.transform.position;

                            originalSlotObject = movingPatientObject.transform.parent.gameObject;
                            originalSlotObject.transform.SetAsLastSibling();
                            Transform parent1 = originalSlotObject.transform.parent;
                            if (parent1 != null)
                            {
                                parent1.SetAsLastSibling();
                                if (parent1.parent != null)
                                {
                                    Transform parent2 = parent1.parent.parent;
                                    if (parent2 != null)
                                    {
                                        parent2.SetAsLastSibling();
                                    }
                                }
                            }
                        }

                        if (touchedColliders[i].gameObject.CompareTag("wardMask"))
                        {
                            zooming = true;
                            scrolling = true;
                            initialScrollPosition = wardContent.GetComponent<RectTransform>().anchoredPosition;
                            initialTouchPosition = rectTouchPosition;
                            //
                        }
                    }
                    if (!zooming && !scrolling && !movingPatient)
                    {
                        badTouch = true;
                    }
                }
                else if (movingPatient)
                {
                    movingPatientObject.transform.position = touchPosition;
                    movingPatientObject.GetComponent<RectTransform>().anchoredPosition3D = movingPatientObject.GetComponent<RectTransform>().anchoredPosition;
                }
                else if (scrolling)
                {
                    zooming = false; // not needed
                    wardContent.GetComponent<RectTransform>().anchoredPosition = initialScrollPosition + rectTouchPosition - initialTouchPosition;
                    Vector2 rectPos = wardContent.GetComponent<RectTransform>().anchoredPosition;
                    if (rectPos.x < windowWidth - hospitalWidth * wardContent.transform.localScale.x)
                    {
                        rectPos.x = windowWidth - hospitalWidth * wardContent.transform.localScale.x;
                    }
                    if (rectPos.y < windowHeight - hospitalHeight * wardContent.transform.localScale.x)
                    {
                        rectPos.y = windowHeight - hospitalHeight * wardContent.transform.localScale.x;
                    }
                    if (rectPos.x > 0)
                    {
                        rectPos.x = 0;
                    }
                    if (rectPos.y > 0)
                    {
                        rectPos.y = 0;
                    }
                    wardContent.GetComponent<RectTransform>().anchoredPosition = rectPos;
                }
                else if (zooming)
                {
                    scrolling = true;
                    zooming = false;
                    initialScrollPosition = wardContent.GetComponent<RectTransform>().anchoredPosition;
                    initialTouchPosition = rectTouchPosition;
                }
            }
            else if (Input.touchCount == 0)
            {
                if (movingPatient)
                {
                    Collider2D[] touchedColliders = Physics2D.OverlapPointAll(movingPatientObject.transform.position);
                    bool isOverlapPossible = true;
                    for (int i = 0; i < touchedColliders.Length; i++)
                    {
                        if (touchedColliders[i].gameObject.CompareTag("wardMask"))
                        {
                            isOverlapPossible = false;
                        }
                    }


                    bool testing = false;
                    string testKit = "";
                    bool changePos = false;
                    Slot curSlot = null;
                    for (int i = 0; i < touchedColliders.Length; i++)
                    {
                        if (touchedColliders[i].gameObject.name == "CommonKit")
                        {
                            testKit = touchedColliders[i].gameObject.name;
                        }
                        else if (touchedColliders[i].gameObject.name == "ExpensiveKit")
                        {
                            testKit = touchedColliders[i].gameObject.name;
                        }
                        else if (touchedColliders[i].gameObject.name == "DeluxeKit")
                        {
                            testKit = touchedColliders[i].gameObject.name;
                        }
                        if ((isOverlapPossible && touchedColliders[i].gameObject.CompareTag("slot") && !(touchedColliders[i].gameObject.name.Substring(0,4) == "Beds")) ||
                            !isOverlapPossible && touchedColliders[i].gameObject.CompareTag("slot"))
                        {
                            curSlot = touchedColliders[i].gameObject.GetComponent<Slot>();
                            if (curSlot != null && curSlot.gameObject != originalSlotObject && curSlot.numPatients < curSlot.maxNumPatients)
                            {
                                if (curSlot.gameObject.name == "Lobby")
                                {
                                    testing = true;
                                    changePos = true;
                                }
                                else
                                {
                                    originalSlotObject.GetComponent<Slot>().removePatient(movingPatientObject);
                                    curSlot.addPatient(movingPatientObject);
                                    //movingPatientObject.transform.GetComponentInChildren<Image>().maskable = true;
                                    movingPatientObject.transform.Find("PatientCharacter(Clone)").GetComponent<Image>().maskable = true;

                                    movingPatient = false;

                                }
                            }
                            if (curSlot.gameObject == originalSlotObject && curSlot.gameObject.name == "Lobby")
                            {
                                testing = true;
                                changePos = false;
                            }
                        }
                        else if (isOverlapPossible && touchedColliders[i].gameObject.CompareTag("Discharge"))
                        {
                            originalSlotObject.GetComponent<Slot>().removePatient(movingPatientObject);
                            //call discharge on movingPatientObject
                            movingPatientObject.GetComponent<Patient>().Discharge();
                            Destroy(movingPatientObject);
                            movingPatient = false;
                        }

                    }
                    if (testing)
                    {
                        //handleTest(testKit, movingPatientObject);
                        //Debug.Log(testKit);
                        //Debug.Log(testKit);
                        int val = ShopUI.GetComponent<Store>().UseItem(testKit, movingPatientObject);
                        if (val == 0 || !changePos)
                        {
                            movingPatientObject.transform.position = initialPatientPosition;
                            movingPatientObject.GetComponent<RectTransform>().anchoredPosition3D = movingPatientObject.GetComponent<RectTransform>().anchoredPosition;
                            //movingPatientObject.transform.GetComponentInChildren<Image>().maskable = true;
                            movingPatientObject.transform.Find("PatientCharacter(Clone)").GetComponent<Image>().maskable = true;
                            movingPatient = false;
                        }
                        else
                        {
                            originalSlotObject.GetComponent<Slot>().removePatient(movingPatientObject);
                            curSlot.addPatient(movingPatientObject);
                            //movingPatientObject.transform.GetComponentInChildren<Image>().maskable = true;
                            movingPatientObject.transform.Find("PatientCharacter(Clone)").GetComponent<Image>().maskable = true;

                            movingPatient = false;
                        }

                    }
                    if (movingPatient)
                    {
                        movingPatientObject.transform.position = initialPatientPosition;
                        movingPatientObject.GetComponent<RectTransform>().anchoredPosition3D = movingPatientObject.GetComponent<RectTransform>().anchoredPosition;
                        //movingPatientObject.transform.GetComponentInChildren<Image>().maskable = true;
                        movingPatientObject.transform.Find("PatientCharacter(Clone)").GetComponent<Image>().maskable = true;
                        movingPatient = false;
                    }
                }
                scrolling = false;
                zooming = false;
                badTouch = false;
            }
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);
                Vector2 touchPosition0 = Camera.main.ScreenToWorldPoint(touch0.position);
                Vector2 touchPosition1 = Camera.main.ScreenToWorldPoint(touch1.position);
                Vector2 rectTouchPosition0 = ScreenToRectPoint(touch0.position);
                Vector2 rectTouchPosition1 = ScreenToRectPoint(touch1.position);
                Vector2 touchCenter = (rectTouchPosition0 + rectTouchPosition1) / 2;
                float touchDifference = (rectTouchPosition0 - rectTouchPosition1).magnitude;
                if (!movingPatient && !zooming && !scrolling && !badTouch)
                {
                    bool isTouch0Good = false;
                    bool isTouch1Good = false;
                    Collider2D[] touchedColliders0 = Physics2D.OverlapPointAll(touchPosition0);
                    Collider2D[] touchedColliders1 = Physics2D.OverlapPointAll(touchPosition1);
                    for (int i = 0; i < touchedColliders0.Length; i++)
                    {
                        if (touchedColliders0[i].gameObject.CompareTag("wardMask"))
                        {
                            isTouch0Good = true;
                        }
                    }
                    for (int i = 0; i < touchedColliders1.Length; i++)
                    {
                        if (touchedColliders1[i].gameObject.CompareTag("wardMask"))
                        {
                            isTouch1Good = true;
                        }
                    }
                    if (isTouch0Good && isTouch1Good)
                    {
                        zooming = true;
                        initialDoubleTouchCenter = touchCenter;
                        initialDoubleTouchDifference = touchDifference;

                        initialWardRectPosition = wardContent.GetComponent<RectTransform>().anchoredPosition;
                        initialWardTouchedPosition = new Vector2(-initialWardRectPosition.x + touchCenter.x - 20, -initialWardRectPosition.y + touchCenter.y - 470);

                        initialWardScale = wardContent.transform.localScale.x;
                    }
                    else
                    {
                        badTouch = true;
                    }
                }
                else if (scrolling)
                {
                    Collider2D[] touchedColliders1 = Physics2D.OverlapPointAll(touchPosition1);
                    bool isTouch1Good = false;
                    for (int i = 0; i < touchedColliders1.Length; i++)
                    {
                        if (touchedColliders1[i].gameObject.CompareTag("wardMask"))
                        {
                            isTouch1Good = true;
                        }
                    }
                    if (isTouch1Good)
                    {
                        zooming = true;
                        scrolling = false;
                        initialWardRectPosition = initialScrollPosition;

                        Vector2 newTouchCenter = (initialTouchPosition + rectTouchPosition1) / 2;
                        initialDoubleTouchCenter = newTouchCenter;
                        initialDoubleTouchDifference = (initialTouchPosition - rectTouchPosition1).magnitude;

                        initialWardTouchedPosition = new Vector2(-initialWardRectPosition.x + newTouchCenter.x - 20, -initialWardRectPosition.y + newTouchCenter.y - 470);

                        initialWardScale = wardContent.transform.localScale.x;
                        if (movingPatient)
                        {
                            movingPatientObject.transform.position = initialPatientPosition;
                            movingPatientObject.GetComponent<RectTransform>().anchoredPosition3D = movingPatientObject.GetComponent<RectTransform>().anchoredPosition;
                            //movingPatientObject.transform.GetComponentInChildren<Image>().maskable = true;
                            movingPatientObject.transform.Find("PatientCharacter(Clone)").GetComponent<Image>().maskable = true;

                            movingPatient = false;
                        }
                    }
                    else
                    {
                        movingPatientObject.transform.position = initialPatientPosition;
                        movingPatientObject.GetComponent<RectTransform>().anchoredPosition3D = movingPatientObject.GetComponent<RectTransform>().anchoredPosition;
                        //movingPatientObject.transform.GetComponentInChildren<Image>().maskable = true;
                        movingPatientObject.transform.Find("PatientCharacter(Clone)").GetComponent<Image>().maskable = true;

                        movingPatient = false;
                        badTouch = true;
                        zooming = false;
                        scrolling = false;
                    }
                }
                else if (zooming)
                {
                    float scale = touchDifference / initialDoubleTouchDifference;
                    wardContent.transform.localScale = Vector3.one * scale * initialWardScale;
                    if (wardContent.transform.localScale.x > maxZoom)
                    {
                        wardContent.transform.localScale = Vector3.one * maxZoom;
                    }
                    if (wardContent.transform.localScale.x < minZoom)
                    {
                        wardContent.transform.localScale = Vector3.one * minZoom;//new Vector3(minZoom, minZoom, minZoom);
                    }
                    scale = wardContent.transform.localScale.x / initialWardScale;
                    Vector2 currentPosOfInitialTouchedPos = initialWardTouchedPosition * scale;
                    //Vector2 rPos = GetComponent<RectTransform>().anchoredPosition;
                    Vector2 newRPos = initialWardRectPosition + initialWardTouchedPosition - currentPosOfInitialTouchedPos + touchCenter - initialDoubleTouchCenter;// new Vector2(initialWardRectPosition.x + currentPosOfInitialTouchedPos.x - initialWardTouchedPosition.x, initialWardRectPosition.y - currentPosOfInitialTouchedPos.y + initialWardTouchedPosition.y) + touchCenter - initialDoubleTouchCenter;
                    if (newRPos.x < windowWidth - hospitalWidth * wardContent.transform.localScale.x)
                    {
                        newRPos.x = windowWidth - hospitalWidth * wardContent.transform.localScale.x;
                    }
                    if (newRPos.y < windowHeight - hospitalHeight * wardContent.transform.localScale.x)
                    {
                        newRPos.y = windowHeight - hospitalHeight * wardContent.transform.localScale.x;
                    }
                    if (newRPos.x > 0)
                    {
                        newRPos.x = 0;
                    }
                    if (newRPos.y > 0)
                    {
                        newRPos.y = 0;
                    }
                    wardContent.GetComponent<RectTransform>().anchoredPosition = newRPos;
                }
                else if (movingPatient)
                {
                    movingPatientObject.transform.position = initialPatientPosition;
                    movingPatientObject.GetComponent<RectTransform>().anchoredPosition3D = movingPatientObject.GetComponent<RectTransform>().anchoredPosition;
                    //movingPatientObject.transform.GetComponentInChildren<Image>().maskable = true;
                    movingPatientObject.transform.Find("PatientCharacter(Clone)").GetComponent<Image>().maskable = true;

                    movingPatient = false;
                    badTouch = true;

                }
            }
            else if (Input.touchCount > 2)
            {
                if (movingPatient)
                {
                    movingPatientObject.transform.position = initialPatientPosition;
                    movingPatientObject.GetComponent<RectTransform>().anchoredPosition3D = movingPatientObject.GetComponent<RectTransform>().anchoredPosition;
                    //movingPatientObject.transform.GetComponentInChildren<Image>().maskable = true;
                    movingPatientObject.transform.Find("PatientCharacter(Clone)").GetComponent<Image>().maskable = true;

                    movingPatient = false;
                }
                badTouch = true;
                zooming = false;
                scrolling = false;
            }
        }
        else
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Vector2 rectTouchPosition = ScreenToRectPoint(touch.position);
                if (!scrolling && !badTouch && !zooming)
                {

                    Collider2D[] touchedColliders = Physics2D.OverlapPointAll(touchPosition);
                    for (int i = 0; i < touchedColliders.Length; i++)
                    {
                        if (touchedColliders[i].gameObject.CompareTag("wardMask"))
                        {
                            zooming = true;
                            scrolling = true;
                            initialScrollPosition = wardContent.GetComponent<RectTransform>().anchoredPosition;
                            initialTouchPosition = rectTouchPosition;
                            //
                        }
                    }
                    if (!zooming && !scrolling)
                    {
                        badTouch = true;
                    }
                }
                else if (scrolling)
                {
                    zooming = false; // not needed
                    wardContent.GetComponent<RectTransform>().anchoredPosition = initialScrollPosition + rectTouchPosition - initialTouchPosition;
                    Vector2 rectPos = wardContent.GetComponent<RectTransform>().anchoredPosition;
                    if (rectPos.x < windowWidth - hospitalWidth * wardContent.transform.localScale.x)
                    {
                        rectPos.x = windowWidth - hospitalWidth * wardContent.transform.localScale.x;
                    }
                    if (rectPos.y < windowHeight - hospitalHeight * wardContent.transform.localScale.x)
                    {
                        rectPos.y = windowHeight - hospitalHeight * wardContent.transform.localScale.x;
                    }
                    if (rectPos.x > 0)
                    {
                        rectPos.x = 0;
                    }
                    if (rectPos.y > 0)
                    {
                        rectPos.y = 0;
                    }
                    wardContent.GetComponent<RectTransform>().anchoredPosition = rectPos;
                }
                else if (zooming)
                {
                    scrolling = true;
                    zooming = false;
                    initialScrollPosition = wardContent.GetComponent<RectTransform>().anchoredPosition;
                    initialTouchPosition = rectTouchPosition;
                }
            }
            else if (Input.touchCount == 0)
            {
                scrolling = false;
                zooming = false;
                badTouch = false;
            }
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);
                Vector2 touchPosition0 = Camera.main.ScreenToWorldPoint(touch0.position);
                Vector2 touchPosition1 = Camera.main.ScreenToWorldPoint(touch1.position);
                Vector2 rectTouchPosition0 = ScreenToRectPoint(touch0.position);
                Vector2 rectTouchPosition1 = ScreenToRectPoint(touch1.position);
                Vector2 touchCenter = (rectTouchPosition0 + rectTouchPosition1) / 2;
                float touchDifference = (rectTouchPosition0 - rectTouchPosition1).magnitude;
                if (!movingPatient && !zooming && !scrolling && !badTouch)
                {
                    bool isTouch0Good = false;
                    bool isTouch1Good = false;
                    Collider2D[] touchedColliders0 = Physics2D.OverlapPointAll(touchPosition0);
                    Collider2D[] touchedColliders1 = Physics2D.OverlapPointAll(touchPosition1);
                    for (int i = 0; i < touchedColliders0.Length; i++)
                    {
                        if (touchedColliders0[i].gameObject.CompareTag("wardMask"))
                        {
                            isTouch0Good = true;
                        }
                    }
                    for (int i = 0; i < touchedColliders1.Length; i++)
                    {
                        if (touchedColliders1[i].gameObject.CompareTag("wardMask"))
                        {
                            isTouch1Good = true;
                        }
                    }
                    if (isTouch0Good && isTouch1Good)
                    {
                        zooming = true;
                        initialDoubleTouchCenter = touchCenter;
                        initialDoubleTouchDifference = touchDifference;

                        initialWardRectPosition = wardContent.GetComponent<RectTransform>().anchoredPosition;
                        initialWardTouchedPosition = new Vector2(-initialWardRectPosition.x + touchCenter.x - 20, -initialWardRectPosition.y + touchCenter.y - 470);

                        initialWardScale = wardContent.transform.localScale.x;
                    }
                    else
                    {
                        badTouch = true;
                    }
                }
                else if (scrolling)
                {
                    Collider2D[] touchedColliders1 = Physics2D.OverlapPointAll(touchPosition1);
                    bool isTouch1Good = false;
                    for (int i = 0; i < touchedColliders1.Length; i++)
                    {
                        if (touchedColliders1[i].gameObject.CompareTag("wardMask"))
                        {
                            isTouch1Good = true;
                        }
                    }
                    if (isTouch1Good)
                    {
                        zooming = true;
                        scrolling = false;
                        initialWardRectPosition = initialScrollPosition;

                        Vector2 newTouchCenter = (initialTouchPosition + rectTouchPosition1) / 2;
                        initialDoubleTouchCenter = newTouchCenter;
                        initialDoubleTouchDifference = (initialTouchPosition - rectTouchPosition1).magnitude;

                        initialWardTouchedPosition = new Vector2(-initialWardRectPosition.x + newTouchCenter.x - 20, -initialWardRectPosition.y + newTouchCenter.y - 470);

                        initialWardScale = wardContent.transform.localScale.x;
                        
                    }
                    else
                    {
                        badTouch = true;
                        zooming = false;
                        scrolling = false;
                    }
                }
                else if (zooming)
                {
                    float scale = touchDifference / initialDoubleTouchDifference;
                    wardContent.transform.localScale = Vector3.one * scale * initialWardScale;
                    if (wardContent.transform.localScale.x > maxZoom)
                    {
                        wardContent.transform.localScale = Vector3.one * maxZoom;
                    }
                    if (wardContent.transform.localScale.x < minZoom)
                    {
                        wardContent.transform.localScale = Vector3.one * minZoom;//new Vector3(minZoom, minZoom, minZoom);
                    }
                    scale = wardContent.transform.localScale.x / initialWardScale;
                    Vector2 currentPosOfInitialTouchedPos = initialWardTouchedPosition * scale;
                    //Vector2 rPos = GetComponent<RectTransform>().anchoredPosition;
                    Vector2 newRPos = initialWardRectPosition + initialWardTouchedPosition - currentPosOfInitialTouchedPos + touchCenter - initialDoubleTouchCenter;// new Vector2(initialWardRectPosition.x + currentPosOfInitialTouchedPos.x - initialWardTouchedPosition.x, initialWardRectPosition.y - currentPosOfInitialTouchedPos.y + initialWardTouchedPosition.y) + touchCenter - initialDoubleTouchCenter;
                    if (newRPos.x < windowWidth - hospitalWidth * wardContent.transform.localScale.x)
                    {
                        newRPos.x = windowWidth - hospitalWidth * wardContent.transform.localScale.x;
                    }
                    if (newRPos.y < windowHeight - hospitalHeight * wardContent.transform.localScale.x)
                    {
                        newRPos.y = windowHeight - hospitalHeight * wardContent.transform.localScale.x;
                    }
                    if (newRPos.x > 0)
                    {
                        newRPos.x = 0;
                    }
                    if (newRPos.y > 0)
                    {
                        newRPos.y = 0;
                    }
                    wardContent.GetComponent<RectTransform>().anchoredPosition = newRPos;
                }
            }
            else if (Input.touchCount > 2)
            {
                badTouch = true;
                zooming = false;
                scrolling = false;
            }

            //item logic
            if(activeCard != null)
            {
                activeCard.SetActive(false);
                activeCard.GetComponent<Image>().maskable = false;
            }

            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchWorldPosition = Camera.main.ScreenToWorldPoint(touch.position);
                if (touch.phase == TouchPhase.Began)
                {
                    Collider2D[] itemColliders = Physics2D.OverlapPointAll(touchWorldPosition);
                    foreach(Collider2D itemCollider in itemColliders)
                    {
                        if (itemCollider.gameObject.CompareTag("item"))
                        {
                            itemObj = itemCollider.gameObject;
                            fullItemObj = itemObj.transform.parent.gameObject;
                            fullItemObj.transform.SetAsLastSibling();
                            itemInitialWorldPosition = itemObj.transform.position;
                            applying = true;
                        }
                    }
                }
                if (touch.phase == TouchPhase.Stationary && applying)
                {
                    Collider2D[] Colliders = Physics2D.OverlapPointAll(touchWorldPosition);
                    bool doThis = false;
                    bool actuallyDoThis = false;
                    foreach (Collider2D collider in Colliders)
                    {
                        if (collider.gameObject.CompareTag("wardMask"))
                        {
                            doThis = true;
                        }
                        if (collider.gameObject.CompareTag("patient"))
                        {
                            actuallyDoThis = true;
                            activeCard = collider.gameObject.GetComponentsInChildren<dummy>(true)[0].gameObject;
                        }

                    }
                    if (doThis && actuallyDoThis && fullItemObj.GetComponent<itemScript>().toBeAppliedOn == itemScript.ApplyOn.Patient)
                    {
                        activeCard.SetActive(true);
                        //activeCard.transform.parent.SetAsLastSibling();//fullPatient
                        activeCard.transform.parent.parent.SetAsLastSibling();//beds
                        activeCard.transform.parent.parent.parent.SetAsLastSibling();//room
                        activeCard.GetComponent<Image>().maskable = true;
                    }
                }
                else if (touch.phase == TouchPhase.Stationary)
                {
                    Collider2D[] Colliders = Physics2D.OverlapPointAll(touchWorldPosition);
                    bool doThis = false;
                    bool actuallyDoThis = false;
                    foreach (Collider2D collider in Colliders)
                    {
                        if (collider.gameObject.CompareTag("wardMask"))
                        {
                            doThis = true;
                        }
                        if (collider.gameObject.CompareTag("patient"))
                        {
                            actuallyDoThis = true;
                            activeCard = collider.gameObject.GetComponentsInChildren<dummy>(true)[0].gameObject;
                        }

                    }
                    if (doThis && actuallyDoThis)
                    {
                        activeCard.SetActive(true);
                        //activeCard.transform.parent.SetAsLastSibling();//fullPatient
                        activeCard.transform.parent.parent.SetAsLastSibling();//beds
                        activeCard.transform.parent.parent.parent.SetAsLastSibling();//room
                        activeCard.GetComponent<Image>().maskable = true;
                    }
                }
                
                if(touch.phase == TouchPhase.Moved && applying)
                {
                    Collider2D[] Colliders = Physics2D.OverlapPointAll(touchWorldPosition);
                    bool doThis= false;
                    bool actuallyDoThis = false;
                    foreach (Collider2D collider in Colliders)
                    {
                        if (collider.gameObject.CompareTag("wardMask"))
                        {
                            doThis = true;
                        }
                        if (collider.gameObject.CompareTag("patient"))
                        {
                            actuallyDoThis = true;
                            activeCard = collider.gameObject.GetComponentsInChildren<dummy>(true)[0].gameObject;
                        }
                    
                    }
                    if (doThis && actuallyDoThis && fullItemObj.GetComponent<itemScript>().toBeAppliedOn == itemScript.ApplyOn.Patient)
                    {
                        activeCard.SetActive(true);
                        //activeCard.transform.parent.SetAsLastSibling();//fullPatient
                        activeCard.transform.parent.parent.SetAsLastSibling();//beds
                        activeCard.transform.parent.parent.parent.SetAsLastSibling();//room
                        activeCard.GetComponent<Image>().maskable = true;
                    }
                    itemObj.transform.position = touchWorldPosition;
                    itemObj.GetComponent<RectTransform>().anchoredPosition3D = itemObj.GetComponent<RectTransform>().anchoredPosition;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Collider2D[] Colliders = Physics2D.OverlapPointAll(touchWorldPosition);
                    bool doThis = false;
                    bool actuallyDoThis = false;
                    foreach (Collider2D collider in Colliders)
                    {
                        if (collider.gameObject.CompareTag("wardMask"))
                        {
                            doThis = true;
                        }
                        if (collider.gameObject.CompareTag("patient"))
                        {
                            actuallyDoThis = true;
                            activeCard = collider.gameObject.GetComponentsInChildren<dummy>(true)[0].gameObject;
                        }

                    }
                    if (doThis && actuallyDoThis)
                    {
                        activeCard.SetActive(true);
                        //activeCard.transform.parent.SetAsLastSibling();//fullPatient
                        activeCard.transform.parent.parent.SetAsLastSibling();//beds
                        activeCard.transform.parent.parent.parent.SetAsLastSibling();//room
                        activeCard.GetComponent<Image>().maskable = true;
                    }
                }
                if (touch.phase == TouchPhase.Ended && applying)
                {
                    Collider2D[] patientColliders = Physics2D.OverlapPointAll(touchWorldPosition);


                    bool apply = false;
                    bool patientApply = false;
                    GameObject patientObj = null;
                    bool wardApply = false;
                    GameObject wardObj = null;
                    foreach (Collider2D collider in patientColliders)
                    {
                        if (collider.gameObject.CompareTag("wardMask"))
                        {
                            apply = true;
                        }
                        if (collider.gameObject.CompareTag("patient"))
                        {
                            patientObj = collider.gameObject;
                            patientApply = true;
                        }
                        if (collider.gameObject.name.Substring(0,4) == "Beds")
                        {
                            wardObj = collider.gameObject.transform.parent.gameObject;
                            wardApply = true;
                            
                        }
                    }
                    if (apply)
                    {
                        if(fullItemObj.GetComponent<itemScript>().toBeAppliedOn == itemScript.ApplyOn.Ward && wardApply)
                        {
                            ShopUI.GetComponent<Store>().UseItem(Store.itemNameFromGameItem[itemObj.transform.Find("nameText").GetComponent<Text>().text], wardObj);//wardObj to be used as input too
                            Debug.Log("applied item on Ward");
                        }
                        if(fullItemObj.GetComponent<itemScript>().toBeAppliedOn == itemScript.ApplyOn.Patient && patientApply)
                        {
                            ShopUI.GetComponent<Store>().UseItem(Store.itemNameFromGameItem[itemObj.transform.Find("nameText").GetComponent<Text>().text], patientObj);//patientObj to be used as input too
                            Debug.Log("applied item on Patient");

                        }
                    }

                    itemObj.transform.position = itemInitialWorldPosition;
                    itemObj.GetComponent<RectTransform>().anchoredPosition3D = itemObj.GetComponent<RectTransform>().anchoredPosition;
                    applying = false;
                }
            }

        }
        if (!movingPatient)
        {
            Testing.SetActive(false);
        }
        
    }
}

