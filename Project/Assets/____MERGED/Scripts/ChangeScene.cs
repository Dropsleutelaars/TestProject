using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {	
	
	// Update is called once per frame

    public Animator startButton;
    public Animator settingsButton;
    public Animator dialog;

    public void OpenSettings()
    {
        startButton.SetBool("isHidden", true);
        settingsButton.SetBool("isHidden", true);
        dialog.enabled = true;
        dialog.SetBool("isHidden", false);
    }

    public void CloseSettings()
    {
        startButton.SetBool("isHidden", false);
        settingsButton.SetBool("isHidden", false);
        dialog.SetBool("isHidden", true);
    }

	public void ChangeToScene (int sceneToChangeTo) 
    {
        Application.LoadLevel(sceneToChangeTo);

	}
}
