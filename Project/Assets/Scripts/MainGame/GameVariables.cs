using System;
using System.Collections.Generic;

public enum GameMode {
    BattleRoyal,
    Conquerors,
    CaptureTheFlag,
    Survival
};
public enum Difficulty {
    Easy,
    Medium,
    Hard
};

//@TODO A BIG REFACTOR HERE 

public class GameVariables {

    private string title;
    private string host;
    private GameMode mode;
    private Dictionary<GameMode, string> availableModes = new Dictionary<GameMode, string>(){
        {GameMode.BattleRoyal, Enum.GetName(typeof(GameMode), GameMode.BattleRoyal)},
        {GameMode.Conquerors, Enum.GetName(typeof(GameMode), GameMode.Conquerors)},
        {GameMode.CaptureTheFlag, Enum.GetName(typeof(GameMode), GameMode.CaptureTheFlag)}
    };
    
    private Difficulty difficulty;
    private Dictionary<Difficulty,string> availableDifficulties = new Dictionary<Difficulty, string>(){
        {Difficulty.Easy, Enum.GetName(typeof(Difficulty), Difficulty.Easy)},
        {Difficulty.Medium, Enum.GetName(typeof(Difficulty), Difficulty.Medium)},
        {Difficulty.Hard, Enum.GetName(typeof(Difficulty), Difficulty.Hard)}
    };

    private int maxPlayers;
    private Dictionary<int, string> availableMaxPlayers = new Dictionary<int, string>(){
        {2, "2"}, {3, "3"}, {4, "4"}, {5, "5"}, {6, "6"},
        {7, "7"}, {8, "8"}, {9, "9"}, {10, "10"}
    };

    private int maxKills;
    private Dictionary<int, string> availableMaxKills = new Dictionary<int, string>(){
        {10, "10"}, {25, "25"}, {50, "50"}, {75, "75"}, {100, "100"},
        {200, "200"}, {500, "500"}, {750, "750"}, {1000,"1000"}
	};

    private Dictionary<string, Skill> usedSkills = new Dictionary<string, Skill>(){
        {"skill 1", new Skill()},
        {"skill 2", new Skill()}, 
        {"skill 3", new Skill()},
        {"skill 4", new Skill()}
    };
    private Dictionary<string, Skill> bannedSkills = new Dictionary<string, Skill>();

    private double timer;
    private Dictionary<double, string> availableTimers = new Dictionary<double, string>(){
        {-1, "none"}, {600, "10'"}, {900, "15'"}, {1200, "20'"}, {1800, "30'"},
        {3600, "60'"}, {5400, "90'"}, {7200, "120'"}, {10800, "180'"}
    };

    private static GameVariables instance = new GameVariables();
    private GameVariables() {
        title = "";
        host = "";
        mode = GameMode.BattleRoyal;
        difficulty = Difficulty.Medium;
        maxPlayers = 10;
        maxKills = 50;
        timer = -1;
    }

    public static GameVariables Instance {
        get { return instance; }
    }

    public void SetTitle(string t) {
        string trimStr = t.TrimStart();
        if(trimStr.Length > 0 && trimStr.Length < 128)
            title = t;
    }
    public string GetTitle() { return title; }

    public void SetHost(string h) { host = h; }
    public string GetHost() { return host; }

    public void SetMode(GameMode m) { mode = m; }
    public GameMode GetMode() { return mode; }
    public Dictionary<GameMode, string> GetAvailableModes() { return availableModes; }

    public void SetMaxKills(int mk) { maxKills = mk; }
    public int GetMaxKills() { return maxKills; }
    public Dictionary<int, string> GetAvailableMaxKills() { return availableMaxKills; }
        
    public void SetMaxPlayers(int mp) { maxPlayers = mp; }
    public int GetMaxPlayers() { return maxPlayers; }
    public Dictionary<int, string> GetAvailableMaxPlayers() { return availableMaxPlayers; }

    public void SetDifficulty(Difficulty dif) { difficulty = dif; }
    public Difficulty GetDifficulty() { return difficulty; }
    public Dictionary<Difficulty, string> GetAvailableDifficulties() { return availableDifficulties; }

    public void SetTimer(double t) { timer = t; }
    public double GetTimer() { return timer; }
    public Dictionary<double, string> GetAvailableTimers() { return availableTimers; }

    public void BanSkill(string key) {
        Utilities.Instance.Assert(usedSkills.ContainsKey(key) && !bannedSkills.ContainsKey(key),
                                                            "Error pre, class:GameVariables, method: BanSkill");
        bannedSkills.Add(key, usedSkills[key]);
        usedSkills.Remove(key);

        Utilities.Instance.Assert(!usedSkills.ContainsKey(key) && bannedSkills.ContainsKey(key),
                                                            "Error post, class:GameVariables, method: BanSkill");
    }
    public void UnbanSkill(string key) {
        Utilities.Instance.Assert(!usedSkills.ContainsKey(key) && bannedSkills.ContainsKey(key),
                                                            "Error pre, class:GameVariables, method: UnbanSkill");
        usedSkills.Add(key, bannedSkills[key]);
        bannedSkills.Remove(key);

        Utilities.Instance.Assert(usedSkills.ContainsKey(key) && !bannedSkills.ContainsKey(key),
                                                            "Error post, class:GameVariables, method: UnbanSkill");
    }
    public Dictionary<string, Skill> GetUsedSkills() { return usedSkills; }
    public Dictionary<string, Skill> GetBannedSkills() { return bannedSkills; }
}
