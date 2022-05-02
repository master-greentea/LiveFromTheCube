using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessage :ICloneable
{
	public int index;
	public string name;
	public string message;
	public int frequency;
	public int positivity;
	public string game;
	public string dependency;

	public object Clone()
	{
		return this.MemberwiseClone();
		throw new NotImplementedException();
	}
}
