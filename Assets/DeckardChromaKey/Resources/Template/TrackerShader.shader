// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TrackerShader"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_128("128", Float) = 128
		_Multiply("Multiply", Float) = 1

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

			uniform sampler2D _MainTex;
			uniform float _128;
			uniform float _Multiply;
			float4 HorizontalBlur1( sampler2D tex, float2 uv_Texture0, float Iterations, out float4 finalColor, float2 Direction )
			{
				float2 Offset = Direction * (1/Iterations);
				for(float i=1; i<=Iterations; i++)
				{ 
				if (uv_Texture0.x <0.01)
				finalColor = step(  tex2D( tex, uv_Texture0 + (Offset * i)), 0.8) + finalColor;
				}
				finalColor = finalColor/Iterations;			return finalColor;
			}
			
			float4 HorizontalBlur8( sampler2D tex, float2 uv_Texture0, float Iterations, out float4 finalColor, float2 Direction )
			{
				float2 Offset = Direction * (1/Iterations);
				for(float i=1; i<=Iterations; i++)
				{ 
				if (uv_Texture0.y <0.01)
				finalColor = step(  tex2D( tex, uv_Texture0 + (Offset * i)), 0.8) + finalColor;
				}
				finalColor = finalColor/Iterations;			return finalColor;
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
				sampler2D tex1 = _MainTex;
				float2 texCoord3 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv_Texture01 = texCoord3;
				float Iterations1 = _128;
				float4 finalColor1 = float4( 0,0,0,0 );
				float2 Direction1 = float2( 1,0 );
				float4 localHorizontalBlur1 = HorizontalBlur1( tex1 , uv_Texture01 , Iterations1 , finalColor1 , Direction1 );
				sampler2D tex8 = _MainTex;
				float2 uv_Texture08 = texCoord3;
				float Iterations8 = _128;
				float4 finalColor8 = float4( 0,0,0,0 );
				float2 Direction8 = float2( 0,1 );
				float4 localHorizontalBlur8 = HorizontalBlur8( tex8 , uv_Texture08 , Iterations8 , finalColor8 , Direction8 );
				float3 appendResult10 = (float3(( 1.0 - finalColor1.w ) , ( 1.0 - finalColor8.w ) , tex2D( _MainTex, texCoord3 ).a));
				float4 appendResult19 = (float4(( appendResult10 * _Multiply ) , 1.0));
				
				
				finalColor = appendResult19;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18712
956;507;1408;815;917.7639;382.3652;1;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;2;-561,-250.5;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-547,-45.5;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;6;-448,210.5;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;0;False;0;False;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;9;-433.3359,336.7813;Inherit;False;Constant;_Vector1;Vector 1;2;0;Create;True;0;0;0;False;0;False;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;5;-535,88.5;Inherit;False;Property;_128;128;1;0;Create;True;0;0;0;False;0;False;128;128;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;8;-197.0123,177.8023;Float;False;$float2 Offset = Direction * (1/Iterations)@$$for(float i=1@ i<=Iterations@ i++)${ $if (uv_Texture0.y <0.01)$finalColor = step(  tex2D( tex, uv_Texture0 + (Offset * i)), 0.8) + finalColor@$}$$finalColor = finalColor/Iterations@			return finalColor@;4;False;5;True;tex;SAMPLER2D;sampler02;In;;Float;False;True;uv_Texture0;FLOAT2;0,0;In;;Float;False;True;Iterations;FLOAT;128;In;;Float;False;True;finalColor;FLOAT4;0,0,0,0;Out;;Float;False;True;Direction;FLOAT2;1,0;In;;Inherit;False;HorizontalBlur;True;False;0;5;0;SAMPLER2D;sampler02;False;1;FLOAT2;0,0;False;2;FLOAT;128;False;3;FLOAT4;0,0,0,0;False;4;FLOAT2;1,0;False;2;FLOAT4;0;FLOAT4;4
Node;AmplifyShaderEditor.CustomExpressionNode;1;-230.5,-91.39999;Float;False;$float2 Offset = Direction * (1/Iterations)@$$for(float i=1@ i<=Iterations@ i++)${ $if (uv_Texture0.x <0.01)$finalColor = step(  tex2D( tex, uv_Texture0 + (Offset * i)), 0.8) + finalColor@$}$$$finalColor = finalColor/Iterations@			return finalColor@;4;False;5;True;tex;SAMPLER2D;sampler02;In;;Float;False;True;uv_Texture0;FLOAT2;0,0;In;;Float;False;True;Iterations;FLOAT;128;In;;Float;False;True;finalColor;FLOAT4;0,0,0,0;Out;;Float;False;True;Direction;FLOAT2;1,0;In;;Inherit;False;HorizontalBlur;True;False;0;5;0;SAMPLER2D;sampler02;False;1;FLOAT2;0,0;False;2;FLOAT;128;False;3;FLOAT4;0,0,0,0;False;4;FLOAT2;1,0;False;2;FLOAT4;0;FLOAT4;4
Node;AmplifyShaderEditor.BreakToComponentsNode;11;93.31235,190.0315;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BreakToComponentsNode;4;70.36718,-154.115;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;20;262.9139,-300.0669;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;22;288.8915,214.2434;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;21;240.5508,-96.87312;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;459.5862,2.473792;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;14;372,339.5;Inherit;False;Property;_Multiply;Multiply;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;622.8,3.199888;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-7,82.10001;Inherit;False;Property;_SearchThreshold;SearchThreshold;2;0;Create;True;0;0;0;False;0;False;0.1;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;785.2084,117.5712;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;12;124,352.5;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1013.2,148.5;Float;False;True;-1;2;ASEMaterialInspector;100;1;TrackerShader;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;8;0;2;0
WireConnection;8;1;3;0
WireConnection;8;2;5;0
WireConnection;8;4;9;0
WireConnection;1;0;2;0
WireConnection;1;1;3;0
WireConnection;1;2;5;0
WireConnection;1;4;6;0
WireConnection;11;0;8;4
WireConnection;4;0;1;4
WireConnection;20;0;2;0
WireConnection;20;1;3;0
WireConnection;22;0;11;3
WireConnection;21;0;4;3
WireConnection;10;0;21;0
WireConnection;10;1;22;0
WireConnection;10;2;20;4
WireConnection;13;0;10;0
WireConnection;13;1;14;0
WireConnection;19;0;13;0
WireConnection;0;0;19;0
ASEEND*/
//CHKSM=6352C02A3F36DB682E209A8C0D8CC0A74E00BE04