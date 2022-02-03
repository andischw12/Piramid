// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ShockResize"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_magnitude("magnitude", Float) = 0
		_Float0("Float 0", Range( 0 , 1)) = 0
		_Float1("Float 1", Float) = 1
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
			uniform float _magnitude;
			float4 _MainTex_TexelSize;
			uniform float _Float1;
			uniform float _Float0;
			float4 ShockSample1( sampler2D Image, float shockMagnitude, float2 destSize, float4 ones, float2 inTex )
			{
				float3 inc = float3(1.0/destSize, 0.0); 
				// could be a uniform      
				float4 curCol = tex2D(Image, inTex);    
				float4 upCol = tex2D(Image, inTex + inc.zy);    
				float4 downCol = tex2D(Image, inTex - inc.zy);    
				float4 rightCol = tex2D(Image, inTex + inc.xz);    
				float4 leftCol = tex2D(Image, inTex - inc.xz);    
				float4 Convexity = 4.0 * curCol - rightCol - leftCol - upCol - downCol;    
				float2 diffusion = float2(dot((rightCol - leftCol) * Convexity, ones),  dot((upCol - downCol) * Convexity, ones));
				diffusion *= shockMagnitude/(length(diffusion) + 0.00001);   
				curCol += (diffusion.x > 0 ? diffusion.x * rightCol :-diffusion.x*leftCol) + (diffusion.y > 0 ? diffusion.y * upCol : -diffusion.y * downCol); 
				   return curCol/(1 + dot(abs(diffusion), ones.xy));  
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
				float4 tex2DNode7 = tex2D( _MainTex, uv_MainTex );
				sampler2D Image1 = _MainTex;
				float shockMagnitude1 = _magnitude;
				float2 appendResult16 = (float2(_MainTex_TexelSize.z , _MainTex_TexelSize.w));
				float2 destSize1 = ( appendResult16 * _Float1 );
				float4 ones1 = float4( 1,1,1,1 );
				float2 inTex1 = uv_MainTex;
				float4 localShockSample1 = ShockSample1( Image1 , shockMagnitude1 , destSize1 , ones1 , inTex1 );
				float4 lerpResult8 = lerp( tex2DNode7 , localShockSample1 , _Float0);
				float4 break32 = lerpResult8;
				float4 appendResult31 = (float4(break32.x , break32.y , break32.z , tex2DNode7.a));
				
				
				finalColor = appendResult31;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
771;399;2033;853;1212.806;598.4431;1.3;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1793.1,-27.2;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;6366cec5ad51a4d48a449c35c0ce31e1;f746f8722fd49f8458b3eb971910786c;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexelSizeNode;15;-1321.296,256.4121;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-558.3123,343.1243;Inherit;False;Property;_Float1;Float 1;4;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-596.9761,234.3494;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1663.224,566.1502;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-493.4,85.40002;Inherit;False;Property;_magnitude;magnitude;2;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-393.3123,245.1243;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CustomExpressionNode;1;-216.1,-23.8;Inherit;False;float3 inc = float3(1.0/destSize, 0.0)@ $// could be a uniform      $float4 curCol = tex2D(Image, inTex)@    $float4 upCol = tex2D(Image, inTex + inc.zy)@    $float4 downCol = tex2D(Image, inTex - inc.zy)@    $float4 rightCol = tex2D(Image, inTex + inc.xz)@    $float4 leftCol = tex2D(Image, inTex - inc.xz)@    $float4 Convexity = 4.0 * curCol - rightCol - leftCol - upCol - downCol@    $float2 diffusion = float2(dot((rightCol - leftCol) * Convexity, ones),  dot((upCol - downCol) * Convexity, ones))@$diffusion *= shockMagnitude/(length(diffusion) + 0.00001)@   $curCol += (diffusion.x > 0 ? diffusion.x * rightCol :-diffusion.x*leftCol) + (diffusion.y > 0 ? diffusion.y * upCol : -diffusion.y * downCol)@ $   return curCol/(1 + dot(abs(diffusion), ones.xy))@  ;4;False;5;True;Image;SAMPLER2D;;In;;Inherit;False;True;shockMagnitude;FLOAT;0;In;;Inherit;False;True;destSize;FLOAT2;512,512;In;;Inherit;False;True;ones;FLOAT4;1,1,1,1;In;;Inherit;False;True;inTex;FLOAT2;0,0;In;;Inherit;False;ShockSample;True;False;0;5;0;SAMPLER2D;;False;1;FLOAT;0;False;2;FLOAT2;512,512;False;3;FLOAT4;1,1,1,1;False;4;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-191.3,217.4;Inherit;False;Property;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-217.5,-307.2;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;8;122.5,-21;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;32;386.701,-143.3315;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-110.0038,485.2659;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;5;-559.5,722;Inherit;False;Property;_DestSize;DestSize;1;0;Create;True;0;0;0;False;0;False;0,0;2048,2048;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-653.9761,396.3494;Inherit;False;2;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GrabScreenPosition;18;-2049.715,608.0667;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;17;-818.9761,535.3494;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DdxOpNode;12;-1020.9,634.2;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;27;-1695.094,878.1106;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DdyOpNode;13;-1007.1,729.3987;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexelSizeNode;10;-757.9,-418.8001;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenParams;19;-2196.633,910.0237;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;31;473.701,88.66846;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;30;697,-112;Float;False;True;-1;2;ASEMaterialInspector;100;1;ShockResize;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;15;0;3;0
WireConnection;16;0;15;3
WireConnection;16;1;15;4
WireConnection;4;2;3;0
WireConnection;28;0;16;0
WireConnection;28;1;29;0
WireConnection;1;0;3;0
WireConnection;1;1;6;0
WireConnection;1;2;28;0
WireConnection;1;4;4;0
WireConnection;7;0;3;0
WireConnection;8;0;7;0
WireConnection;8;1;1;0
WireConnection;8;2;9;0
WireConnection;32;0;8;0
WireConnection;20;0;14;0
WireConnection;14;0;4;0
WireConnection;14;1;17;0
WireConnection;17;0;12;0
WireConnection;17;1;13;0
WireConnection;12;0;27;1
WireConnection;13;0;27;2
WireConnection;10;0;3;0
WireConnection;31;0;32;0
WireConnection;31;1;32;1
WireConnection;31;2;32;2
WireConnection;31;3;7;4
WireConnection;30;0;31;0
ASEEND*/
//CHKSM=E1160E2D711B58AB4C2A4D7FD2B2DC22219ABA0D