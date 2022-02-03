// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Denoiser"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Size("Size", Range( 0 , 0.01)) = 0
		_Threshold("Threshold", Range( 0 , 1)) = 0
		_ColorQualifier("ColorQualifier", Color) = (0,0,0,0)
		_DeNoiseThreshold("DeNoiseThreshold", Range( 0 , 1)) = 0
		_DeNoiseRange("DeNoiseRange", Range( -1 , 1)) = 0
		[Toggle]_PreviewQualifier("PreviewQualifier", Float) = 0
		_MixWithOriginal("MixWithOriginal", Range( 0 , 1)) = 0
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

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _Size;
			uniform float _Threshold;
			uniform float _DeNoiseThreshold;
			uniform float _DeNoiseRange;
			uniform float4 _ColorQualifier;
			uniform float _PreviewQualifier;
			uniform float _MixWithOriginal;
			float4 DenoiseBlur1( sampler2D tex, float Directions, float Quality, float Size, float2 iResolution, float2 UV0, out float4 Col, float range )
			{
				float Pi = 6.28318530718; 
				float2 Radius = Size/iResolution.xy;
				float2 UW;
				UW = UV0/iResolution.xy;
				float4 Col2 = tex2D(tex, UW);
				Col = 0;
				    
				    for( float d=0.0; d<Pi; d+=Pi/Directions)
				    {
					for(float i=1.0/Quality; i<=1.0; i+=1.0/Quality)
				       	{
					//Col += tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i);
					float4 tempCol = tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i);
				if ( distance(tempCol, Col2) < range)
				Col += tempCol;
				else Col += Col2;
				        	}
				    }
				    Col /= Quality * Directions ;
				    return  Col;
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
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode19 = tex2D( _MainTex, uv_MainTex );
				sampler2D tex1 = _MainTex;
				float Directions1 = 16.0;
				float Quality1 = 8.0;
				float Size1 = _Size;
				float2 iResolution1 = float2( 1,1 );
				float2 UV01 = i.ase_texcoord1.xy;
				float4 Col1 = float4( 0,0,0,0 );
				float range1 = _Threshold;
				float4 localDenoiseBlur1 = DenoiseBlur1( tex1 , Directions1 , Quality1 , Size1 , iResolution1 , UV01 , Col1 , range1 );
				float smoothstepResult22 = smoothstep( _DeNoiseThreshold , ( _DeNoiseThreshold + _DeNoiseRange ) , distance( _ColorQualifier , Col1 ));
				float4 lerpResult26 = lerp( tex2DNode19 , Col1 , smoothstepResult22);
				float4 temp_cast_2 = (smoothstepResult22).xxxx;
				float4 lerpResult27 = lerp( lerpResult26 , temp_cast_2 , (( _PreviewQualifier )?( 1.0 ):( 0.0 )));
				float4 lerpResult29 = lerp( lerpResult27 , tex2DNode19 , _MixWithOriginal);
				float3 temp_output_41_0 = (lerpResult29).rgb;
				float4 appendResult43 = (float4(temp_output_41_0 , (lerpResult29).a));
				
				
				finalColor = appendResult43;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18809
519;585;1408;559;-677.4526;605.8346;1.394679;True;True
Node;AmplifyShaderEditor.RangedFloatNode;15;-297.7868,318.3519;Inherit;False;Property;_Threshold;Threshold;2;0;Create;True;0;0;0;False;0;False;0;0.09;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-676.2939,165.9408;Inherit;False;Property;_Size;Size;1;0;Create;False;0;0;0;False;0;False;0;0.0008;0;0.01;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-743.287,-247.5584;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexCoordVertexDataNode;13;-607.5912,346.6383;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-654.1758,71.6176;Inherit;False;Constant;_Quality;Quality;2;1;[IntRange];Create;True;0;0;0;False;0;False;8;8;1;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-609.8227,-30.2068;Inherit;False;Constant;_Directions;Directions;1;1;[IntRange];Create;False;0;0;0;False;0;False;16;16;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;1;-210.6883,-46.21721;Inherit;False;float Pi = 6.28318530718@ $float2 Radius = Size/iResolution.xy@$float2 UW@$UW = UV0/iResolution.xy@$float4 Col2 = tex2D(tex, UW)@$Col = 0@$    $    for( float d=0.0@ d<Pi@ d+=Pi/Directions)$    {$	for(float i=1.0/Quality@ i<=1.0@ i+=1.0/Quality)$       	{$	//Col += tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i)@$	float4 tempCol = tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i)@$if ( distance(tempCol, Col2) < range)$Col += tempCol@$else Col += Col2@$        	}$    }$    Col /= Quality * Directions @$    return  Col@;4;False;8;True;tex;SAMPLER2D;;In;;Float;False;True;Directions;FLOAT;16;In;;Float;False;True;Quality;FLOAT;4;In;;Float;False;True;Size;FLOAT;10;In;;Float;False;True;iResolution;FLOAT2;1,1;In;;Float;False;True;UV0;FLOAT2;0,0;In;;Float;False;True;Col;FLOAT4;0,0,0,0;Out;;Float;False;True;range;FLOAT;0.15;In;;Inherit;False;DenoiseBlur;True;False;0;8;0;SAMPLER2D;;False;1;FLOAT;16;False;2;FLOAT;4;False;3;FLOAT;10;False;4;FLOAT2;1,1;False;5;FLOAT2;0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT;0.15;False;2;FLOAT4;0;FLOAT4;7
Node;AmplifyShaderEditor.RangedFloatNode;23;155.8785,59.28764;Inherit;False;Property;_DeNoiseThreshold;DeNoiseThreshold;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;-158.8152,-242.1426;Inherit;False;Property;_ColorQualifier;ColorQualifier;4;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0.8937077,0.004716992,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;218.0063,166.2513;Inherit;False;Property;_DeNoiseRange;DeNoiseRange;6;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;18;289.7404,-75.16451;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;410.9994,68.43292;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-67.66087,-444.5201;Inherit;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;22;543.3843,7.698633;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;28;629.9554,171.415;Inherit;False;Property;_PreviewQualifier;PreviewQualifier;7;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;26;565.2365,-213.0379;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;27;846.3105,-110.0121;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;30;880.0997,144.7387;Inherit;False;Property;_MixWithOriginal;MixWithOriginal;8;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;29;1111.472,-141.4384;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;44;1335.783,-76.8582;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;41;1320.183,-195.1582;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-80.92401,224.0635;Inherit;False;Property;_intensity;intensity;3;0;Create;True;0;0;0;False;0;False;0;0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;1611.133,-221.0248;Inherit;False;Property;_Float3;Float 3;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;1999.699,-344.2452;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;36;418.2914,-566.2982;Inherit;False;FLOAT2;4;0;FLOAT;0.9;False;1;FLOAT;1.6;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;48;1606.412,-315.0161;Inherit;False;Property;_Float2;Float 2;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;596.7841,-537.0582;Inherit;False;Constant;_Float1;Float 0;10;0;Create;True;0;0;0;False;0;False;8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;603.2843,-449.9581;Inherit;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;34;824.1779,-599.9841;Inherit;False;float Pi = 6.28318530718@ $float2 Radius = Size/iResolution.xy@$    float2 UW@$UW = UV0/iResolution.xy@$$Col = 0@$    $    for( float d=0.0@ d<Pi@ d+=Pi/Directions)$    {$		for(float i=1.0/Quality@ i<=1.0@ i+=1.0/Quality)$        {$			Col += tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i)@		$        }$    }$    $$    Col /= Quality * Directions@$    return  Col@;4;False;7;True;tex;SAMPLER2D;;In;;Float;False;True;Directions;FLOAT;16;In;;Float;False;True;Quality;FLOAT;2;In;;Float;False;True;Size;FLOAT2;10,0;In;;Float;False;True;iResolution;FLOAT2;1,1;In;;Float;False;True;UV0;FLOAT2;0,0;In;;Float;False;True;Col;FLOAT4;0,0,0,0;Out;;Float;False;GaussianBlur;True;False;0;7;0;SAMPLER2D;;False;1;FLOAT;16;False;2;FLOAT;2;False;3;FLOAT2;10,0;False;4;FLOAT2;1,1;False;5;FLOAT2;0,0;False;6;FLOAT4;0,0,0,0;False;2;FLOAT4;0;FLOAT4;7
Node;AmplifyShaderEditor.RangedFloatNode;33;279.11,-643.1759;Inherit;False;Property;_SharpenSize;SharpenSize;9;0;Create;True;0;0;0;False;0;False;0;0.00234;0;0.01;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;42;1348.784,-409.6579;Inherit;False;True;True;True;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;612.3846,-642.3582;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;67.24343,160.7556;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;43;2023.484,5.041771;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TexelSizeNode;35;373.5541,-468.8069;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;52;1820.242,-326.0664;Inherit;False;Property;_Float4;Float 3;12;0;Create;True;0;0;0;False;0;False;0;17.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;1759.469,-250.7837;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;2013.177,-228.5934;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;40;1620.152,-419.8333;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;32;2197.63,-223.4316;Float;False;True;-1;2;ASEMaterialInspector;100;1;Denoiser;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;1;0;2;0
WireConnection;1;1;3;0
WireConnection;1;2;4;0
WireConnection;1;3;5;0
WireConnection;1;5;13;0
WireConnection;1;7;15;0
WireConnection;18;0;21;0
WireConnection;18;1;1;7
WireConnection;24;0;23;0
WireConnection;24;1;25;0
WireConnection;19;0;2;0
WireConnection;22;0;18;0
WireConnection;22;1;23;0
WireConnection;22;2;24;0
WireConnection;26;0;19;0
WireConnection;26;1;1;7
WireConnection;26;2;22;0
WireConnection;27;0;26;0
WireConnection;27;1;22;0
WireConnection;27;2;28;0
WireConnection;29;0;27;0
WireConnection;29;1;19;0
WireConnection;29;2;30;0
WireConnection;44;0;29;0
WireConnection;41;0;29;0
WireConnection;51;0;40;0
WireConnection;51;1;52;0
WireConnection;34;0;2;0
WireConnection;34;1;39;0
WireConnection;34;2;38;0
WireConnection;34;3;37;0
WireConnection;34;5;13;0
WireConnection;42;0;34;7
WireConnection;37;0;33;0
WireConnection;37;1;36;0
WireConnection;16;0;1;7
WireConnection;16;1;17;0
WireConnection;43;0;41;0
WireConnection;43;3;44;0
WireConnection;35;0;2;0
WireConnection;50;0;48;0
WireConnection;50;1;49;0
WireConnection;46;0;51;0
WireConnection;40;0;41;0
WireConnection;40;1;42;0
WireConnection;32;0;43;0
ASEEND*/
//CHKSM=4357BB69E859935E86829011EDD86288C6535D98