using UnityEngine;
using System.Collections;

public class Utilities: MonoBehaviour{
	
	private static Utilities instance = new Utilities();
	
	private Utilities(){ }

	public static Utilities Instance{
		get { return instance; }
	}	
	
	public void Assert(bool b, string s){
		if (!b) Debug.LogError(s);
	}
}
