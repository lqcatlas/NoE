
Shader "ShaderMan/PuppyLine"
	{

	Properties{
	_MainTex ("MainTex", 2D) = "white" {}
	}

	SubShader
	{
	Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

	Pass
	{
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha

	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"

	struct VertexInput {
    fixed4 vertex : POSITION;
	fixed2 uv:TEXCOORD0;
    fixed4 tangent : TANGENT;
    fixed3 normal : NORMAL;
	//VertexInput
	};
	struct appdata
	{
		float4 vertex : POSITION;
		float4 color : COLOR;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 color:COLOR;
		//              UNITY_FOG_COORDS(1)
		float4 vertex : SV_POSITION; 
	};


	//Variables
	sampler2D _MainTex;  
	float _Outline;
	sampler2D _AlphaTex;
	float4 _MainTex_TexelSize; 



	v2f vert (VertexInput v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos (v.vertex);
		o.uv = v.uv;
		//VertexFactory
		return o;
	} 
	fixed4 frag(v2f i) : SV_Target
	{
	
		fixed2 uv = i.uv; 

		fixed pencilOffset = floor(abs(cos(_Time.y)) * 6.0) * 0.1;

		uv += (tex2Dlod(_MainTex,
			float4(uv / 5.0 + pencilOffset, 0.1, 0.1))) * 0.01; 
		 

		fixed4 col =  tex2D(_MainTex, uv);  

		return  col; 
	}
	ENDCG
	}
  }
}

