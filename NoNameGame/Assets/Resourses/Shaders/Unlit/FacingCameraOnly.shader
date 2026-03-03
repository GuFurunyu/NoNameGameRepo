Shader "Unlit/FacingCameraOnly"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaxAngle ("Max Angle (degrees)", Range(0, 90)) = 30
        _AngleFalloff ("Angle Falloff", Range(0, 45)) = 10
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
                float facing : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MaxAngle;
            float _AngleFalloff;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                // 获取相机的世界空间前向方向（在正交相机下是恒定的）
                // UnityCameraToWorld._13_23_33 存储了相机前向方向
                float3 cameraForward = normalize(mul((float3x3)unity_CameraToWorld, float3(0, 0, 1)));
                
                // 计算物体的世界空间法线
                float3 worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                
                // 计算法线与相机前向方向的点积
                // 注意：相机看的是-z方向，所以实际是 -cameraForward
                // 但我们想要法线与相机朝向对齐，所以直接用cameraForward
                float dotProduct = dot(worldNormal, -cameraForward); // 使用负号因为相机看向-z
                
                // 将角度转换为点积阈值
                float maxAngleRad = radians(_MaxAngle);
                float maxAngleDot = cos(maxAngleRad);
                
                // 计算过渡范围
                float falloffAngleRad = radians(_AngleFalloff);
                float falloffStartDot = cos(maxAngleRad + falloffAngleRad);
                
                // 计算朝向系数
                if (dotProduct >= maxAngleDot)
                {
                    o.facing = 1.0;
                }
                else if (dotProduct >= falloffStartDot)
                {
                    // 平滑过渡
                    o.facing = 1.0 - (maxAngleDot - dotProduct) / (maxAngleDot - falloffStartDot);
                }
                else
                {
                    o.facing = 0.0;
                }
                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                if (i.facing <= 0)
                {
                    discard;
                    return fixed4(0, 0, 0, 0);
                }
                
                fixed4 col = tex2D(_MainTex, i.uv);
                return fixed4(col.rgb, i.facing);
            }
            ENDCG
        }
    }
}