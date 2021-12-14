using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
	void Start()
	{
		IEnumerator enumerator = SomeNumbers();
		while (enumerator.MoveNext())
		{
			object result = enumerator.Current;
			Debug.Log("Number: " + result);
		}
	}

	IEnumerator SomeNumbers()
	{
		yield return 3;
		yield return 5;
		yield return 8;
	}
}
