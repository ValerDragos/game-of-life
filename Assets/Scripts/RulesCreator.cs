using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RulesCreator : BaseMonoBehaviour {

	[SerializeField] private Material material;

	[SerializeField] private List<Rule> rules = null;
	// Use this for initialization
	void Awake () {
		Texture2D tex = new Texture2D(9,256,TextureFormat.RGB24, false);
		tex.filterMode = FilterMode.Point;
		tex.wrapMode = TextureWrapMode.Clamp;

		for (int i=0; i< tex.width;i++)
		{
			tex.SetPixel(i,0,new Color(0, 0, 0, 0));
		}
		
		for (int ruleIndex = 0; ruleIndex<rules.Count; ruleIndex++)
		{
			for (int i=0; i< tex.width;i++)
			{
				tex.SetPixel(i,ruleIndex+1,new Color(
					(rules[ruleIndex].forCreateNewCell.Contains(i))? 1f : 0f, 
				    (rules[ruleIndex].forSurviveCell.Contains(i))? 1f : 0f, 
					0, 
					0
					));
			}
		}

		tex.Apply();

		material.SetTexture("_RuleTex", tex);
	}
}

[System.Serializable]
public class Rule
{
	[SerializeField] public List<int> forCreateNewCell = null;
	[SerializeField] public List<int> forSurviveCell = null;
}
