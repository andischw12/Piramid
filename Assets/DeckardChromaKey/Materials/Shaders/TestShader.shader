// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TestShader"
{
	Properties
	{
		_GreenScreenTVStudio("Green-Screen-TV-Studio", 2D) = "white" {}
		_Color0("Color 0", Color) = (0,0,0,0)
		_Float0("Float 0", Range( 0 , 1)) = 0
		_Float1("Float 1", Range( 0 , 1)) = 0
		_Premultiply("Premultiply", Float) = 0
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

			uniform float _Float0;
			uniform float _Float1;
			uniform sampler2D _GreenScreenTVStudio;
			uniform float4 _GreenScreenTVStudio_ST;
			uniform float _Premultiply;
			uniform float4 _Color0;

			
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
				float2 uv_GreenScreenTVStudio = i.ase_texcoord1.xy * _GreenScreenTVStudio_ST.xy + _GreenScreenTVStudio_ST.zw;
				float4 tex2DNode1 = tex2D( _GreenScreenTVStudio, uv_GreenScreenTVStudio );
				float3 break7_g1 = tex2DNode1.rgb;
				float3 break7_g2 = _Color0.rgb;
				float smoothstepResult13 = smoothstep( _Float0 , _Float1 , distance( ( break7_g1.y - ( ( break7_g1.x + break7_g1.z ) * _Premultiply ) ) , ( break7_g2.y - ( ( break7_g2.x + break7_g2.z ) * _Premultiply ) ) ));
				float4 temp_cast_2 = (smoothstepResult13).xxxx;
				
				
				finalColor = temp_cast_2;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18712
316;272;1961;1020;1009.051;873.3894;1.6;True;True
Node;AmplifyShaderEditor.SamplerNode;1;-584.9998,-207.9;Inherit;True;Property;_GreenScreenTVStudio;Green-Screen-TV-Studio;0;0;Create;True;0;0;0;False;0;False;-1;052068d5ead972241a8ad76f4ab6d4e1;052068d5ead972241a8ad76f4ab6d4e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-216,-508.4999;Inherit;False;Property;_Color0;Color 0;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0.7568628,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-75.67765,-156.8657;Inherit;False;Property;_Premultiply;Premultiply;4;0;Create;True;0;0;0;False;0;False;0;1.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;22;187.749,123.4106;Inherit;False;PreKey;-1;;1;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;23;70.94911,-289.3895;Inherit;False;PreKey;-1;;2;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;9;468.1,-48.60001;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;356.5,328;Inherit;False;Property;_Float1;Float 1;3;0;Create;True;0;0;0;False;0;False;0;0.38;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;360.5,245;Inherit;False;Property;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;0;0.29;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;640.7223,-427.2656;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;700.5225,-262.1656;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomExpressionNode;15;877.0003,-281.0002;Inherit;False;float maskY = 0.2989 * KeyColor.r + 0.5866 * KeyColor.g + 0.1145 * KeyColor.b@$float maskCr = 0.7132 * (KeyColor.r - maskY)@$float maskCb = 0.5647 * (KeyColor.b - maskY)@$$float Y = 0.2989 * Color.r + 0.5866 * Color.g + 0.1145 * Color.b@$float Cr = 0.7132 * (Color.r - Y)@$float Cb = 0.5647 * (Color.b - Y)@$$CrCb = float2(Cr, Cb)@$CrCbMask = float2(maskCr, maskCb)@$_Distance = distance(float2(Cr, Cb), float2(maskCr, maskCb))@$return CrCb@$;1;False;5;True;Color;FLOAT3;0,0,0;In;;Inherit;False;True;CrCb;FLOAT2;0,0;Out;;Inherit;False;True;CrCbMask;FLOAT2;0,0;Out;;Inherit;False;True;KeyColor;FLOAT3;0,0,0;In;;Inherit;False;True;_Distance;FLOAT;0;Out;;Inherit;False;YCrCb;True;False;0;5;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;4;FLOAT;0;FLOAT2;2;FLOAT2;3;FLOAT;5
Node;AmplifyShaderEditor.RelayNode;21;-176.2779,251.3344;Inherit;False;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;13;679.5,235;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1156.6,49.80001;Float;False;True;-1;2;ASEMaterialInspector;100;1;TestShader;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;22;1;1;0
WireConnection;22;2;19;0
WireConnection;23;1;4;0
WireConnection;23;2;19;0
WireConnection;9;0;22;0
WireConnection;9;1;23;0
WireConnection;17;0;4;0
WireConnection;17;1;19;0
WireConnection;20;0;1;0
WireConnection;20;1;19;0
WireConnection;15;0;20;0
WireConnection;15;3;17;0
WireConnection;13;0;9;0
WireConnection;13;1;12;0
WireConnection;13;2;14;0
WireConnection;0;0;13;0
ASEEND*/
//CHKSM=7D9392B4C4FAE8FE1052F37576C31C12C4FBF3DD