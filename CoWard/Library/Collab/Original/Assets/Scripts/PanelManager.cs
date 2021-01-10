using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PanelManager : MonoBehaviour
{
    // Start is called before the first frame update

    private int current_panel;
    [SerializeField] GameObject[] panels;
    [SerializeField] Button PrevButton;
    [SerializeField] Button NextButton; 


    void Start()
    {

        PrevButton.onClick.AddListener(()=>prev_button());
        NextButton.onClick.AddListener(()=>next_button());

        for (int i = 0; i < panels.Length; i++)
        {
            if(i==current_panel)
                panels[i].SetActive(true);
            else
                panels[i].SetActive(false);
        }

    }

    public void prev_button()
    {
        
        if(current_panel == 3)
        {
            NextButton.transform.Find("Text").GetComponent<Text>().text = "Next";
        }

        current_panel -= 1;
        if (current_panel < 0)
            current_panel = 0;
        
    }
    public void next_button()
    {
       if(current_panel==3){
        SceneManager.LoadScene("Level");
       }
        current_panel += 1;
        if(current_panel > 3)
            current_panel = 3;
        if(current_panel == 3)
        {
            NextButton.transform.Find("Text").GetComponent<Text>().text = "Get Started!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if(i==current_panel)
                panels[i].SetActive(true);
            else
                panels[i].SetActive(false);
        }
    }
}
