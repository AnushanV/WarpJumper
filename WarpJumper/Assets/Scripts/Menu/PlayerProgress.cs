[System.Serializable]
public class PlayerProgress{
    public int levelNum; //the level that the player is on
    public float[] lastCheckpointPos = new float[2]; //the last checkpoint position of the player

    public PlayerProgress() {
        //Default level and position for level 1
        this.levelNum = 1;
        this.lastCheckpointPos[0] = -6;
        this.lastCheckpointPos[1] = -1;
    }

    public PlayerProgress(int levelNum, float x, float y) {
        this.levelNum = levelNum;
        this.lastCheckpointPos[0] = x;
        this.lastCheckpointPos[1] = y;
    }
}
