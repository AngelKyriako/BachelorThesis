using System;
using System.Collections.Generic;

public enum GameMode {
    BattleRoyal,
    Conquerors,
    CaptureTheFlag,
    Survival
};

public enum GameMap {
    MeetingPoint,
}

public enum GameDifficulty {
    Easy,
    Medium,
    Hard
};

public class GameVariables {

    #region attributes
    private string title;
    private KeyValuePair<string, GameMode> mode;
    private KeyValuePair<string, GameMap> map;
    private KeyValuePair<string, GameDifficulty> difficulty;
    private KeyValuePair<string, int> maxPlayers, targetKills;
    private KeyValuePair<string, float> timer;

    private Dictionary<string, GameMode> availableModes;
    private Dictionary<string, GameMap> availableMaps;
    private Dictionary<string, GameDifficulty> availableDifficulties;
    private Dictionary<string, int> availableMaxPlayers, availableTargetKills;
    private Dictionary<string, float> availableTimers;
    #endregion

    private static GameVariables instance = new GameVariables();
    public static GameVariables Instance {
        get { return instance; }
    }

    private GameVariables() {
        title = string.Empty;
        mode = new KeyValuePair<string, GameMode>(Enum.GetName(typeof(GameMode), GameMode.BattleRoyal), GameMode.BattleRoyal);        
        availableModes = new Dictionary<string, GameMode>(){ {Enum.GetName(typeof(GameMode), GameMode.BattleRoyal), GameMode.BattleRoyal},
                                                             {Enum.GetName(typeof(GameMode), GameMode.Conquerors), GameMode.Conquerors}
                                                           };
        map = new KeyValuePair<string, GameMap>(Enum.GetName(typeof(GameMap), GameMap.MeetingPoint), GameMap.MeetingPoint);
        availableMaps = new Dictionary<string, GameMap>(){ {Enum.GetName(typeof(GameMap), GameMap.MeetingPoint), GameMap.MeetingPoint}
                                                           };
        difficulty = new KeyValuePair<string, GameDifficulty>(Enum.GetName(typeof(GameDifficulty), GameDifficulty.Medium), GameDifficulty.Medium);
        availableDifficulties = new Dictionary<string, GameDifficulty>(){ {Enum.GetName(typeof(GameDifficulty), GameDifficulty.Easy), GameDifficulty.Easy},
                                                                          {Enum.GetName(typeof(GameDifficulty), GameDifficulty.Medium), GameDifficulty.Medium},
                                                                          {Enum.GetName(typeof(GameDifficulty), GameDifficulty.Hard), GameDifficulty.Hard}
                                                                        };
        maxPlayers = new KeyValuePair<string, int>("4", 4);
        availableMaxPlayers = new Dictionary<string, int>(){ {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5}, {"6", 6},
                                                             {"7", 7}, {"8", 8}, {"9", 9}, {"10", 10}
                                                           };
        targetKills = new KeyValuePair<string, int>("5", 1);
        availableTargetKills = new Dictionary<string, int>(){ {"5", 5}, {"10", 10}, {"25", 25}, {"50", 50}, {"75", 75}, {"100", 100},
                                                              {"200", 200}, {"500", 500}, {"750", 750}, {"1000", 1000}
	                                                        };
        timer = new KeyValuePair<string, float>("None", 0);
        //availableTimers = new Dictionary<string, float>(){ {"None", -1}, {"10'", 600}, {"15'", 900}, {"20'", 1200}, {"30'", 1800},
        //                                                    {"60'", 3600}, {"90'", 5400}, {"120'", 7200}, {"180'", 10800}
        //                                                  };
        availableTimers = new Dictionary<string, float>(){ {"None", 0}, {"10''", 10}, {"20''", 20}, {"30''", 30},
                                                            {"1'", 60}, {"2'", 120}, {"3'", 180}, {"5'", 300}
                                                          };
    }

    #region Accessors
    public string Title {
        get { return title; }
        set {
            string trimStr = value.TrimStart();
            if (trimStr.Length > 0 && trimStr.Length < 128)
                title = value;
        }
    }
    public KeyValuePair<string, GameMode> Mode {
        get { return mode; }
        set { mode = value; }
    }
    public KeyValuePair<string, GameMap> Map {
        get { return map; }
        set { map = value; }
    }
    public KeyValuePair<string, GameDifficulty> Difficulty {
        get { return difficulty; }
        set { difficulty = value; }
    }
    public KeyValuePair<string, int> MaxPlayers {
        get { return maxPlayers; }
        set { maxPlayers = value; }
    }
    public KeyValuePair<string, int> TargetKills {
        get { return targetKills; }
        set { targetKills = value; }
    }
    public KeyValuePair<string, float> Timer {
        get { return timer; }
        set { timer = value; }
    }

    public Dictionary<string, GameMode> AvailableModes {
        get { return availableModes; }
    }
    public Dictionary<string, GameMap> AvailableMaps {
        get { return availableMaps; }
    }
    public Dictionary<string, GameDifficulty> AvailableDifficulties {
        get { return availableDifficulties; }
    }
    public Dictionary<string, int> AvailableMaxPlayers {
        get { return availableMaxPlayers; }
    }
    public Dictionary<string, int> AvailableTargetKills {
        get { return availableTargetKills; }
    }
    public Dictionary<string, float> AvailableTimers {
        get { return availableTimers; }
    }
    #endregion
}