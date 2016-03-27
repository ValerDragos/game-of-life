Shader "Processers/View" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_TextureSheet ("Dot", 2D) = "white" {}
		_ColorSample ("Color Sample", 2D) = "white" {}
		
		_VerticalLimit ("Vertical Limite", float) = 1
		
		_DeadColor ("Dead Color", Color) = (0,0,0,0)
	}
	SubShader {
	 	
		Tags 
		{ 
			"Queue" = "Geometry" 
			"IgnoreProjector" = "True"
		}
		
		Pass
		{
			ZTest Off Cull Off ZWrite Off Blend Off
	  		Fog { Mode off } 
		
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360 because it uses wrong array syntax (type[size] name)
			#pragma exclude_renderers d3d11 xbox360
			//#pragma target 2.0
			//#pragma glsl
			#pragma vertex vert 
			#pragma fragment frag
			
			uniform sampler2D _MainTex;
			uniform sampler2D _MainTex_TexelSize;
			uniform sampler2D _TextureSheet;
			uniform sampler2D _ColorSample;
			
			uniform float _VerticalLimit;
			
			uniform fixed4 _DeadColor;
			
			// base input structs
			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : Color;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD0;
				fixed4 color: Color;
			};
			
			// vertex function
			vertexOutput vert (vertexInput v)
			{
				vertexOutput o; 
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = v.texcoord;
				o.color = v.color;
				
				return o;
			} 
			
			// fragment function
			fixed4 frag(vertexOutput i) : COLOR
			{
				float4 sample = tex2D(_MainTex, i.tex);
				//fixed dotSample = tex2D(_Dot, i.tex * _Dot_ST.xy + _Dot_ST.zw).w;
				//if (sample.x == 0) return fixed4(0.0,0.0,0.0,0.0);
				//else
				//{
				//float factor = (sample.z*255.0+sample.w)/256.0;
				fixed4 colorSample = tex2D(_ColorSample, half2(sample.z,sample.y / _VerticalLimit));
				return sample.x * colorSample;// * dotSample;
				//}
				
			}
			ENDCG
		}
	} 
	FallBack Off
}
