using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void LoadLevel (string name) {		
		Debug.Log ("New Level load: " + name);		
		SceneManager.LoadScene (name, LoadSceneMode.Single);
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}

}
