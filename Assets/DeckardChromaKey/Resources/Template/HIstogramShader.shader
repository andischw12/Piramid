// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HistogramShader"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_128("128", Float) = 128
		_Contrast("Contrast", Float) = 0
		_PreCorrectGamma("PreCorrectGamma", Float) = 0
		_Float1("Float 1", Float) = 0

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

			uniform float _Float1;
			uniform sampler2D _MainTex;
			uniform float _128;
			uniform float _PreCorrectGamma;
			uniform float _Contrast;
			float4 HorizontalBlur1( sampler2D tex, float2 uv_Texture0, float Iterations, out float4 finalColor, float2 Direction )
			{
				float2 Offset = Direction * (1/Iterations);
				for(float i=1; i<=Iterations; i++)
				{ 
				finalColor = tex2D( tex, uv_Texture0 + (Offset * i))+ finalColor;
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
				float2 Direction1 = float2( 0,1 );
				float4 localHorizontalBlur1 = HorizontalBlur1( tex1 , uv_Texture01 , Iterations1 , finalColor1 , Direction1 );
				float4 temp_cast_0 = (_PreCorrectGamma).xxxx;
				float4 break32 = pow( finalColor1 , temp_cast_0 );
				float3 appendResult44 = (float3(break32.x , break32.y , break32.z));
				float grayscale45 = Luminance(appendResult44);
				float smoothstepResult47 = smoothstep( 0.0 , _Float1 , distance( grayscale45 , i.ase_texcoord1.xy.y ));
				float smoothstepResult33 = smoothstep( 0.0 , _Contrast , distance( break32.x , i.ase_texcoord1.xy.y ));
				float smoothstepResult35 = smoothstep( 0.0 , _Contrast , distance( break32.y , i.ase_texcoord1.xy.y ));
				float smoothstepResult39 = smoothstep( 0.0 , _Contrast , distance( break32.z , i.ase_texcoord1.xy.y ));
				float3 appendResult19 = (float3(( 1.0 - smoothstepResult33 ) , ( 1.0 - smoothstepResult35 ) , ( 1.0 - smoothstepResult39 )));
				float4 appendResult52 = (float4(( ( 1.0 - smoothstepResult47 ) + appendResult19 ) , 1.0));
				
				
				finalColor = appendResult52;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18809
499;211;1458;1117;678.9992;559.0222;1.6;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-547,-45.5;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-535,88.5;Inherit;False;Property;_128;128;1;0;Create;True;0;0;0;False;0;False;128;32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-599,-298.5;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;None;6366cec5ad51a4d48a449c35c0ce31e1;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.Vector2Node;6;-448,210.5;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;0;False;0;False;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;53;-81.72928,411.5485;Inherit;False;Property;_PreCorrectGamma;PreCorrectGamma;5;0;Create;True;0;0;0;False;0;False;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;1;-253.5,-87.39999;Float;False;$float2 Offset = Direction * (1/Iterations)@$$for(float i=1@ i<=Iterations@ i++)${ $$finalColor = tex2D( tex, uv_Texture0 + (Offset * i))+ finalColor@$$}$$finalColor = finalColor/Iterations@			return finalColor@;4;False;5;True;tex;SAMPLER2D;sampler02;In;;Float;False;True;uv_Texture0;FLOAT2;0,0;In;;Float;False;True;Iterations;FLOAT;128;In;;Float;False;True;finalColor;FLOAT4;0,0,0,0;Out;;Float;False;True;Direction;FLOAT2;1,0;In;;Inherit;False;HorizontalBlur;True;False;0;5;0;SAMPLER2D;sampler02;False;1;FLOAT2;0,0;False;2;FLOAT;128;False;3;FLOAT4;0,0,0,0;False;4;FLOAT2;1,0;False;2;FLOAT4;0;FLOAT4;4
Node;AmplifyShaderEditor.PowerNode;54;309.2557,166.8275;Inherit;False;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;32;584.3781,241.1028;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TexCoordVertexDataNode;36;593.5234,729.3672;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;44;948.2066,29.51371;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DistanceOpNode;41;866.9389,918.9974;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;45;1098.629,50.54052;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;941.7369,1119.671;Inherit;False;Property;_Contrast;Contrast;4;0;Create;True;0;0;0;False;0;False;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;37;880.4426,691.409;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;31;909.2901,458.5615;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;35;1076.231,696.0447;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;1091.639,-117.6587;Inherit;False;Property;_Float1;Float 1;6;0;Create;True;0;0;0;False;0;False;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;33;1105.078,463.1972;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;39;1062.727,923.6331;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;46;1016.842,-318.3325;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;34;1321.711,486.9113;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;47;1212.63,-313.6967;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;42;1285.81,908.1639;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;38;1299.314,680.5755;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;48;1390.424,-269.3205;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;1467.857,268.9838;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;1452.849,120.0906;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector2Node;9;-433.3359,336.7813;Inherit;False;Constant;_Vector1;Vector 1;2;0;Create;True;0;0;0;False;0;False;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;21;240.5508,-96.87312;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;165,569.5;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;11;56.31235,190.0315;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;20;262.9139,-300.0669;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;52;1723.839,119.033;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;22;197.8915,400.2434;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;622.8,3.199888;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;4;70.36718,-154.115;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;10;459.5862,2.473792;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;14;372,339.5;Inherit;False;Property;_Multiply;Multiply;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-7,82.10001;Inherit;False;Property;_SearchThreshold;SearchThreshold;2;0;Create;True;0;0;0;False;0;False;0.1;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;8;-197.0123,177.8023;Float;False;$float2 Offset = Direction * (1/Iterations)@$$for(float i=1@ i<=Iterations@ i++)${ $if (uv_Texture0.y <0.01)$finalColor = step(  tex2D( tex, uv_Texture0 + (Offset * i)), 0.8) + finalColor@$}$$finalColor = finalColor/Iterations@			return finalColor@;4;False;5;True;tex;SAMPLER2D;sampler02;In;;Float;False;True;uv_Texture0;FLOAT2;0,0;In;;Float;False;True;Iterations;FLOAT;128;In;;Float;False;True;finalColor;FLOAT4;0,0,0,0;Out;;Float;False;True;Direction;FLOAT2;1,0;In;;Inherit;False;HorizontalBlur;True;False;0;5;0;SAMPLER2D;sampler02;False;1;FLOAT2;0,0;False;2;FLOAT;128;False;3;FLOAT4;0,0,0,0;False;4;FLOAT2;1,0;False;2;FLOAT4;0;FLOAT4;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;51;2025.437,118.3161;Float;False;True;-1;2;ASEMaterialInspector;100;1;HistogramShader;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;1;0;2;0
WireConnection;1;1;3;0
WireConnection;1;2;5;0
WireConnection;1;4;6;0
WireConnection;54;0;1;4
WireConnection;54;1;53;0
WireConnection;32;0;54;0
WireConnection;44;0;32;0
WireConnection;44;1;32;1
WireConnection;44;2;32;2
WireConnection;41;0;32;2
WireConnection;41;1;36;2
WireConnection;45;0;44;0
WireConnection;37;0;32;1
WireConnection;37;1;36;2
WireConnection;31;0;32;0
WireConnection;31;1;36;2
WireConnection;35;0;37;0
WireConnection;35;2;43;0
WireConnection;33;0;31;0
WireConnection;33;2;43;0
WireConnection;39;0;41;0
WireConnection;39;2;43;0
WireConnection;46;0;45;0
WireConnection;46;1;36;2
WireConnection;34;0;33;0
WireConnection;47;0;46;0
WireConnection;47;2;49;0
WireConnection;42;0;39;0
WireConnection;38;0;35;0
WireConnection;48;0;47;0
WireConnection;19;0;34;0
WireConnection;19;1;38;0
WireConnection;19;2;42;0
WireConnection;50;0;48;0
WireConnection;50;1;19;0
WireConnection;21;0;4;3
WireConnection;11;0;8;4
WireConnection;20;0;2;0
WireConnection;20;1;3;0
WireConnection;52;0;50;0
WireConnection;22;0;11;3
WireConnection;13;0;10;0
WireConnection;13;1;14;0
WireConnection;4;0;1;4
WireConnection;10;0;21;0
WireConnection;10;1;22;0
WireConnection;10;2;20;4
WireConnection;8;0;2;0
WireConnection;8;1;3;0
WireConnection;8;2;5;0
WireConnection;8;4;9;0
WireConnection;51;0;52;0
ASEEND*/
//CHKSM=052E1774700CDCA68895CD846C41F96873917D29