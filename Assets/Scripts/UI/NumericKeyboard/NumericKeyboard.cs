using UnityEngine;
using System.Collections;
using System;

public class NumericKeyboard : MonoBehaviour {

	private Action<int> OnKeyPressed;

	public void Open (Action<int> keyPressed)
	{
		OnKeyPressed = keyPressed; 
	}

	public void Close ()
	{

	}

	public void PressKey (int number)
	{
		if (OnKeyPressed != null) OnKeyPressed(number);
		Debug.Log("KEY PRESSED: "+number);
	}
}
