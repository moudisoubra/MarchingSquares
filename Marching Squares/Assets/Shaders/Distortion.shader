Shader "Soubra/Transparent UVDistort"
{
	Properties
	{
		_ColorA("Main Color A", Color) = (1,1,1)
		_ColorB("Main Color B", Color) = (1,1,1)
		_MainTex("Texture", 2D) = "white" {}
	_MainScroll("MainTex Scroll/Distort", Range(-2,2)) = 1
	_Offset("Offset Gradient A/B", Range(-2,2)) = 1
		_XSpeed("X Scrolling Speed", Range(-2,2)) = 1
		_YSpeed("Y Scrolling Speed", Range(-4,4)) = 1
	}
		SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		LOD 100
		Zwrite Off
		Blend One One // additive blending

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD1;
			float4 vertex : SV_POSITION;
		float4 col : COLOR;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float4 _ColorA, _ColorB, _Tint;
	float _Offset, _XSpeed, _YSpeed, _MainScroll;
	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		o.col = lerp(_ColorA, _ColorB, v.vertex.y + _Offset); // gradient over vertex
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{

	fixed4 maintex = tex2D(_MainTex, i.uv + (_Time.x * _MainScroll)) * i.col; // first texture

	UNITY_APPLY_FOG(i.fogCoord, maintex);
	return maintex;
	}
		ENDCG
	}
	}
}