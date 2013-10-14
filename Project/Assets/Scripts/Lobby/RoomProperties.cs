using UnityEngine;
using System.Collections;

public class RoomProperties : MonoBehaviour {
	
	public enum GameType{
		battleRoyal,
		conquerors,
		captureTheFlag
	};
	public enum Difficulty{
		easy,
		medium,
		hard
	};	
	
	private string name;
	private GameType type;
	private int maxKills;
	private double timer;
	private Difficulty difficulty;
	private int maxDragonSkills;
	
	public RoomProperties(){
		name = "new room";
		SetGameType(GameType.battleRoyal);
		maxKills = 50;
		difficulty = Difficulty.medium;
		timer = -1;
		maxDragonSkills = 4;
	}
	
	public RoomProperties(string n){
		name = n;
		SetGameType(GameType.battleRoyal);
		maxKills = 50;
		difficulty = Difficulty.medium;
		timer = -1;
		maxDragonSkills = 4;
	}
	
	public void SetName(String n){ name = n; }
	public String GetName(){ return name; }
	
	public void SetGameType(GameType t){
		type = t;
		if (t == GameType.battleRoyal)
			maxKills = 50;
	}
	public GameType GetGameType(){ return type; }
	
	public void SetMaxKills(int mk){ maxKills = mk; }
	public int GetMaxKills(){ return maxKills; }
	
	public void SetDifficulty(Difficulty dif){ difficulty = dif; }
	public Difficulty GetDifficulty(){ return difficulty; }
	
	public void SetTimer(double t){ timer = t; }
	public double GetTimer(){ return timer; }
	
	public void SetMaxDragonSkills(int mds){ maxDragonSkills = mds; }
	public int GetMaxDragonSkills(){ return maxDragonSkills; }
}
