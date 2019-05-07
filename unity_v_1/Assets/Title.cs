using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	uint exitCountValue = 0;
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			exitCountValue++;
			if (!IsInvoking("disable_DoubleClick"))
				Invoke("disable_DoubleClick", 0.3f);
		}
		if (exitCountValue == 2)
		{
			CancelInvoke("disable_DoubleClick");
			Application.Quit();
		}
	}

	void disable_DoubleClick()
	{
		exitCountValue = 0;
	}

	//void Update()
	//{
	//	if (Application.platform == RuntimePlatform.Android)
	//	{
	//		if (Input.GetKeyDown(KeyCode.Escape))
	//		{
	//			Application.Quit();
	//		}
	//	}
	//}

	public void Click()
	{
		SceneManager.LoadScene(1);
	}

	public void Click2()
	{
		SceneManager.LoadScene(2);
	}
}
