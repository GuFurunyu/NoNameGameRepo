Shader "Unlit/OrthoFacingAndWhiteTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        // 相机朝向控制
        _MaxAngle ("Max Angle (degrees)", Range(0, 90)) = 30
        _AngleFalloff ("Angle Falloff", Range(0, 45)) = 10
        
        // 白色透明控制
        _WhiteThreshold ("White Threshold", Range(0, 1)) = 0.95
        _WhiteSoftness ("White Softness", Range(0, 0.3)) = 0.05
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float facingAlpha : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MaxAngle;
            float _AngleFalloff;
            float _WhiteThreshold;
            float _WhiteSoftness;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                // 1. 计算相机朝向透明度
                // 获取相机的世界空间前向方向
                float3 cameraForward = normalize(mul((float3x3)unity_CameraToWorld, float3(0, 0, 1)));
                
                // 计算物体的世界空间法线
                float3 worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                
                // 计算法线与相机前向方向的点积
                float dotProduct = dot(worldNormal, -cameraForward);
                
                // 将角度转换为点积阈值
                float maxAngleRad = radians(_MaxAngle);
                float maxAngleDot = cos(maxAngleRad);
                
                // 计算过渡范围
                float falloffAngleRad = radians(_AngleFalloff);
                float falloffStartDot = cos(maxAngleRad + falloffAngleRad);
                
                // 计算朝向系数
                if (dotProduct >= maxAngleDot)
                {
                    o.facingAlpha = 1.0;
                }
                else if (dotProduct >= falloffStartDot)
                {
                    // 平滑过渡
                    o.facingAlpha = 1.0 - (maxAngleDot - dotProduct) / (maxAngleDot - falloffStartDot);
                }
                else
                {
                    o.facingAlpha = 0.0;
                }
                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 如果朝向完全透明，直接丢弃片段
                if (i.facingAlpha <= 0)
                {
                    discard;
                    return fixed4(0, 0, 0, 0);
                }
                
                // 2. 读取纹理并计算白色透明度
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // 计算颜色亮度
                float brightness = (col.r + col.g + col.b) / 3.0;
                
                // 计算白色透明度
                float whiteAlpha = 1.0 - smoothstep(_WhiteThreshold - _WhiteSoftness, 
                                                   _WhiteThreshold + _WhiteSoftness, 
                                                   brightness);
                
                // 合并两个透明度
                float finalAlpha = i.facingAlpha * whiteAlpha;
                
                // 如果最终透明度为0，丢弃片段
                if (finalAlpha <= 0)
                {
                    discard;
                }
                
                return fixed4(col.rgb, finalAlpha);
            }
            ENDCG
        }
    }
}