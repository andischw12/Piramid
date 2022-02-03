// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/DepthToNormal"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_strength("strength", Float) = 0
		_ColorInfluence("ColorInfluence", Range( 0 , 1)) = 0
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
				float4 ase_tangent : TANGENT;
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _strength;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			uniform float _ColorInfluence;
			void CalculateUVsSmooth46_g4( float2 UV, float4 TexelSize, out float2 UV0, out float2 UV1, out float2 UV2, out float2 UV3, out float2 UV4, out float2 UV5, out float2 UV6, out float2 UV7, out float2 UV8 )
			{
				{
				    float3 pos = float3( TexelSize.xy, 0 );
				    float3 neg = float3( -pos.xy, 0 );
				    UV0 = UV + neg.xy;
				    UV1 = UV + neg.zy;
				    UV2 = UV + float2( pos.x, neg.y );
				    UV3 = UV + neg.xz;
				    UV4 = UV;
				    UV5 = UV + pos.xz;
				    UV6 = UV + float2( neg.x, pos.y );
				    UV7 = UV + pos.zy;
				    UV8 = UV + pos.xy;
				    return;
				}
			}
			
			float3 CombineSamplesSmooth58_g4( float Strength, float S0, float S1, float S2, float S3, float S4, float S5, float S6, float S7, float S8 )
			{
				{
				    float3 normal;
				    normal.x = Strength * ( S0 - S2 + 2 * S3 - 2 * S5 + S6 - S8 );
				    normal.y = Strength * ( S0 + 2 * S1 + S2 - S6 - 2 * S7 - S8 );
				    normal.z = 1.0;
				    return normalize( normal );
				}
			}
			
			void CalculateUVsSmooth46_g3( float2 UV, float4 TexelSize, out float2 UV0, out float2 UV1, out float2 UV2, out float2 UV3, out float2 UV4, out float2 UV5, out float2 UV6, out float2 UV7, out float2 UV8 )
			{
				{
				    float3 pos = float3( TexelSize.xy, 0 );
				    float3 neg = float3( -pos.xy, 0 );
				    UV0 = UV + neg.xy;
				    UV1 = UV + neg.zy;
				    UV2 = UV + float2( pos.x, neg.y );
				    UV3 = UV + neg.xz;
				    UV4 = UV;
				    UV5 = UV + pos.xz;
				    UV6 = UV + float2( neg.x, pos.y );
				    UV7 = UV + pos.zy;
				    UV8 = UV + pos.xy;
				    return;
				}
			}
			
			float3 CombineSamplesSmooth58_g3( float Strength, float S0, float S1, float S2, float S3, float S4, float S5, float S6, float S7, float S8 )
			{
				{
				    float3 normal;
				    normal.x = Strength * ( S0 - S2 + 2 * S3 - 2 * S5 + S6 - S8 );
				    normal.y = Strength * ( S0 + 2 * S1 + S2 - S6 - 2 * S7 - S8 );
				    normal.z = 1.0;
				    return normalize( normal );
				}
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldTangent = UnityObjectToWorldDir(v.ase_tangent);
				o.ase_texcoord2.xyz = ase_worldTangent;
				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord3.xyz = ase_worldNormal;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord4.xyz = ase_worldBitangent;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				o.ase_texcoord2.w = 0;
				o.ase_texcoord3.w = 0;
				o.ase_texcoord4.w = 0;
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
				float temp_output_91_0_g4 = _strength;
				float Strength58_g4 = temp_output_91_0_g4;
				float localCalculateUVsSmooth46_g4 = ( 0.0 );
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 temp_output_85_0_g4 = uv_MainTex;
				float2 UV46_g4 = temp_output_85_0_g4;
				float4 TexelSize46_g4 = _MainTex_TexelSize;
				float2 UV046_g4 = float2( 0,0 );
				float2 UV146_g4 = float2( 0,0 );
				float2 UV246_g4 = float2( 0,0 );
				float2 UV346_g4 = float2( 0,0 );
				float2 UV446_g4 = float2( 0,0 );
				float2 UV546_g4 = float2( 0,0 );
				float2 UV646_g4 = float2( 0,0 );
				float2 UV746_g4 = float2( 0,0 );
				float2 UV846_g4 = float2( 0,0 );
				CalculateUVsSmooth46_g4( UV46_g4 , TexelSize46_g4 , UV046_g4 , UV146_g4 , UV246_g4 , UV346_g4 , UV446_g4 , UV546_g4 , UV646_g4 , UV746_g4 , UV846_g4 );
				float4 break140_g4 = tex2D( _MainTex, UV046_g4 );
				float S058_g4 = break140_g4.a;
				float4 break142_g4 = tex2D( _MainTex, UV146_g4 );
				float S158_g4 = break142_g4.a;
				float4 break146_g4 = tex2D( _MainTex, UV246_g4 );
				float S258_g4 = break146_g4.a;
				float4 break148_g4 = tex2D( _MainTex, UV346_g4 );
				float S358_g4 = break148_g4.a;
				float4 break150_g4 = tex2D( _MainTex, UV446_g4 );
				float S458_g4 = break150_g4.a;
				float4 break152_g4 = tex2D( _MainTex, UV546_g4 );
				float S558_g4 = break152_g4.a;
				float4 break154_g4 = tex2D( _MainTex, UV646_g4 );
				float S658_g4 = break154_g4.a;
				float4 break156_g4 = tex2D( _MainTex, UV746_g4 );
				float S758_g4 = break156_g4.a;
				float4 break158_g4 = tex2D( _MainTex, UV846_g4 );
				float S858_g4 = break158_g4.a;
				float3 localCombineSamplesSmooth58_g4 = CombineSamplesSmooth58_g4( Strength58_g4 , S058_g4 , S158_g4 , S258_g4 , S358_g4 , S458_g4 , S558_g4 , S658_g4 , S758_g4 , S858_g4 );
				float3 ase_worldTangent = i.ase_texcoord2.xyz;
				float3 ase_worldNormal = i.ase_texcoord3.xyz;
				float3 ase_worldBitangent = i.ase_texcoord4.xyz;
				float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
				float3 tangentToWorldDir68_g4 = mul( ase_tangentToWorldFast, localCombineSamplesSmooth58_g4 );
				float3 temp_output_2_0 = tangentToWorldDir68_g4;
				float temp_output_91_0_g3 = _strength;
				float Strength58_g3 = temp_output_91_0_g3;
				float localCalculateUVsSmooth46_g3 = ( 0.0 );
				float2 temp_output_85_0_g3 = uv_MainTex;
				float2 UV46_g3 = temp_output_85_0_g3;
				float4 TexelSize46_g3 = _MainTex_TexelSize;
				float2 UV046_g3 = float2( 0,0 );
				float2 UV146_g3 = float2( 0,0 );
				float2 UV246_g3 = float2( 0,0 );
				float2 UV346_g3 = float2( 0,0 );
				float2 UV446_g3 = float2( 0,0 );
				float2 UV546_g3 = float2( 0,0 );
				float2 UV646_g3 = float2( 0,0 );
				float2 UV746_g3 = float2( 0,0 );
				float2 UV846_g3 = float2( 0,0 );
				CalculateUVsSmooth46_g3( UV46_g3 , TexelSize46_g3 , UV046_g3 , UV146_g3 , UV246_g3 , UV346_g3 , UV446_g3 , UV546_g3 , UV646_g3 , UV746_g3 , UV846_g3 );
				float4 break140_g3 = tex2D( _MainTex, UV046_g3 );
				float S058_g3 = break140_g3.r;
				float4 break142_g3 = tex2D( _MainTex, UV146_g3 );
				float S158_g3 = break142_g3.r;
				float4 break146_g3 = tex2D( _MainTex, UV246_g3 );
				float S258_g3 = break146_g3.r;
				float4 break148_g3 = tex2D( _MainTex, UV346_g3 );
				float S358_g3 = break148_g3.r;
				float4 break150_g3 = tex2D( _MainTex, UV446_g3 );
				float S458_g3 = break150_g3.r;
				float4 break152_g3 = tex2D( _MainTex, UV546_g3 );
				float S558_g3 = break152_g3.r;
				float4 break154_g3 = tex2D( _MainTex, UV646_g3 );
				float S658_g3 = break154_g3.r;
				float4 break156_g3 = tex2D( _MainTex, UV746_g3 );
				float S758_g3 = break156_g3.r;
				float4 break158_g3 = tex2D( _MainTex, UV846_g3 );
				float S858_g3 = break158_g3.r;
				float3 localCombineSamplesSmooth58_g3 = CombineSamplesSmooth58_g3( Strength58_g3 , S058_g3 , S158_g3 , S258_g3 , S358_g3 , S458_g3 , S558_g3 , S658_g3 , S758_g3 , S858_g3 );
				float3 tangentToWorldDir68_g3 = mul( ase_tangentToWorldFast, localCombineSamplesSmooth58_g3 );
				float3 lerpResult18 = lerp( temp_output_2_0 , tangentToWorldDir68_g3 , _ColorInfluence);
				float3 break11 = ( ( lerpResult18 * float3( 0.5,0.5,0.5 ) ) + float3( 0.5,0.5,0.5 ) );
				float4 appendResult6 = (float4(break11.x , break11.y , break11.z , tex2D( _MainTex, uv_MainTex ).a));
				
				
				finalColor = appendResult6;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18801
545;702;1408;559;794.2416;189.0115;1.3;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;4;-799.9568,-26.05252;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;5;-678.1136,191.1136;Inherit;False;Property;_strength;strength;1;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-385.0057,326.0527;Inherit;False;Property;_ColorInfluence;ColorInfluence;2;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;15;-474.8322,135.31;Inherit;False;Normal From Texture;-1;;3;9728ee98a55193249b513caf9a0f1676;13,149,0,147,0,143,0,141,0,139,0,151,0,137,0,153,0,159,0,157,0,155,0,135,0,108,1;4;87;SAMPLER2D;0;False;85;FLOAT2;0,0;False;74;SAMPLERSTATE;0;False;91;FLOAT;4.23;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;2;-452.6735,-73.87688;Inherit;False;Normal From Texture;-1;;4;9728ee98a55193249b513caf9a0f1676;13,149,3,147,3,143,3,141,3,139,3,151,3,137,3,153,3,159,3,157,3,155,3,135,3,108,1;4;87;SAMPLER2D;0;False;85;FLOAT2;0,0;False;74;SAMPLERSTATE;0;False;91;FLOAT;1.5;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;18;-84.76177,209.3066;Inherit;False;3;0;FLOAT3;0.5,0.5,1;False;1;FLOAT3;0,0,1;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;103.9319,61.80764;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.5,0.5,0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;285.5864,-13.59237;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.5,0.5,0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;7;-564.0076,412.716;Inherit;True;Property;_TextureSample12;Texture Sample 12;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;11;198.0582,213.0429;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;6;355.4892,195.5593;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BlendNormalsNode;17;-69.73714,-39.40517;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;560.7564,-0.5525284;Float;False;True;-1;2;ASEMaterialInspector;100;1;Hidden/DepthToNormal;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;15;87;4;0
WireConnection;15;91;5;0
WireConnection;2;87;4;0
WireConnection;2;91;5;0
WireConnection;18;0;2;0
WireConnection;18;1;15;0
WireConnection;18;2;19;0
WireConnection;13;0;18;0
WireConnection;14;0;13;0
WireConnection;7;0;4;0
WireConnection;11;0;14;0
WireConnection;6;0;11;0
WireConnection;6;1;11;1
WireConnection;6;2;11;2
WireConnection;6;3;7;4
WireConnection;17;0;2;0
WireConnection;17;1;18;0
WireConnection;0;0;6;0
ASEEND*/
//CHKSM=DC9519456D53462F605E8333A580449592497621