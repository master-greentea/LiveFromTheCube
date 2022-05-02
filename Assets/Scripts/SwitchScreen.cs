using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScreen : MonoBehaviour
{
	[SerializeField] public GameObject[] apps;
	int index;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (TutorialManager.Instance.tutorialFinished)
		{
			for (int i = 0; i < apps.Length; i++)
			{
				if (apps[i].activeInHierarchy)
				{
					index = i;
				}
			}
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				if (index != 0)
				{
					apps[index].SetActive(false);
				}
				if (index < apps.Length - 1)
				{
					apps[index + 1].SetActive(true);
				}
				else
				{
					apps[0].SetActive(true);
				}

			}
		}

		//		Debug.Log(index);
	}
}
