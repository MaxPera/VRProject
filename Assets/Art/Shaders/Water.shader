Shader "Unlit/Water"
{
    Properties
    {
        _CausticsTex ("Caustics", 2D) = "white" {}
		[NoScaleOffset] _FlowMap ("Flow Map", 2D) = "black" {}

        _WaterLevel ("Water Level", Range(0, 1)) = 0

        [Header(Color)][Space]
        _CleanColor ("Clean Water Color", Color) = (1, 1, 1, 1)
        _DirtyColor ("Dirty Water Color", Color) = (1, 1, 1, 1)
        
        [Header(Water Properties)][Space]
        _MinGlossiness ("Minimum Glossiness", Range(0, 1)) = 0.8
        _WaterHeight ("Max Water Height", Range(0, 4)) = 3
        _FlowStrength ("Flow Strength", Range(-0.25, 0.25)) = 0
        _FlowSpeed ("Flow Speed", Range(0, 1)) = 0.5
        _Jumps ("Jumps per phase", Range(-0.25, 0.25)) = 0.25
    }
    SubShader
    {
        Tags 
        {  
            "RenderType"="Transparent"
            "Queue"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD0;
            };

            sampler2D _CausticsTex, _FlowMap;
            float4 _CausticsTex_ST, _CleanColor, _DirtyColor;
            float _MinGlossiness, _WaterHeight, _WaterLevel, _FlowStrength, _FlowSpeed, _Jumps;

            v2f vert (appdata v)
            {
                v.vertex.y += _WaterHeight * _WaterLevel;

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _CausticsTex);
                o.normal = v.normal;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.normal);
                float3 light = _WorldSpaceLightPos0.xyz;
                float3 lambert = saturate(dot(normal, light));
                float3 diffuseLight = lambert * _LightColor0.xyz;

                float3 viewDirection = normalize( _WorldSpaceCameraPos - i.worldPos);
                float3 halfVector = normalize(light + viewDirection);

                float3 specularLight = saturate(dot(halfVector, normal)) * (lambert > 0);
                float glossiness = _MinGlossiness + (1.0 * _WaterLevel) * (1.0 - _MinGlossiness);
                float specularExponent = exp2(glossiness * 11) + 2;
                specularLight = pow(specularLight, specularExponent) * glossiness;
                specularLight *= _LightColor0.xyz;
              
                fixed4 baseColor = _CleanColor * _WaterLevel + _DirtyColor * (1 - _WaterLevel);
                float transparency = _CleanColor.w * _WaterLevel + _DirtyColor.w * (1 - _WaterLevel);

                float2 flowVector = tex2D(_FlowMap, i.uv).rg * 2 - 1;
                flowVector *= _FlowStrength;
                float2 jump = float2(_Jumps, _Jumps);
                float noise = tex2D(_FlowMap, i.uv).a;
			    float time = _Time.y * _FlowSpeed + noise;
                float progress = frac(time);
	            float3 uvw;
	            uvw.xy = i.uv - flowVector * progress;
                uvw.xy += (time - progress) * jump;
	            uvw.z = 1 - abs(1 - 2 * progress);
			    fixed4 causticsA = tex2D(_CausticsTex, uvw.xy) * uvw.z;

                time = _Time.y * _FlowSpeed + noise;
                progress = frac(time + 0.5);
	            uvw.xy = i.uv - flowVector * progress + 0.5;
                uvw.xy += (time - progress) * jump;
	            uvw.z = 1 - abs(1 - 2 * progress);
			    fixed4 causticsB = tex2D(_CausticsTex, uvw.xy) * uvw.z;

                fixed4 caustics = (causticsA + causticsB) * _WaterLevel;

                return float4(diffuseLight * baseColor + specularLight + caustics, transparency);
            }
            ENDHLSL
        }
    }
}
