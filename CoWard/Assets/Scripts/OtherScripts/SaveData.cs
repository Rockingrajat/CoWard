using System;

[System.Serializable]
public class SaveData
{
    public float[] patientArray;
    public int[] inventory;
    public int[] failure_ins;
    public int lastRoom;
    public float startTime, lastUpdate, alpha;
    public float currentTime;
    public int score;
    public float rating, spawnInterval;
    public int totalTime, level, waitingRoom_penalty;
    
    public SaveData(float[] patientArray, int[] inventory, int[] failure_ins, int lastRoom, float startTime, float lastUpdate,float alpha, float currentTime, int score, float rating, int totalTime, int level, float spawnInterval, int waitingRoom_penalty)
    {
        
        this.lastRoom = lastRoom;
        this.startTime = startTime;
        this.lastUpdate = lastUpdate;
        this.alpha = alpha;
        this.patientArray = patientArray;
        this.currentTime = currentTime;
        this.inventory = inventory;
        this.failure_ins = failure_ins;
        this.score = score;
        this.rating = rating;
        this.totalTime = totalTime;
        this.level = level;
        this.spawnInterval = spawnInterval;
        this.waitingRoom_penalty = waitingRoom_penalty;
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
