using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Patient : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject prefabPatientCard;
    [SerializeField] GameObject prefabPatientCharacter;
    
    GameObject patientCard;
    GameObject patientCharacter;
    GameObject ward;


    bool previouslyMoving = false;
    public int indexInSlot;
    public int gender = 0;  //1     2
                            //M     F
    public int age = 0;     //1         2       3       4       5
                            //child     teen    young   middle  old
    public float severity = 0, Initseverity; //total number of points to be allocated to symptoms, to be set randomly.
    //symptoms      0       1       2
    //              NA      mild    high
    public int fever = 0;
    public int cough = 0;
    public int tiredness = 0;
    public int chest_pain = 0;
    public int breathing_difficulty = 0;
    public int proximity_to_covid_patient = 0;
    public int infected = 0, infected_init=0;//0 is false//to be set randomly depending on severity.
    
    public int wealth = 0;//generate this many coins per tick for this patient, if placed in bed.
    public int anger = 0;//tells how much penalty(coin/rating) to deduct, if discharged when patient is positive

    public float infect_threshold = 2.2f, threshold = 5f;

    public float initTime;
    float timeFactorFlatness = 0.1f;//TO BE MODIFIED

    public int max_coins = 0;
    public int mythreshold;
    public bool is_dead=false;
    public int max_stars = 5;

    public float prob_sev_inc, prob_sev_dec_base;
    public float init_prob_inc, init_prob_dec;
    public float room_multiplier = 0.01f;
    public int[] test_time = new int[3];
    public float[] acc = new float[3];
    public int high_severity_threshold = 8;
    public int high_severity_time_max = 30;
    public int high_severity_time;
    public int clock;
    public float final_score;
    public bool score_done = false;

    Text ageText;
    Text feverText;
    Text coughText;
    Text tirednessText;
    Text chest_painText;
    Text breathing_difficultyText;
    Text proximity_to_covid_patientText;
    Text max_coinsText;

    public float startTestTime;
    public string kitName;
    public float totalTestTime, accuracy;
    public int priorPositive;



    void UpdateAge()
    {
        if (age == 5)
        {
            ageText.text = "Elderly";
        }
        else if (age == 4)
        {
            ageText.text = "Middle Age";
        }
        else if (age == 3)
        {
            ageText.text = "Young";
        }
        else if (age == 2)
        {
            ageText.text = "teen";
        }
        else if (age == 1)
        {
            ageText.text = "child";
        }

        if (gender == 1)
        {
            ageText.text += " (M)";
        }
        else if (gender == 2)
        {
            ageText.text += " (F)";
        }
    }

    void UpdateFeverSymptoms()
    {
        if (fever == 2)
        {
            feverText.text = "<color=red>High</color> Fever";
        }
        else if (fever == 1)
        {
            feverText.text = "<color=#968D1C>Mild</color> Fever";
        }
        else if (fever == 0)
        {
            feverText.text = "<color=green>No</color> Fever";
        }
    }

    void UpdateCoughSymptoms()
    {
        {
            if (cough == 2)
            {
                coughText.text = "<color=red>High</color> Cough";
            }
            else if (cough == 1)
            {
                coughText.text = "<color=#968D1C>Mild</color> Cough";
            }
            else if (cough == 0)
            {
                coughText.text = "<color=green>No</color> Cough";
            }
        }
    }

    void UpdateTirednessSymptoms()
    {
        {
            if (tiredness == 2)
            {
                tirednessText.text = "<color=red>High</color> Tiredness";
            }
            else if (tiredness == 1)
            {
                tirednessText.text = "<color=#968D1C>Mild</color> Tiredness";
            }
            else if (tiredness == 0)
            {
                tirednessText.text = "<color=green>No</color> Tiredness";
            }
        }
    }

    void UpdateChestPainSymptoms()
    {
        {
            if (chest_pain == 2)
            {
                chest_painText.text = "<color=red>High</color> Chest Pain";
            }
            else if (chest_pain == 1)
            {
                chest_painText.text = "<color=#968D1C>Mild</color> Chest Pain";
            }
            else if (chest_pain == 0)
            {
                chest_painText.text = "<color=green>No</color> Chest Pain";
            }
        }
    }

    void UpdateBreathingDifficultySymptoms()
    {
        {
            if (breathing_difficulty == 2)
            {
                breathing_difficultyText.text = "<color=red>High</color> Breathing Difficulty";
            }
            else if (breathing_difficulty == 1)
            {
                breathing_difficultyText.text = "<color=#968D1C>Mild</color> Breathing Difficulty";
            }
            else if (breathing_difficulty == 0)
            {
                breathing_difficultyText.text = "<color=green>No</color> Breathing Difficulty";
            }
        }
    }

    void UpdateProximityToCovidPatients()
    {
        {
            if (proximity_to_covid_patient == 2)
            {
                proximity_to_covid_patientText.text = "<color=red>High</color> Proximity To Covid Patients";
            }
            else if (proximity_to_covid_patient == 1)
            {
                proximity_to_covid_patientText.text = "<color=#968D1C>Mild</color> Proximity To Covid Patients";
            }
            else if (proximity_to_covid_patient == 0)
            {
                proximity_to_covid_patientText.text = "<color=green>No</color> Proximity To Covid Patients";
            }
        }
    }
    //Rajdeep's portion: start

    private void SetPositivity()
    {
        //Needs to converted to a random process
        double p = 0.7;
        
        System.Random rnd = new System.Random();
        double rv = rnd.NextDouble();
        if(rv<p)
        {
            make_positive();
        }
        else
        {
            make_negative();   
        }

    }

    private void SetSeverity()
    {
        float sigma = 3;

        // Sasta gaussian
        float v1,v2,s;
        do
        {
            v1 = 2.0f * Random.Range(0f,1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f,1f) - 1.0f;
            s = v1*v1+v2*v2;
        }
        while( s>= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f*Mathf.Log(s))/s);
        float g1 = v1*s*sigma + 7.5f;

        do
        {
            v1 = 2.0f * Random.Range(0f,1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f,1f) - 1.0f;
            s = v1*v1+v2*v2;
        }
        while( s>= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f*Mathf.Log(s))/s);
        float g2 = v1*s*sigma + 2.5f;

        //

        if(infected==1)
            severity = g1;
        else
            severity = g2;

        severity = Mathf.Clamp(severity, 1, 10);
    }
    
    int test_positivity()
    {
        System.Random rnd = new System.Random();
        double rv = rnd.NextDouble();
        if(rv<accuracy)
        {
            return priorPositive;
        }
        else
        {
            return (1-priorPositive);
        }
    }
    //Rajdeep's portion: end
    private void InitializeSymptoms(float severity)
    {
        int[] alpha = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        for(int i = (int)severity; i < 10; i++)
        {
            alpha[i] = 20 + i;
        }
        for (int i = 0; i < 10; i++)
        {
            int temp = alpha[i];
            int randomIndex = Random.Range(i, 10);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }

        if(alpha[0] < 15 && alpha[1] < 15)
        {
            fever = 2;
        }
        else if(alpha[0] > 15 && alpha[1] > 15)
        {
            fever = 0;
        }
        else
        {
            fever = 1;
        }

        if (alpha[2] < 15 && alpha[3] < 15)
        {
            cough = 2;
        }
        else if (alpha[2] > 15 && alpha[3] > 15)
        {
            cough = 0;
        }
        else
        {
            cough = 1;
        }

        if (alpha[4] < 15 && alpha[5] < 15)
        {
            tiredness = 2;
        }
        else if (alpha[4] > 15 && alpha[5] > 15)
        {
            tiredness = 0;
        }
        else
        {
            tiredness = 1;
        }

        if (alpha[6] < 15 && alpha[7] < 15)
        {
            chest_pain = 2;
        }
        else if (alpha[6] > 15 && alpha[7] > 15)
        {
            chest_pain = 0;
        }
        else
        {
            chest_pain = 1;
        }

        if (alpha[8] < 15 && alpha[9] < 15)
        {
            breathing_difficulty = 2;
        }
        else if (alpha[8] > 15 && alpha[9] > 15)
        {
            breathing_difficulty = 0;
        }
        else
        {
            breathing_difficulty = 1;
        }

    }

    public void InitializeLate() 
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        patientCard = Instantiate(prefabPatientCard, transform);
        patientCharacter = Instantiate(prefabPatientCharacter, transform);

        ageText = patientCard.transform.Find("Age(Gender)").GetComponent<Text>();
        feverText = patientCard.transform.Find("Fever").GetComponent<Text>();
        coughText = patientCard.transform.Find("Cough").GetComponent<Text>();
        chest_painText = patientCard.transform.Find("ChestPain").GetComponent<Text>();
        tirednessText = patientCard.transform.Find("Tiredness").GetComponent<Text>();
        breathing_difficultyText = patientCard.transform.Find("BreathingDifficulty").GetComponent<Text>();
        max_coinsText = patientCard.transform.Find("MaxCoins").GetComponent<Text>();
        max_coinsText.text = "Max coins: "+max_coins.ToString();
        UpdateAge();
        UpdateFeverSymptoms();
        UpdateCoughSymptoms();
        UpdateTirednessSymptoms();
        UpdateChestPainSymptoms();
        UpdateBreathingDifficultySymptoms();
    }

    public void Initialize(int level)//called when patient consults doc
    {
        //col = GetComponent<BoxCollider2D>();
        // ward = GameObject.FindGameObjectWithTag("Ward");
        test_time[0] = 5; test_time[1] = 7; test_time[2] = 8;
        acc[0] = 0.7f; acc[1] = 0.9f; acc[2] = 1f;

        mythreshold = mythreshold = Random.Range(0,4);
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //patientCard = Instantiate(prefabPatientCard, transform);
        //patientCharacter = Instantiate(prefabPatientCharacter, transform);
        // prefabPatientCard.onClick.AddListener(ShowCard_onClick);
        age = Random.Range(1, 6);
        gender = Random.Range(1, 3);

        //time_to_cure = 0;
        //depending on age initialize severity
        
        SetPositivity();
        SetSeverity();
        high_severity_time = 0;
        switch(level){
            
            case 1: 
            prob_sev_inc = 0.07f;
            prob_sev_dec_base = 0.1f;
            max_coins = 100+Random.Range(0,11)*10;
            break;
            case 2:
            prob_sev_inc = 0.07f;
            prob_sev_dec_base = 0.1f;
            max_coins = 200+Random.Range(0,11)*10;
            break;
            case 3:
            prob_sev_inc = 0.07f;
            prob_sev_dec_base = 0.1f;
            max_coins = 300+Random.Range(0,11)*10;
            break;
            case 4:
            prob_sev_inc = 0.07f;
            prob_sev_dec_base = 0.1f;
            max_coins = 400+Random.Range(0,11)*10;
            break;
            case 5:
            prob_sev_inc = 0.07f;
            prob_sev_dec_base = 0.1f;
            max_coins = 500+Random.Range(0,11)*10;
            break;

        }
        init_prob_dec = prob_sev_dec_base;
        init_prob_inc = prob_sev_inc;
        clock = 0;
        /*
        if(age == 5)
        {
            infected = Random.Range(0, 2);
            severity = infected * Random.Range(5, 11) + (1 - infected) * Random.Range(1, 6);
        }
        if(age == 4 || age == 1)
        {
            infected = Random.Range(0, 2);
            severity = infected * Random.Range(2, 9) + (1 - infected) * Random.Range(1, 6);
        }
        if(age == 3 || age == 2)
        {
            infected = Random.Range(0, 2);
            severity = infected * Random.Range(1, 7) + (1 - infected) * Random.Range(1, 6);
        }
        */

        //depending on severity initialize symptoms
        InitializeSymptoms(severity);
        Initseverity = severity;
        infected_init = infected;
        //Debug.Log(severity + "     " + (fever + cough + tiredness + chest_pain + breathing_difficulty).ToString());
        // randomly initialize proximity_to_covid_patient. This is rarely severe.
        int temp_prox = infected * Random.Range(0, 21) + (1-infected) * Random.Range(0,20);
        if(temp_prox == 20)
        {
            proximity_to_covid_patient = 2;
        }
        else if(temp_prox > 9)
        {
            proximity_to_covid_patient = 1;
        }
        else if(temp_prox > 0)
        {
            proximity_to_covid_patient = 0;
        }
        //severity, age, infected, symptoms, proximity has been initialized

        //initialize initial position (to be done)
        //initialPos = transform.position;
        initTime = Time.time;

        InitializeLate();
       
    }
   

    public void startTesting(string kitName)
    {
        startTestTime = Time.time;
        priorPositive = infected;
        patientCard.transform.Find("kitName").GetComponent<Text>().text = kitName;
        patientCard.transform.Find("positivity").GetComponent<Text>().text = ": Testing..";
        switch (kitName)
        {
            case "CommonKit":
                totalTestTime = test_time[0];
                accuracy = acc[0];
                break;
            case "ExpensiveKit":
                totalTestTime = test_time[1];
                accuracy = acc[1];
                break;
            case "DeluxeKit":
                totalTestTime = test_time[2];
                accuracy = acc[2];
                break;
        }
    }

    private void FixedUpdate()
    {
        if(!is_dead)
            UpdateSeverity();
        if(is_dead)
        {
            patientCharacter.transform.Find("dead").gameObject.SetActive(true);
        }
    }

    private void Update()
    {
/*        if (startTesting)
        {
            startTestTime = Time.time;
            startTesting = false;
            priorPositive = infected;
            switch (kitName)
            {
                case "CommonKit":
                    totalTestTime = 10;
                    accuracy = 0.7f;
                    break;
                case "ExpensiveKit":
                    totalTestTime = 15;
                    accuracy = 0.9f;
                    break;
                case "DeluxeKit":
                    totalTestTime = 20;
                    accuracy = 1f;
                    break;
            }
        }*/
        if(startTestTime != 0 && Time.time - startTestTime > totalTestTime)
        {
            startTestTime = 0;
            if (test_positivity() == 1)
            {
                patientCard.transform.Find("positivity").GetComponent<Text>().text = "<color=red>Positive</color>";

            }
            else
            {
                patientCard.transform.Find("positivity").GetComponent<Text>().text = "<color=green>Negative</color>";
            }
        }

        if (mainCamera.GetComponent<MainScript>().movingPatient && gameObject == mainCamera.GetComponent<MainScript>().movingPatientObject)
        {
            if (!previouslyMoving)
            {
                previouslyMoving = true;
                patientCard.SetActive(true);
                //patientCharacter.GetComponent<Image>().sprite = Resources.Load<Sprite>("character/" + age.ToString() + "/" + gender.ToString() + "/1.png");
            }
            //GetComponent<Image>().sprite = 
        }
        else if(!Store.shopOn)
        {
            if (previouslyMoving)
            {
                previouslyMoving = false;
                patientCard.SetActive(false);
                /*if(transform.parent.gameObject.name == "Lobby")
                {
                    Debug.Log("YO");
                    Debug.Log("characterSitting/" + age.ToString() + "/" + gender.ToString() + "/1.png");
                    patientCharacter.GetComponent<Image>().sprite = Resources.Load<Sprite>("characterSitting/" + age.ToString() + "/" + gender.ToString() + "/1.png");
                }*/
            }
        }

        //if()  add some time factor for update
        //UpdateSeverity();

        if (infected_init == 0 && Random.Range(0, 5) == 3 && transform.parent.name.Substring(0,4) == "Beds")
        {
            float time = mainCamera.GetComponent<MainScript>().startTime;
            float avg_severity = 0;
            if(transform.parent.childCount>1)
            {
                    for (int i = 0; i < transform.parent.childCount; i++)
                    {
                        Transform pat = transform.parent.GetChild(i);

                        if (pat != gameObject.transform && pat != null)
                        {
                            avg_severity += (pat.GetComponent<Patient>().severity - threshold);
                        }
                    }
                    avg_severity /= (transform.parent.childCount-1);
                    if (avg_severity >= infect_threshold)
                    {
                        severity = threshold + avg_severity;
                        infected_init = 1;
                        make_positive();
                    }
            }
        }
    }

/*    public void UpdateSeverity(){
        time = Time.time;
        severity = Initseverity - (int)(time - initTime);
    }*/

    public void administer_medicine()
    {

        if(prob_sev_dec_base == 0.25f)
        {
            prob_sev_inc += 0.05f;
            prob_sev_inc += Mathf.Min(0.3f, prob_sev_inc);
        }  

        prob_sev_dec_base += 0.05f;
        prob_sev_dec_base = Mathf.Min(0.25f, prob_sev_dec_base);


    }

    public float score()
    {

        if (score_done)
            return final_score;
        

        if (is_dead)
        {
            score_done = true;
            final_score = 0.5f * max_coins;
            return final_score;
        }
        float improvement_score, time_score, expected_time;

        //TWEAK CHANGE OPTIMIZE
        if (infected_init == 0 && infected == 0)
        {
            expected_time = 20.0f;
            improvement_score = 1.0f;
        }
        else
        {
            improvement_score = (Initseverity - severity) / Initseverity;
            expected_time = 0.8f / (init_prob_inc - init_prob_dec);
        }

        if (clock < expected_time)
        {
            time_score = 1;
        }
        else
        {
            // TWEAK CHANGE OPTIMIZE
            time_score = 0.6f - 0.4f * (clock - expected_time -500.0f) / 500.0f;
        }
        
        score_done = true; 
        final_score = time_score * improvement_score * max_coins;
        return final_score;
    }


    public int rating(int score)
    {
        if(score<0)
            return 0;
        
        return (int) score * max_stars / max_coins ;

    }

    public void make_positive()
    {
        prob_sev_inc = init_prob_inc;
        infected = 1;
    }
    public void make_negative()
    {
        prob_sev_inc = 0.0f;
        infected = 0;
    }

    public void Discharge()
    {
        //Debug.Log("discharge");
        //ADD CODE
        //int alpha = 10;
        // float time_factor = Mathf.Clamp01( 1 - Mathf.Exp(- timeFactorFlatness * (Time.time - initTime)));
        // float heal_percentage = (Initseverity - severity) / Initseverity;
        // int coins_to_add = (int)(max_coins / 2 * Mathf.Clamp(heal_percentage + time_factor, 0, 2));
        
        if (!is_dead)
        {
            int coins_to_add = (int) score();
            StartCoroutine(mainCamera.GetComponent<Attributes>().UpdateScore(coins_to_add, 1)); 
            mainCamera.GetComponent<Attributes>().UpdateRating(rating(coins_to_add));
        }
        else
        {
            int coins_to_add = (int) score();
            StartCoroutine(mainCamera.GetComponent<Attributes>().UpdateScore(coins_to_add, 2));
        }
    }

    public void UpdateSeverity()
    {

        clock += 1;

        float rv = Random.Range(0.0f, 1.0f);
        bool increased = false;
        if (rv < prob_sev_inc)
        {
            severity += 1;
            if(severity > 15)
            {
                //death
            }else if(severity > 12)
            {
                //warning
            }else if(severity < 11)
            {
                int i = Random.Range(0, 11- (int)severity);
                if ((i -= 2 - fever) < 0)
                {
                    fever += 1;
                    UpdateFeverSymptoms();
                }
                else if ((i -= 2 - cough) < 0)
                {
                    cough += 1;
                    UpdateCoughSymptoms();
                }
                else if ((i -= 2 - tiredness) < 0)
                {
                    tiredness += 1;
                    UpdateTirednessSymptoms();
                }
                else if ((i -= 2 - chest_pain) < 0)
                {
                    chest_pain += 1;
                    UpdateChestPainSymptoms();
                }
                else if ((i -= 2 - breathing_difficulty) < 0)
                {
                    breathing_difficulty += 1;
                    UpdateBreathingDifficultySymptoms();
                }
                else
                {
                    Debug.Log("error1");
                }
            }
            increased = true;
            //Debug.Log("increased");
        }
        
        if (severity >= 8)
        {
            high_severity_time += 1;
        }

        if(high_severity_time >= high_severity_time_max)
        {
            if(!is_dead)
                FindObjectOfType<AudioManager>().Play("death");
            is_dead = true;
            int coins_to_add = (int) score();
            StartCoroutine(mainCamera.GetComponent<Attributes>().UpdateScore(coins_to_add, 0)); 
            mainCamera.GetComponent<Attributes>().UpdateRating(rating(coins_to_add));
            

        }
        string roomTag = transform.parent.parent.gameObject.tag;
        if(roomTag.Substring(0, 5) == "level")
        {
            float prob_sev_dec = (roomTag[5] - '0')* room_multiplier + prob_sev_dec_base;
            if(!increased && rv < prob_sev_inc + prob_sev_dec)
            {
                //Debug.Log("decreased");
                
                severity -= 1;
                if(severity < 0)
                {
                    severity = 0;
                }else if(severity < 10)
                {
                    int i = Random.Range(0, (int)severity + 1);
                    if ((i -= fever) < 0)
                    {
                        fever -= 1;
                        UpdateFeverSymptoms();
                    }
                    else if ((i -= cough) < 0)
                    {
                        cough -= 1;
                        UpdateCoughSymptoms();
                    }
                    else if ((i -= tiredness) < 0)
                    {
                        tiredness -= 1;
                        UpdateTirednessSymptoms();
                    }
                    else if ((i -= chest_pain) < 0)
                    {
                        chest_pain -= 1;
                        UpdateChestPainSymptoms();
                    }
                    else if ((i -= breathing_difficulty) < 0)
                    {
                        breathing_difficulty -= 1;
                        UpdateBreathingDifficultySymptoms();
                    }
                    else 
                    { 
                        Debug.Log("error2"); 
                    }
                }
            }
            else
            {
                //Debug.Log("same");
            }
        }
        if((int)severity != fever + cough + tiredness + chest_pain + breathing_difficulty && (int)severity <= 10)
        {
            Debug.Log("error3");
            Debug.Log(severity.ToString() + " " + (fever + cough + tiredness + chest_pain + breathing_difficulty).ToString());
        }
        if (severity < mythreshold)
        {
            make_negative();
        }


    }

}
