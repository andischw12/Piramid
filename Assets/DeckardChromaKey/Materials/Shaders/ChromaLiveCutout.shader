// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ChromaLiveCutout"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.98
		_MainTex("_MainTex", 2D) = "white" {}
		_treshold1("treshold", Float) = 0
		_smooth("smooth", Float) = 0
		_Power("Power", Float) = 0
		_Border1("Border", Range( 0 , 0.01)) = 0
		_MaskPower1("MaskPower", Float) = 0
		_leftCut1("leftCut", Range( 0 , 1)) = 0
		_despillChroma("despillChroma", Float) = 0
		_ultimatte("ultimatte", 2D) = "white" {}
		_Float0("Float 0", Float) = 1
		_Sharpen("Sharpen", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _Power;
		uniform float _Border1;
		uniform float _Sharpen;
		uniform float _despillChroma;
		uniform float _Float0;
		uniform float _treshold1;
		uniform float _smooth;
		uniform sampler2D _ultimatte;
		uniform float4 _ultimatte_ST;
		uniform float _MaskPower1;
		uniform float _leftCut1;
		uniform float _Cutoff = 0.98;


		float4 Despill( float4 result , float _Despill , float _DespillLuminanceAdd , float4 color )
		{
			float v = (2*result.b+result.r)/4;
			                if(result.g > v) result.g = lerp(result.g, v, _Despill);
			                float4 dif = (color - result);
			                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b;
			                result += lerp(0, desaturatedDif, _DespillLuminanceAdd);
			return result;
		}


		float3 temperature_Deckard1_g3( float3 In , float Temperature , float Tint )
		{
			 float t1 = Temperature * 10 / 6;
			    float t2 = Tint * 10 / 6;
			    // Get the CIE xy chromaticity of the reference white point.
			    // Note: 0.31271 = x value on the D65 white point
			    float x = 0.31271 - t1 * (t1 < 0 ? 0.1 : 0.05);
			    float standardIlluminantY = 2.87 * x - 3 * x * x - 0.27509507;
			    float y = standardIlluminantY + t2 * 0.05;
			    // Calculate the coefficients in the LMS space.
			    float3 w1 = float3(0.949237, 1.03542, 1.08728); // D65 white point
			    // CIExyToLMS
			    float Y = 1;
			    float X = Y * x / y;
			    float Z = Y * (1 - x - y) / y;
			    float L = (0.7328 * X + 0.4296 * Y - 0.1624 * Z);
			    float M = -0.7036 * X + 1.6975 * Y + 0.0061 * Z;
			    float S = 0.0030 * X + 0.0136 * Y + 0.9834 * Z;
			    float3 w2 = float3(L, M, S);
			    float3 balance = float3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);
			    float3x3 LIN_2_LMS_MAT = {
			        3.90405e-1, 5.49941e-1, 8.92632e-3,
			        7.08416e-2, 9.63172e-1, 1.35775e-3,
			        2.31082e-2, 1.28021e-1, 9.36245e-1
			    };
			    float3x3 LMS_2_LIN_MAT = {
			        2.85847e+0, -1.62879e+0, -2.48910e-2,
			        -2.10182e-1,  1.15820e+0,  3.24281e-4,
			        -4.18120e-2, -1.18169e-1,  1.06867e+0
			    };
			    float3 lms = mul(LIN_2_LMS_MAT, In);
			    lms *= balance;
			    return mul(LMS_2_LIN_MAT, lms);
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv0_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode203 = tex2D( _MainTex, uv0_MainTex );
			float4 temp_cast_0 = (_Power).xxxx;
			float4 temp_output_208_0 = pow( tex2DNode203 , temp_cast_0 );
			float4 tex2DNode201 = tex2D( _MainTex, ( uv0_MainTex + ( float2( -1,0 ) * _Border1 ) ) );
			float4 tex2DNode199 = tex2D( _MainTex, ( uv0_MainTex + ( float2( 0,1 ) * _Border1 ) ) );
			float4 tex2DNode200 = tex2D( _MainTex, ( uv0_MainTex + ( float2( 0,-1 ) * _Border1 ) ) );
			float4 tex2DNode202 = tex2D( _MainTex, ( uv0_MainTex + ( float2( 1,0 ) * _Border1 ) ) );
			float4 temp_cast_1 = (_Power).xxxx;
			float4 temp_output_228_0 = ( temp_output_208_0 + saturate( ( ( temp_output_208_0 - pow( ( ( tex2DNode201 + tex2DNode199 + tex2DNode200 + tex2DNode202 ) * 0.25 ) , temp_cast_1 ) ) * _Sharpen ) ) );
			float4 result234 = temp_output_228_0;
			float _Despill234 = _despillChroma;
			float _DespillLuminanceAdd234 = 0.0;
			float4 color234 = temp_output_228_0;
			float4 localDespill234 = Despill( result234 , _Despill234 , _DespillLuminanceAdd234 , color234 );
			float3 In1_g3 = localDespill234.xyz;
			float Temperature1_g3 = _Float0;
			float Tint1_g3 = 0.0;
			float3 localtemperature_Deckard1_g3 = temperature_Deckard1_g3( In1_g3 , Temperature1_g3 , Tint1_g3 );
			float3 temp_output_240_0 = localtemperature_Deckard1_g3;
			o.Emission = saturate( temp_output_240_0 );
			o.Alpha = 1;
			float temp_output_219_0 = ( _treshold1 + _smooth );
			float2 uv_ultimatte = i.uv_texcoord * _ultimatte_ST.xy + _ultimatte_ST.zw;
			float4 tex2DNode212 = tex2D( _ultimatte, uv_ultimatte );
			float smoothstepResult232 = smoothstep( _treshold1 , temp_output_219_0 , distance( tex2DNode201 , tex2DNode212 ));
			float smoothstepResult230 = smoothstep( _treshold1 , temp_output_219_0 , distance( tex2DNode203 , tex2DNode212 ));
			float smoothstepResult227 = smoothstep( _treshold1 , temp_output_219_0 , distance( tex2DNode200 , tex2DNode212 ));
			float smoothstepResult231 = smoothstep( _treshold1 , temp_output_219_0 , distance( tex2DNode202 , tex2DNode212 ));
			float lerpResult253 = lerp( pow( ( smoothstepResult232 * smoothstepResult230 * smoothstepResult227 * smoothstepResult231 ) , _MaskPower1 ) , 0.0 , step( uv0_MainTex.x , _leftCut1 ));
			clip( lerpResult253 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17701
1158;295;1693;911;1191.913;904.4201;1;True;True
Node;AmplifyShaderEditor.Vector2Node;186;-2687.308,1077.832;Inherit;False;Constant;_Vector3;Vector 2;20;0;Create;True;0;0;False;0;1,0;0,0.002;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;184;-2495.308,-90.16782;Inherit;True;Property;_MainTex;_MainTex;5;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Vector2Node;188;-2591.308,437.8322;Inherit;False;Constant;_Vector1;Vector 0;17;0;Create;True;0;0;False;0;0,1;-0.002,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;185;-2687.308,757.8322;Inherit;False;Constant;_Vector2;Vector 1;18;0;Create;True;0;0;False;0;0,-1;0.002,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;187;-3119.308,501.8322;Inherit;False;Property;_Border1;Border;17;0;Create;True;0;0;False;0;0;4E-05;0;0.01;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;189;-2655.308,1381.832;Inherit;False;Constant;_Vector4;Vector 3;19;0;Create;True;0;0;False;0;-1,0;0,-0.002;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;190;-2703.308,197.8322;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;-2367.308,469.8322;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;191;-2431.308,757.8322;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;-2399.308,1381.832;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;193;-2447.308,1077.832;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;195;-2207.308,1269.832;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;196;-2239.308,965.8322;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;198;-2239.308,325.8322;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;197;-2223.308,645.8322;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;199;-2095.308,213.8322;Inherit;True;Property;_TextureSample2;Texture Sample 1;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;202;-2047.308,949.8322;Inherit;True;Property;_TextureSample4;Texture Sample 3;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;201;-2015.308,1253.832;Inherit;True;Property;_TextureSample5;Texture Sample 4;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;200;-2063.308,613.8322;Inherit;True;Property;_TextureSample3;Texture Sample 2;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;205;-623.3077,917.8322;Inherit;False;Constant;_Float8;Float 7;26;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;206;-1727.308,-250.1678;Inherit;False;Property;_Power;Power;13;0;Create;True;0;0;False;0;0;2.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;203;-2079.308,-58.16782;Inherit;True;Property;_TextureSample1;Texture Sample 0;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;204;-351.3077,709.8322;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;207;-127.3077,741.8322;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;208;-1519.308,-154.1678;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1.24;False;1;COLOR;0
Node;AmplifyShaderEditor.RelayNode;209;1056.693,37.83219;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;210;80.69231,629.8322;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1.24;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;211;912.6926,373.8322;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;212;-191.3077,-794.1678;Inherit;True;Property;_ultimatte;ultimatte;23;0;Create;True;0;0;False;0;-1;de95aa1d908b6fa469321aba44d5eeb7;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;213;896.6926,501.8322;Inherit;False;Property;_Sharpen;Sharpen;25;0;Create;True;0;0;False;0;0;1.52;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;218;-1471.308,-570.1678;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;1168.693,293.8322;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;214;-1263.308,21.83218;Inherit;False;Property;_treshold1;treshold;9;0;Create;True;0;0;False;0;0;-0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;216;-1223.108,146.4322;Inherit;False;Property;_smooth;smooth;11;0;Create;True;0;0;False;0;0;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;224;-1087.308,-58.16782;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;219;-1023.308,101.8322;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;225;-1359.308,1317.832;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;220;-1391.308,1013.832;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;222;1344.693,325.8322;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DistanceOpNode;223;-1375.308,693.8322;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;230;-815.3078,-10.16782;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;231;-991.3076,1077.832;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;228;1424.693,133.8322;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;227;-975.3076,741.8322;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;232;-1007.308,1365.832;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;233;1648.693,389.8322;Inherit;False;Property;_despillChroma;despillChroma;21;0;Create;True;0;0;False;0;0;1.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;235;2144.693,325.8322;Inherit;False;Property;_Float0;Float 0;24;0;Create;True;0;0;False;0;1;0.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;238;1184.693,-282.1678;Inherit;False;Property;_leftCut1;leftCut;20;0;Create;True;0;0;False;0;0;0.533;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;237;-367.3077,117.8322;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;236;-31.30769,-58.16782;Inherit;False;Property;_MaskPower1;MaskPower;18;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;234;1840.693,165.8322;Inherit;False;float v = (2*result.b+result.r)/4@$                if(result.g > v) result.g = lerp(result.g, v, _Despill)@$                float4 dif = (color - result)@$                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b@$                result += lerp(0, desaturatedDif, _DespillLuminanceAdd)@$return result@;4;False;4;True;result;FLOAT4;0,0,0,0;In;;Float;False;True;_Despill;FLOAT;0;In;;Float;False;True;_DespillLuminanceAdd;FLOAT;0;In;;Float;False;True;color;FLOAT4;0,0,0,0;In;;Float;False;Despill;False;True;0;4;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StepOpNode;241;1488.693,-170.1678;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;240;2416.692,133.8322;Inherit;False;ColorTemperature;1;;3;fccce2e41bca18a41863dccc23edb3ef;0;3;2;FLOAT3;0,0,0;False;5;FLOAT;0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;239;208.6928,-138.1678;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;229;1775.614,595.052;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;258;-95.30769,-186.1678;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;271;-495.3078,341.8322;Inherit;False;Constant;_Float6;Float 5;16;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;273;48.69231,-346.1678;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;274;2608.692,-42.16782;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;269;64.69231,389.8322;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1.24;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;260;1168.693,501.8322;Inherit;False;Property;_sharpenSmooth1;sharpenSmooth;27;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;247;464.6926,-410.1678;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;270;480.6926,37.83219;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;226;1520.693,837.8322;Inherit;False;Property;_LuminanceRemapping;LuminanceRemapping;28;0;Create;False;0;0;False;0;0,0,0,0;0,1,-0.41,3.55;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;264;16.69231,229.8322;Inherit;False;Property;_smoothDefringe;smoothDefringe;12;0;Create;True;0;0;False;0;0;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;259;-15.30769,117.8322;Inherit;False;Property;_tresholdDefringe;tresholdDefringe;10;0;Create;True;0;0;False;0;0;-2.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;244;-351.3077,-138.1678;Inherit;False;Property;_Power2;Power2;16;0;Create;False;0;0;False;0;0;469.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;243;-1343.308,389.8322;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;246;-783.3078,341.8322;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;2448.692,-458.1678;Inherit;False;Property;_Smoothness1;Smoothness;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;221;1456.693,629.8322;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexelSizeNode;245;-2927.308,277.8322;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;251;512.6926,-298.1678;Inherit;False;Property;_Color1;Color 0;19;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;276;208.6928,-458.1678;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;215;1168.693,629.8322;Inherit;False;Global;_GrabScreen1;Grab Screen 0;22;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;255;-1567.308,213.8322;Inherit;False;Property;_Float3;Float 1;6;0;Create;True;0;0;False;0;0;0.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;254;1296.693,-778.1678;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;256;-1887.308,-666.1678;Inherit;False;Property;_KEyColor1;KEyColor;4;0;Create;True;0;0;False;0;0,0,0,0;0,1,0.09895801,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RGBToHSVNode;257;752.6926,-298.1678;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;249;2464.692,-554.1678;Inherit;False;Property;_MEtalic1;MEtalic;15;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;278;3408.692,-106.1678;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;1,1,1;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;242;2832.692,165.8322;Inherit;False;Property;_NormalStrenght;NormalStrenght;26;0;Create;True;0;0;False;0;0;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;275;-1327.308,229.8322;Inherit;False;Property;_Float5;Float 2;8;0;Create;True;0;0;False;0;0;0.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;267;832.6926,-458.1678;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;253;1664.693,-282.1678;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;252;736.6926,197.8322;Inherit;False;Property;_DespillIlluminance1;DespillIlluminance;22;0;Create;True;0;0;False;0;0;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;263;2064.693,-650.1678;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.4339623,0.4339623,0.4339623,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RGBToHSVNode;265;656.6926,-666.1678;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;266;1008.693,-330.1678;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;272;224.6928,181.8322;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;277;1136.693,-650.1678;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;268;2768.692,21.83218;Inherit;False;Normal From Height;-1;;4;1942fe2c5f1a1f94881a33d532e4afeb;0;1;20;FLOAT;0;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;261;3072.692,21.83218;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;262;-1551.308,133.8322;Inherit;False;Property;_Float4;Float 0;7;0;Create;True;0;0;False;0;0;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4229.733,-492.7066;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ChromaLiveCutout;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.98;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;190;2;184;0
WireConnection;192;0;188;0
WireConnection;192;1;187;0
WireConnection;191;0;185;0
WireConnection;191;1;187;0
WireConnection;194;0;189;0
WireConnection;194;1;187;0
WireConnection;193;0;186;0
WireConnection;193;1;187;0
WireConnection;195;0;190;0
WireConnection;195;1;194;0
WireConnection;196;0;190;0
WireConnection;196;1;193;0
WireConnection;198;0;190;0
WireConnection;198;1;192;0
WireConnection;197;0;190;0
WireConnection;197;1;191;0
WireConnection;199;0;184;0
WireConnection;199;1;198;0
WireConnection;202;0;184;0
WireConnection;202;1;196;0
WireConnection;201;0;184;0
WireConnection;201;1;195;0
WireConnection;200;0;184;0
WireConnection;200;1;197;0
WireConnection;203;0;184;0
WireConnection;203;1;190;0
WireConnection;204;0;201;0
WireConnection;204;1;199;0
WireConnection;204;2;200;0
WireConnection;204;3;202;0
WireConnection;207;0;204;0
WireConnection;207;1;205;0
WireConnection;208;0;203;0
WireConnection;208;1;206;0
WireConnection;209;0;208;0
WireConnection;210;0;207;0
WireConnection;210;1;206;0
WireConnection;211;0;209;0
WireConnection;211;1;210;0
WireConnection;218;0;212;0
WireConnection;217;0;211;0
WireConnection;217;1;213;0
WireConnection;224;0;203;0
WireConnection;224;1;218;0
WireConnection;219;0;214;0
WireConnection;219;1;216;0
WireConnection;225;0;201;0
WireConnection;225;1;218;0
WireConnection;220;0;202;0
WireConnection;220;1;218;0
WireConnection;222;0;217;0
WireConnection;223;0;200;0
WireConnection;223;1;218;0
WireConnection;230;0;224;0
WireConnection;230;1;214;0
WireConnection;230;2;219;0
WireConnection;231;0;220;0
WireConnection;231;1;214;0
WireConnection;231;2;219;0
WireConnection;228;0;209;0
WireConnection;228;1;222;0
WireConnection;227;0;223;0
WireConnection;227;1;214;0
WireConnection;227;2;219;0
WireConnection;232;0;225;0
WireConnection;232;1;214;0
WireConnection;232;2;219;0
WireConnection;237;0;232;0
WireConnection;237;1;230;0
WireConnection;237;2;227;0
WireConnection;237;3;231;0
WireConnection;234;0;228;0
WireConnection;234;1;233;0
WireConnection;234;3;228;0
WireConnection;241;0;190;1
WireConnection;241;1;238;0
WireConnection;240;2;234;0
WireConnection;240;5;235;0
WireConnection;239;0;237;0
WireConnection;239;1;236;0
WireConnection;229;0;221;3
WireConnection;229;1;226;1
WireConnection;229;2;226;2
WireConnection;229;3;226;3
WireConnection;229;4;226;4
WireConnection;258;0;230;0
WireConnection;258;1;244;0
WireConnection;273;0;208;0
WireConnection;273;1;258;0
WireConnection;274;0;240;0
WireConnection;247;0;270;0
WireConnection;247;1;273;0
WireConnection;270;0;276;0
WireConnection;270;1;259;0
WireConnection;270;2;272;0
WireConnection;243;0;199;0
WireConnection;243;1;218;0
WireConnection;246;0;243;0
WireConnection;246;1;214;0
WireConnection;246;2;219;0
WireConnection;221;0;215;0
WireConnection;245;0;184;0
WireConnection;276;0;273;0
WireConnection;276;1;212;0
WireConnection;254;0;247;0
WireConnection;254;1;277;0
WireConnection;257;0;251;0
WireConnection;278;0;261;0
WireConnection;278;2;253;0
WireConnection;267;0;265;1
WireConnection;267;1;265;2
WireConnection;253;0;239;0
WireConnection;253;2;241;0
WireConnection;263;0;215;0
WireConnection;265;0;247;0
WireConnection;266;0;257;1
WireConnection;266;1;257;2
WireConnection;272;0;259;0
WireConnection;272;1;264;0
WireConnection;277;0;267;0
WireConnection;277;1;266;0
WireConnection;268;20;240;0
WireConnection;261;0;268;40
WireConnection;261;1;242;0
WireConnection;0;2;274;0
WireConnection;0;10;253;0
ASEEND*/
//CHKSM=36AA1F982ACCE1812534832E613F0CBB9390EBD5