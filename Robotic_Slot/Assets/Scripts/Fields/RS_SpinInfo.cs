
using System.Collections.Generic;
[System.Serializable]
public class RS_SpinInfo
{
    public int[][] slots { get; set; }
    public float win { get; set; }
    public float totalBet { get; set; }
    public int[] winFlag { get; set; }
    public float balance { get; set; }
    public int freeSpins { get; set; }
    public List<RS_Playline> playlineWithWins { get; set; }
}

public class RS_Playline
{
    public int[][] playline { get; set; }
    public float win { get; set; }
    public bool wildFlag { get; set; }
}