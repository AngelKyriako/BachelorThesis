using UnityEngine;
using System.Collections;

public class BaseSkill{
	
	private string title;
	private string description;
	
	public BaseSkill(string t, string des){
		title = t;
		description = des;
	}
	
	public string GetTitle(){ return title; }
	
	public string GetDescription(){ return description; }
}
