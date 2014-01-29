using UnityEngine;
using System.Collections.Generic;

public class GameOverManager: SingletonMono<GameOverManager> {

    private const int GAME_OVER_LEVEL = 4;

    private GameOverManager() { }

    private PlayerTeam winningTeam;

    void Awake () {
        enabled = false;
	}

    void OnLevelWasLoaded(int level) {
        if (level == GAME_OVER_LEVEL) {
            Vector3 winSpawnPoint = GameObject.Find("WinnersArea/SpawnPoints/SpawnPoint" + (int)GameManager.Instance.MyPlayerColor).transform.position;
            Vector3 loseSpawnPoint = GameObject.Find("LosersArea/SpawnPoints/SpawnPoint" + (int)GameManager.Instance.MyPlayerColor).transform.position;

            if (GameManager.Instance.IsMyTeam(winningTeam))
                TeleportManager.Instance.TeleportToPoint(winSpawnPoint);
            else
                TeleportManager.Instance.TeleportToPoint(loseSpawnPoint);

            GameManager.Instance.MyCharacter.transform.LookAt(Camera.main.transform);            
        }
    }

    public void SetUp(PlayerTeam _winningTeam) {
        winningTeam = _winningTeam;

        GameManager.Instance.MyCharacterModel.IsSilenced = true;
        GameManager.Instance.MyCharacterModel.IsStunned = true;        

        //AppendInfoToPlayerStatisticsFile();        
        enabled = true;
    }

    private void AppendInfoToPlayerStatisticsFile() {
        string statisticsText = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
        statisticsText += "IsWinner\t" + GameManager.Instance.IsMyTeam(winningTeam);
        statisticsText += "\nPlayers count\t" + GameManager.Instance.AllPlayerKeys.Count;
        statisticsText += "\nTeam members count\t" + GameManager.Instance.AllAlliesKeys.Count;
        statisticsText += "\nKills\t" + GameManager.Instance.MyCharacterModel.Kills;
        statisticsText += "\nDeaths\t" + GameManager.Instance.MyCharacterModel.Deaths;
        //statisticsText += "\nAssists\t" + GameManager.Instance.MyCharacterModel.Assists;        
        statisticsText += "\nFinal stat build:\n Level\t" + GameManager.Instance.MyCharacterModel.Level;
        statisticsText += "\nSTR\t" + GameManager.Instance.MyCharacterModel.GetStat((int)StatType.STR).FinalValue;
        statisticsText += "\nSTA\t" + GameManager.Instance.MyCharacterModel.GetStat((int)StatType.STA).FinalValue;
        statisticsText += "\nAGI\t" + GameManager.Instance.MyCharacterModel.GetStat((int)StatType.AGI).FinalValue;
        statisticsText += "\nINT\t" + GameManager.Instance.MyCharacterModel.GetStat((int)StatType.INT).FinalValue;
        statisticsText += "\nCHA\t" + GameManager.Instance.MyCharacterModel.GetStat((int)StatType.CHA).FinalValue;
        statisticsText += "\n" + CombatManager.Instance.SkillCastingCountersToString();
        statisticsText += "\nGame preferences\n";
        statisticsText += "\nMode:\t" + GameVariables.Instance.Mode.Key;
        if (GameVariables.Instance.Mode.Value.Equals(GameMode.BattleRoyal))
            statisticsText += "\nStarting lifes\t" + GameVariables.Instance.StartingLifes.Key;
        else
            statisticsText += "\nTarget Kills\t" + GameVariables.Instance.TargetKills.Key;
        statisticsText += "\nMap\t" + GameVariables.Instance.Map.Key;
        statisticsText += "\nDifficulty\t"+GameVariables.Instance.Difficulty.Key;
        statisticsText += "\n\n" + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"PlayerStatistics.txt", true)) {
            file.WriteLine(statisticsText);
        }        
    }

    public PlayerTeam WinningTeam {
        get { return winningTeam; }
        set { winningTeam = value; }
    }
}
