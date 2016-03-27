using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RenderTextureDrawer;
using FileManager;

namespace GOL
{
	public class MaterialMultyCompileFlags
	{
		private static readonly string[] SAMPLE_AMOUNT = new string[] {"SAMPLES_1", "SAMPLES_2", "SAMPLES_3", "SAMPLES_4", "SAMPLES_5", "SAMPLES_6", "SAMPLES_7", "SAMPLES_8"};
		private static readonly string[] LIFE_TIME = new string[] {"LIFETIME", "LIFETIME_AND_DIE", "NO_LIFETIME"};
		private static readonly string[] ANIMATION = new string[] {"INCREMENT_ANIMATION_VALUE", "DONT_INCREMENT_ANIMATION_VALUE"};

		public enum LifeTime
		{
			LifeTime = 0,
			LifeTimeAndDie = 1,
			NoLifeTime = 2
		}
		public enum Animation
		{
			IncrementWComponent = 0,
			DontIncrementWComponent = 1
		}

		private Material _material;

		public MaterialMultyCompileFlags (Material material)
		{
			_material = material;
		}

		public void SetSampleAmountFlag (int amountOfSamples)
		{
			amountOfSamples = Mathf.Clamp (amountOfSamples - 1, 0, 7);
			for (int i=0; i<SAMPLE_AMOUNT.Length; i++) 
			{
				if (i == amountOfSamples) Shader.EnableKeyword( SAMPLE_AMOUNT[i] );
				else Shader.DisableKeyword( SAMPLE_AMOUNT[i] );
			}
		}
		
		public void SetLifeTimeFlag (LifeTime ValueType)
		{
			foreach (LifeTime val in Enum.GetValues(typeof(LifeTime)))
			{
				if (val == ValueType) Shader.EnableKeyword( LIFE_TIME[(int)val] );
				else Shader.DisableKeyword( LIFE_TIME[(int)val] );
			}
		}
		
		public void SetAnimationFlag (Animation ValueType)
		{
			foreach (Animation val in Enum.GetValues(typeof(Animation)))
			{
				if (val == ValueType) Shader.EnableKeyword( ANIMATION[(int)val] );
				else Shader.DisableKeyword( ANIMATION[(int)val] );
			}
		}
	}
}
