using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Helper : MonoBehaviour
{
	[SerializeField] GameObject prefabPatient;
    static int roomsPerFloor = 4;
    static int patientProperties = 21;
	public int[] SetFailure(int num_failures, int total_time){
        int[] failure_ins = new int[num_failures];
        int offset = 15;
        for(int i=0;i<num_failures;i++){
            failure_ins[i] = Random.Range(offset,total_time-offset);
        }
        System.Array.Sort<int>(failure_ins, new System.Comparison<int>( 
                  (i1, i2) => i2.CompareTo(i1)));
        return failure_ins;
    }
    public float[] CreatePatients(int num_patients, int level, int lastRoom){
        int[] roomdetail = new int[(lastRoom/100)*roomsPerFloor*4];
        float[] patientArray = new float[num_patients * patientProperties]; //age, gender, 5 symptoms, severity, prob_sev_inc, prob_sev_dec_base, room_multiplier, position, infected
        // Debug.Log("patients.leng" + patients.Length);
        for(int k = 0; k< num_patients; k++)
        {
            int i = patientProperties * k;
            GameObject obj = Instantiate(prefabPatient, transform);
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
