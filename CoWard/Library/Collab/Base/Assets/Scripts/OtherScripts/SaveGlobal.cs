using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SaveGlobal
{
    public static int max_levels = 15;
    public int farthest_level =1;
    public float[] rating=new float[max_levels];
    public float[] threshold = new float[max_levels];

    
    public SaveGlobal(int level, float curr_rating){
        
        threshold[0] = 3f; threshold[1] = 3f; threshold[2] = 3f; 
        threshold[3] = 3f; threshold[4] = 3f;
        SaveGlobal data = SaveSystem<SaveGlobal>.LoadPlayer("global");
        rating[level-1] = Mathf.Max(data.rating[level-1],curr_rating);
        if(curr_rating>=threshold[level-1]){
            farthest_level = Mathf.Max(data.farthest_level,level);
        }
        
    }
}
