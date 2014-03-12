/// <summary>
///  This script is not attached to any GameObject but it is usd
///  by the PlayerDataBase script in building the PlayerList
/// 
/// All code is taken from Gamer To Game Developer Series 1
/// http://www.gamertogamedeveloper.com
/// No credit is taken myself
/// </summary>

public class PlayerDataClass {

	// Variables start________
	public int networkPlayer;
	public string playerName;
	public int playerScore;
	public string playerTeam;

	// Variables end__________

	public PlayerDataClass Constructor()
	{
		PlayerDataClass capture = new PlayerDataClass();
		capture.networkPlayer = networkPlayer;
		capture.playerName = playerName;
		capture.playerScore = playerScore;
		capture.playerTeam = playerTeam;

		return capture;
	}

}
