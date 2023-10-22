using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AnimationHelper : MonoBehaviour
{ 
	[Serializable]
	public class AnimationEvent : UnityEvent { }
	[SerializeField]
	private AnimationEvent m_eventAnimation = new AnimationEvent();

	public void CallFunction()
	{
		m_eventAnimation.Invoke();
	}
}
