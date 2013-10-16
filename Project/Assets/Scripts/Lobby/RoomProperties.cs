using System.Collections.Generic;

public class RoomProperties{
	
	public enum GameMode{
		BattleRoyal,
		Conquerors,
		CaptureTheFlag
	};
	public enum Difficulty{
		Easy,
		Medium,
		Hard
	};	
	
	private string title;
	private string host;
	private GameMode mode;
	private Difficulty difficulty;
	private int maxPlayers;
	private int maxKills;
	private double timer;
	private Dictionary<string,BaseSkill> usedSkills, bannedSkills;
	
	public RoomProperties(string t){
		title = t;
		host = null;
		SetMode(GameMode.BattleRoyal);
		maxPlayers = 10;
		difficulty = Difficulty.Medium;
		maxKills = 50;
		timer = -1;
		usedSkills = new Dictionary<string,BaseSkill>();
		bannedSkills = new Dictionary<string,BaseSkill>();
	}
	
	public RoomProperties(string t, string h){
		title = t;
		host = h;
		SetMode(GameMode.BattleRoyal);
		maxPlayers = 10;
		difficulty = Difficulty.Medium;
		maxKills = 50;
		timer = -1;
		usedSkills = new Dictionary<string,BaseSkill>();
		usedSkills.Add("skill 1", new BaseSkill("skill 1", "description 1"));
		usedSkills.Add("skill 2", new BaseSkill("skill 2", "description 2"));
		usedSkills.Add("skill 3", new BaseSkill("skill 3", "description 3"));
		usedSkills.Add("skill 5", new BaseSkill("skill 5", "description 5"));
		
		bannedSkills = new Dictionary<string,BaseSkill>();
		bannedSkills.Add("skill 4", new BaseSkill("skill 4", "description 4"));
	}
		
	
	public void SetTitle(string t){ title = t; }
	public string GetTitle(){ return title; }
	
	public void SetHost(string h){ host = h; }
	public string GetHost(){ return host; }	
	
	public void SetMode(GameMode m){
		mode = m;
		if (m == GameMode.BattleRoyal)
			maxKills = 50;
	}
	public GameMode GetMode(){ return mode; }
	public bool IsBattleRoyal(){ return (mode==GameMode.BattleRoyal); }
	public bool IsConquerors(){ return (mode==GameMode.Conquerors); }
	public bool IsCaptureTheFlag(){ return (mode==GameMode.CaptureTheFlag); }

	public void SetDifficulty(Difficulty dif){ difficulty = dif; }
	public Difficulty GetDifficulty(){ return difficulty; }	
	public bool IsEasy(){ return (difficulty==Difficulty.Easy); }
	public bool IsMedium(){ return (difficulty==Difficulty.Medium); }
	public bool IsHard(){ return (difficulty==Difficulty.Hard); }
	
	public void SetMaxKills(int mk){ maxKills = mk; }
	public int GetMaxKills(){ return maxKills; }
	
	public void SetMaxPlayers(int mp){ maxPlayers = mp; }
	public int GetMaxPlayers(){ return maxPlayers; }
	
	public void SetTimer(double t){ timer = t; }
	public double GetTimer(){ return timer; }
	
	public void BanSkill(string key){
		Utilities.Instance.Assert(usedSkills.ContainsKey(key)&&!bannedSkills.ContainsKey(key),
								  "Error pre, class:RoomProperties, method: BanSkill");
		bannedSkills.Add(key, usedSkills[key]);			
		usedSkills.Remove(key);
		
		Utilities.Instance.Assert(!usedSkills.ContainsKey(key)&&bannedSkills.ContainsKey(key),
								  "Error post, class:RoomProperties, method: BanSkill");
	}
	public void UnbanSkill(string key){
		Utilities.Instance.Assert(!usedSkills.ContainsKey(key)&&bannedSkills.ContainsKey(key),
								  "Error pre, class:RoomProperties, method: UnbanSkill");
		usedSkills.Add(key, bannedSkills[key]);
		bannedSkills.Remove(key);
		
		Utilities.Instance.Assert(usedSkills.ContainsKey(key)&&!bannedSkills.ContainsKey(key),
								  "Error post, class:RoomProperties, method: UnbanSkill");
	}
	public Dictionary<string,BaseSkill> GetUsedSkills(){ return usedSkills; }
	public Dictionary<string,BaseSkill> GetBannedSkills(){ return bannedSkills; }
}
