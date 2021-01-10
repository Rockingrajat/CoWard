using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    float height;
    float width;
    float reservedPatientSize = 120;
    float patientSize = 100;
    float patientGap;
    int xSlots;
    int ySlots;
    public int numPatients = 0;
    public int maxNumPatients;
    // Start is called before the first frame update

    public void ForceStart()
    {
        if (gameObject.name.Substring(0, 4) == "Beds")
        {
            reservedPatientSize = 200;
            patientSize = 200;

        }
        height = GetComponent<RectTransform>().sizeDelta.y;
        width = GetComponent<RectTransform>().sizeDelta.x;
        patientGap = (reservedPatientSize - patientSize) / 2;
        xSlots = Mathf.RoundToInt(width / reservedPatientSize);
        ySlots = Mathf.RoundToInt(height / reservedPatientSize);
        maxNumPatients = xSlots * ySlots;
        if(gameObject.name == "Lobby")
        {
            maxNumPatients = 5;
        }
    }
    void Start()
    {
        /*if(gameObject.name.Substring(0, 4)=="Beds"){
            reservedPatientSize = 250;
           
        }
       
        height = GetComponent<RectTransform>().sizeDelta.y;
        width = GetComponent<RectTransform>().sizeDelta.x;
        patientGap = (reservedPatientSize - patientSize) / 2;
        xSlots = Mathf.RoundToInt(width / reservedPatientSize);
        ySlots = Mathf.RoundToInt(height / reservedPatientSize);
        maxNumPatients = xSlots * ySlots;*/
    }

    public bool addPatient(GameObject patient)
    {
        if(numPatients < maxNumPatients)
        {
            //patient.tag = "patientIn" + tag;
            patient.transform.SetParent(transform);
            patient.transform.localScale = transform.localScale;
            patient.GetComponent<Patient>().indexInSlot = numPatients;
            patient.GetComponent<RectTransform>().anchoredPosition = new Vector2((numPatients % xSlots) * reservedPatientSize + patientGap, (numPatients / xSlots) * reservedPatientSize + patientGap);
            numPatients++;
            return true;
        }
        else
        {
            return false;
        }
    }

     public bool addPatientToWard(GameObject patient)
    {
        if(numPatients < maxNumPatients)
        {
            //patient.tag = "patientIn" + tag;
            patient.transform.SetParent(transform);
            patient.transform.localScale = transform.localScale;
            patient.GetComponent<Patient>().indexInSlot = numPatients;
            patient.GetComponent<RectTransform>().anchoredPosition = new Vector2((numPatients % xSlots) * reservedPatientSize + patientGap, (numPatients / xSlots) * reservedPatientSize + patientGap);
            numPatients++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool removePatient(GameObject patient)
    {
        int index = patient.GetComponent<Patient>().indexInSlot;
        if(index < numPatients)
        {
            bool oneMore = false;
            for(int j = 0; j < numPatients; j++)
            {
                GameObject currPatient = transform.GetChild(j).gameObject;
                if(currPatient.name != "Testing")
                {
                    // s(currPatient);
                    // Debug.Log(j);
                    int i = currPatient.GetComponent<Patient>().indexInSlot;
                    if(i > index)
                    {
                        currPatient.GetComponent<Patient>().indexInSlot = i - 1;
                        currPatient.GetComponent<RectTransform>().anchoredPosition = new Vector2(((i-1) % xSlots) * reservedPatientSize + patientGap, ((i-1) / xSlots) * reservedPatientSize + patientGap);
                    }
                }
                else{
                    oneMore = true;
                }
            }
            if (oneMore)
            {
                GameObject currPatient = transform.GetChild(numPatients).gameObject;
                int i = currPatient.GetComponent<Patient>().indexInSlot;
                if (i > index)
                {
                    currPatient.GetComponent<Patient>().indexInSlot = i - 1;
                    currPatient.GetComponent<RectTransform>().anchoredPosition = new Vector2(((i - 1) % xSlots) * reservedPatientSize + patientGap, ((i - 1) / xSlots) * reservedPatientSize + patientGap);
                }
            }
            numPatients--;
            return true;
        }
        else
        {
            return false;
        }
    }
}
