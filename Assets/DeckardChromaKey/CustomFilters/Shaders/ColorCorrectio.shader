// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ColorCorrection"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_midpoint("midpoint", Range( 0 , 1)) = 0.5
		_Contrast("Contrast", Float) = 1
		_Saturation("Saturation", Range( 0 , 2)) = 0
		_Shadows("Shadows", Range( -1 , 1)) = 0
		_Highligts("Highligts", Range( 0 , 2)) = 1
		_LuminanceBased("LuminanceBased", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _midpoint;
			uniform float _Contrast;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _Saturation;
			uniform float _Shadows;
			uniform float _Highligts;
			uniform float _LuminanceBased;
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}
			float3 MyCustomExpression1_g1( float Midpoint, float Contrast, float3 In )
			{
				float midpoint = pow(Midpoint, 2.2);
				    return  (In - midpoint) * Contrast + midpoint;
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float Midpoint1_g1 = _midpoint;
				float Contrast1_g1 = _Contrast;
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode1 = tex2D( _MainTex, uv_MainTex );
				float3 In1_g1 = tex2DNode1.rgb;
				float3 localMyCustomExpression1_g1 = MyCustomExpression1_g1( Midpoint1_g1 , Contrast1_g1 , In1_g1 );
				float3 hsvTorgb7 = RGBToHSV( localMyCustomExpression1_g1 );
				float temp_output_9_0 = ( hsvTorgb7.y * _Saturation );
				float3 hsvTorgb22 = HSVToRGB( float3(hsvTorgb7.x,temp_output_9_0,(_Shadows + (hsvTorgb7.z - 0.0) * (_Highligts - _Shadows) / (1.0 - 0.0))) );
				float4 appendResult25 = (float4(hsvTorgb22 , tex2DNode1.a));
				float3 hsvTorgb8 = HSVToRGB( float3(hsvTorgb7.x,temp_output_9_0,hsvTorgb7.z) );
				float4 appendResult6 = (float4(hsvTorgb8 , tex2DNode1.a));
				float4 appendResult12 = (float4(_Shadows , _Shadows , _Shadows , 0.0));
				float4 appendResult16 = (float4(_Highligts , _Highligts , _Highligts , 1.0));
				float4 lerpResult23 = lerp( appendResult25 , (appendResult12 + (appendResult6 - float4(0,0,0,0)) * (appendResult16 - appendResult12) / (float4(1,1,1,1) - float4(0,0,0,0))) , _LuminanceBased);
				
				
				finalColor = lerpResult23;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18809
641;303;1901;1154;929.5507;465.2129;1.3;True;True
Node;AmplifyShaderEditor.SamplerNode;1;-472,-135.5;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-341,242.5;Inherit;False;Property;_Contrast;Contrast;2;0;Create;True;0;0;0;False;0;False;1;1.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-436,113.5;Inherit;False;Property;_midpoint;midpoint;1;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;3;-131,130.6;Inherit;False;ContrastMidpoint;-1;;1;82fa6eec51a71ba41a3d531aa7afcea5;0;3;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;10;3.8569,32.74866;Inherit;False;Property;_Saturation;Saturation;3;0;Create;True;0;0;0;False;0;False;0;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;7;-100.84,-144.3873;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;224.6569,12.04866;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-0.701333,315.4373;Inherit;False;Property;_Shadows;Shadows;4;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-7.201344,475.337;Inherit;False;Property;_Highligts;Highligts;5;0;Create;True;0;0;0;False;0;False;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;21;245.6493,-392.4129;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;8;416.373,-125.8474;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector4Node;19;134.4987,787.3368;Inherit;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.HSVToRGBNode;22;558.9493,-363.8129;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;16;287.899,463.6372;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector4Node;20;573.8985,700.2369;Inherit;False;Constant;_Vector1;Vector 0;6;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;12;313.8988,312.8374;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;322.926,189.7809;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;11;632.3979,268.6369;Inherit;False;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;795.5492,-265.0129;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;24;591.4493,111.9871;Inherit;False;Property;_LuminanceBased;LuminanceBased;6;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;23;834.5493,-11.5129;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1000.93,-112.7739;Float;False;True;-1;2;ASEMaterialInspector;100;1;ColorCorrection;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;3;2;4;0
WireConnection;3;3;5;0
WireConnection;3;4;1;0
WireConnection;7;0;3;0
WireConnection;9;0;7;2
WireConnection;9;1;10;0
WireConnection;21;0;7;3
WireConnection;21;3;13;0
WireConnection;21;4;15;0
WireConnection;8;0;7;1
WireConnection;8;1;9;0
WireConnection;8;2;7;3
WireConnection;22;0;7;1
WireConnection;22;1;9;0
WireConnection;22;2;21;0
WireConnection;16;0;15;0
WireConnection;16;1;15;0
WireConnection;16;2;15;0
WireConnection;12;0;13;0
WireConnection;12;1;13;0
WireConnection;12;2;13;0
WireConnection;6;0;8;0
WireConnection;6;3;1;4
WireConnection;11;0;6;0
WireConnection;11;1;19;0
WireConnection;11;2;20;0
WireConnection;11;3;12;0
WireConnection;11;4;16;0
WireConnection;25;0;22;0
WireConnection;25;3;1;4
WireConnection;23;0;25;0
WireConnection;23;1;11;0
WireConnection;23;2;24;0
WireConnection;0;0;23;0
ASEEND*/
//CHKSM=48AA012E805CEC3D658F6E90648262EB88BF7C37