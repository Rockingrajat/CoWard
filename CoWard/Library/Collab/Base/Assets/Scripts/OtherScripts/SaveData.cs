using System;

[System.Serializable]
public class SaveData
{
    public float[] patientArray;
    public int floors;
    public int roomsPerFloor;
    public float startTime, lastUpdate, alpha;
    public float currentTime;
    //public int level;

    public SaveData(float[] patientArray, int floors,int roomsPerFloor, float startTime,float  lastUpdate,float alpha, float currentTime)
    {
        this.floors = floors;
        this.roomsPerFloor = roomsPerFloor;
        this.startTime = startTime;
        this.lastUpdate = lastUpdate;
        this.alpha = alpha;
        this.patientArray = patientArray;
        this.currentTime = currentTime;
        /*this.oneDLengthOfGrid = oneDLengthOfGrid;
        this.numberOfWords = numberOfWords;
        this.difficulty = difficulty;

        this.grid = new char[oneDLengthOfGrid * oneDLengthOfGrid];
        this.isRandomChar = new bool[oneDLengthOfGrid * oneDLengthOfGrid];
        this.clues = new string[numberOfWords];
        this.words = new string[numberOfWords];
        this.indexOfWords = new int[numberOfWords];
        this.startIndexArray = new int[numberOfWords];
        this.endIndexArray = new int[numberOfWords];

        for (int i = 0; i < numberOfWords; i++)
        {
            this.clues[i] = clues[i];
            this.words[i] = words[i];
            this.indexOfWords[i] = indexOfWords[i];
            this.startIndexArray[i] = startIndexArray[i];
            this.endIndexArray[i] = endIndexArray[i];
        }

        for (int i = 0; i < oneDLengthOfGrid * oneDLengthOfGrid; i++)
        {
            this.grid[i] = grid[i];
            this.isRandomChar[i] = isRandomChar[i];
        }*/
    }
}
