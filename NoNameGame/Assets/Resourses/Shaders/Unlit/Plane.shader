Shader "Unlit/Plane"
{
    Properties
    {
        // 主颜色控制
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        
        // 描边控制 - 现在颜色和透明度分开
        _OutlineColor ("Outline Color", Color) = (1, 0, 0, 1)
        _OutlineAlpha ("Outline Alpha", Range(0, 1)) = 1
        _OutlineWidth ("Outline Width", Range(0.01, 2)) = 1
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }
        
        // 启用透明度混合
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
        // 第一遍：渲染背面作为描边
        Pass
        {
            Name "OUTLINE"
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert_outline
            #pragma fragment frag_outline
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            
            struct v2f_outline
            {
                float4 pos : SV_POSITION;
            };
            
            float _OutlineWidth;
            float4 _OutlineColor;
            float _OutlineAlpha;
            
            v2f_outline vert_outline (appdata v)
            {
                v2f_outline o;
                
                // 在视图空间计算扩展，避免不均匀缩放问题
                float4 pos = UnityObjectToClipPos(v.vertex);
                float3 normal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float2 offset = TransformViewToProjection(normal.xy);
                
                // 应用描边宽度
                pos.xy += offset * _OutlineWidth;
                o.pos = pos;
                
                return o;
            }
            
            fixed4 frag_outline (v2f_outline i) : SV_Target
            {
                // 使用独立的_OutlineAlpha控制描边透明度
                return fixed4(_OutlineColor.rgb, _OutlineAlpha);
            }
            ENDCG
        }
        
        // 第二遍：渲染物体主色
        Pass
        {
            Name "MAIN"
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert_main
            #pragma fragment frag_main
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
            };
            
            struct v2f_main
            {
                float4 pos : SV_POSITION;
            };
            
            float4 _MainColor;
            
            v2f_main vert_main (appdata v)
            {
                v2f_main o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            
            fixed4 frag_main (v2f_main i) : SV_Target
            {
                // 使用_MainColor的alpha通道控制主颜色的透明度
                return _MainColor;
            }
            ENDCG
        }
    }
}