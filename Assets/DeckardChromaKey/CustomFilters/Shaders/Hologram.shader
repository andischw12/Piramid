// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DeckardChromKey/Hologram"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Color("Color", Color) = (0.4198113,0.877045,1,1)
		_HolographicLine("HolographicLine", 2D) = "white" {}
		_scanlineIntensity("scanlineIntensity", Float) = 0
		_CRTNoise("CRTNoise", 2D) = "white" {}
		_Intensity("Intensity", Float) = 2
		_noiseIntensity("noiseIntensity", Range( 0 , 1)) = 0.3
		_slit("slit", Float) = 0
		_blendWithOriginal("blendWithOriginal", Range( 0 , 1)) = 0
		_Transparent("Transparent", Range( 0 , 1)) = 1
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
			#include "UnityShaderVariables.cginc"


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
			uniform sampler2D _HolographicLine;
			uniform float4 _HolographicLine_ST;
			uniform float _slit;
			uniform float _scanlineIntensity;
			uniform float4 _Color;
			uniform sampler2D _CRTNoise;
			uniform float4 _CRTNoise_ST;
			uniform float _noiseIntensity;
			uniform float _Intensity;
			uniform float _Transparent;
			uniform float _blendWithOriginal;
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
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
				float2 uv_HolographicLine = i.ase_texcoord1.xy * _HolographicLine_ST.xy + _HolographicLine_ST.zw;
				float mulTime15 = _Time.y * 12.86;
				float2 temp_cast_1 = (mulTime15).xx;
				float simplePerlin2D28 = snoise( temp_cast_1*0.54 );
				simplePerlin2D28 = simplePerlin2D28*0.5 + 0.5;
				float2 temp_cast_2 = (_SinTime.w).xx;
				float simplePerlin2D31 = snoise( temp_cast_2*-2.2 );
				simplePerlin2D31 = simplePerlin2D31*0.5 + 0.5;
				float4 tex2DNode6 = tex2D( _HolographicLine, ( ( uv_HolographicLine + simplePerlin2D28 ) * ( simplePerlin2D31 * 2.0 ) ) );
				float2 appendResult35 = (float2(( tex2DNode6.g * _slit * _SinTime.w ) , 0.0));
				float4 tex2DNode2 = tex2D( _MainTex, ( uv_MainTex + appendResult35 ) );
				float2 uv_CRTNoise = i.ase_texcoord1.xy * _CRTNoise_ST.xy + _CRTNoise_ST.zw;
				float4 lerpResult23 = lerp( tex2D( _CRTNoise, ( uv_CRTNoise + mulTime15 ) ) , float4( 1,1,1,1 ) , _noiseIntensity);
				float4 appendResult9 = (float4((( ( tex2DNode2 + ( tex2DNode6.r * saturate( _SinTime.w ) * _scanlineIntensity ) ) * float4( 1,1,1,0 ) * _Color * saturate( lerpResult23 ) * _Intensity )).rgb , ( _Transparent * tex2DNode2.a )));
				float4 lerpResult39 = lerp( tex2D( _MainTex, uv_MainTex ) , appendResult9 , _blendWithOriginal);
				
				
				finalColor = lerpResult39;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18809
453;193;1901;1148;1513.224;810.0407;1.6;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;15;-726.5,-466;Inherit;False;1;0;FLOAT;12.86;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;24;-712.2998,834.8001;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-866.4,451;Inherit;False;0;6;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;31;-841.3275,669.225;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;-2.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;28;-643.7276,571.725;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;0.54;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-433.1276,483.3251;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-518.9274,675.7251;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-340.8273,599.0251;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;6;-119,325.0002;Inherit;True;Property;_HolographicLine;HolographicLine;3;0;Create;True;0;0;0;False;0;False;-1;fd8a456f34124f747b29774327ba0dd9;fd8a456f34124f747b29774327ba0dd9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-855.6271,173.9249;Inherit;False;Property;_slit;slit;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-713.5,-634;Inherit;False;0;13;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-573.5269,151.825;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-1032.427,-148.4751;Inherit;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;35;-752.9272,29.62494;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-465.5,-495;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;13;-315.5,-557;Inherit;True;Property;_CRTNoise;CRTNoise;5;0;Create;True;0;0;0;False;0;False;-1;e74079df3013ed74bbe96162536a676a;e74079df3013ed74bbe96162536a676a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-457.5,-301;Inherit;False;Property;_noiseIntensity;noiseIntensity;7;0;Create;True;0;0;0;False;0;False;0.3;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;29;141.4727,491.1251;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;318.5,510;Inherit;False;Property;_scanlineIntensity;scanlineIntensity;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-723.0272,-97.77502;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;349.5,285;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-548.2002,-167.7;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;23;174.5,-509;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-178.5,-133;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-0.5,220;Inherit;False;Property;_Intensity;Intensity;6;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-270.5,245;Inherit;False;Property;_Color;Color;2;0;Create;True;0;0;0;False;0;False;0.4198113,0.877045,1,1;0.4198113,0.877045,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;22;-8.5,-230;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;18.5,-131;Inherit;False;5;5;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;41;544.376,298.7592;Inherit;False;Property;_Transparent;Transparent;10;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;446.7761,113.1593;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;8;299.5,-31.99999;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;38;475.5762,-301.2408;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;2;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;9;651.1,142;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;40;646.7762,421.9591;Inherit;False;Property;_blendWithOriginal;blendWithOriginal;9;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-196.5,125;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-1035.2,251.8;Inherit;True;Property;_HolographicScanline;HolographicScanline;1;0;Create;True;0;0;0;False;0;False;-1;af3827aa7b60e6f4ba38e85d84c16229;af3827aa7b60e6f4ba38e85d84c16229;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;39;891.5763,117.9591;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-264.1277,812.2251;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-2.54;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;20;186.5,-218;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;1069.6,-104.8;Float;False;True;-1;2;ASEMaterialInspector;100;1;DeckardChromKey/Hologram;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;31;0;24;4
WireConnection;28;0;15;0
WireConnection;26;0;25;0
WireConnection;26;1;28;0
WireConnection;32;0;31;0
WireConnection;30;0;26;0
WireConnection;30;1;32;0
WireConnection;6;1;30;0
WireConnection;36;0;6;2
WireConnection;36;1;37;0
WireConnection;36;2;24;4
WireConnection;35;0;36;0
WireConnection;17;0;16;0
WireConnection;17;1;15;0
WireConnection;13;1;17;0
WireConnection;29;0;24;4
WireConnection;34;0;33;0
WireConnection;34;1;35;0
WireConnection;10;0;6;1
WireConnection;10;1;29;0
WireConnection;10;2;11;0
WireConnection;2;1;34;0
WireConnection;23;0;13;0
WireConnection;23;2;19;0
WireConnection;7;0;2;0
WireConnection;7;1;10;0
WireConnection;22;0;23;0
WireConnection;4;0;7;0
WireConnection;4;2;5;0
WireConnection;4;3;22;0
WireConnection;4;4;14;0
WireConnection;42;0;41;0
WireConnection;42;1;2;4
WireConnection;8;0;4;0
WireConnection;9;0;8;0
WireConnection;9;3;42;0
WireConnection;12;0;3;1
WireConnection;12;1;2;4
WireConnection;39;0;38;0
WireConnection;39;1;9;0
WireConnection;39;2;40;0
WireConnection;27;0;24;4
WireConnection;1;0;39;0
ASEEND*/
//CHKSM=F517C4CC0A8FC4036D6959C0A21E82A2DA5CF5C4