Shader "Unlit/GridLines"
{
    Properties
    {
        // 主颜色
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        
        // 网格参数
        _GridSize ("Grid Size", Range(0.1, 10)) = 1
        _LineWidth ("Line Width", Range(0.001, 0.1)) = 0.01
        _LineColor ("Line Color", Color) = (0, 0, 0, 1)
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "Queue"="Geometry"
        }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };
            
            float4 _MainColor;
            float _GridSize;
            float _LineWidth;
            float4 _LineColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                // 获取世界坐标
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 将世界坐标映射到网格空间
                float2 gridPos = i.worldPos.xz / _GridSize;
                
                // 计算到最近网格线的距离（使用fract得到小数部分）
                float2 gridFrac = frac(gridPos);
                
                // 计算每个方向到最近网格线的距离
                float2 distToLine = min(gridFrac, 1.0 - gridFrac);
                
                // 归一化距离（考虑线宽）
                float2 lineFactor = smoothstep(_LineWidth, 0.0, distToLine);
                
                // 合并水平和垂直线
                float gridFactor = max(lineFactor.x, lineFactor.y);
                
                // 混合颜色：网格线颜色 + 主颜色
                fixed4 finalColor = lerp(_MainColor, _LineColor, gridFactor);
                
                return finalColor;
            }
            ENDCG
        }
    }
}