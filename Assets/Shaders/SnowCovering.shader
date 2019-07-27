Shader "Custom/SnowCovering" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Snow ("Snow Level", Range(0,1)) = 0
     	_SnowColor ("Snow Color", Color) = (1.0,1.0,1.0,1.0)
      	_SnowDirection ("Snow Direction", Vector) = (0,1,0)
      	_SnowDepth ("Snow Depth", Range(0,0.3)) = 0.1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldNormal;
          	INTERNAL_DATA
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Snow;
      	float4 _SnowColor;
      	float4 _SnowDirection;
      	float _SnowDepth;
		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			//o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			//o.Alpha = c.a;

			//if(dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz)>lerp(1,-1,_Snow))
        		//此处我们可以看出_Snow参数只是一个插值项，当上述夹角余弦值大于
        		//lerp(1,-1,_Snow)=1-2*_Snow时，即表示此处积雪覆盖，所以此值越大，
        		//积雪程度程度越大。此时给覆盖积雪的区域填充雪的颜色
        		//o.Albedo = _SnowColor.rgb;
    	  	//else
        		//否则使用物体原先颜色，表示未覆盖积雪 
        		//o.Albedo = tex.rgb;        
          o.Alpha = 1;

          o.Albedo = _SnowColor.rgb;
		}

		void vert (inout appdata_full v) {
          //将_SnowDirection转化到模型的局部坐标系下
          float4 sn = mul(UNITY_MATRIX_IT_MV, _SnowDirection);
 
          if(dot(v.normal, sn.xyz) >= lerp(1,-1, (_Snow*2)/3))
          {
           	v.vertex.xyz += (sn.xyz + v.normal) * _SnowDepth * _Snow;
          }
      	}
		ENDCG
	}
	FallBack "Diffuse"
}
