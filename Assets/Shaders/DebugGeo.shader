//Shader written by Josh Wilson using the help of two blogs
//Explains the basics of barymetric coordinates and how to make a wireframe shader:
//https://catlikecoding.com/unity/tutorials/advanced-rendering/flat-and-wireframe-shading/ 
//Show genius technique for only drawing quads:
//http://www.shaderslab.com/demo-94---wireframe-without-diagonal.html
Shader "Unlit/DebugGeo"
{
    Properties
    {
        _Color ("Color", color) = (1.0, 1.0, 1.0, 1.0)
        [PowerSlider(3.0)]
        _FrameWidth ("Wireframe width", Range(0.0, 2.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}
        LOD 100
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.0
            #pragma geometry geo;

            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float4 worldPos : SV_POSITION;
            };

            //Information for the geomtry portion of the shader
            struct g2f {
                float4 pos : SV_POSITION;
                float3 bary : TEXCOORD0;
            };
            

            [maxvertexcount(3)]
            void geo(triangle v2g IN[3], inout TriangleStream<g2f> stream) {
                //Define a vector3 for our float parameter
                float3 param = float3(0.0, 0.0, 0.0);
 
                //Basically get the edges of our given triangle
                float Edge1 = length(IN[0].worldPos - IN[1].worldPos);
                float Edge2 = length(IN[1].worldPos - IN[2].worldPos);
                float Edge3 = length(IN[2].worldPos - IN[0].worldPos);
               
                //What this does is essentially remove the diaganol tri from a given quad
                if(Edge1 > Edge2 && Edge1 > Edge3){
                    param.y = 1.0;
                }
                else if (Edge2 > Edge3 && Edge2 > Edge1){
                    param.x = 1.0;
                }
                else{
                    param.z = 1.0;
                }

                //Gets the barymetric coordinates and appends the stream so we have them properly
                g2f o;
                o.pos = mul(UNITY_MATRIX_VP, IN[0].worldPos);
                o.bary = float3(1.0, 0.0, 0.0) + param;
                stream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[1].worldPos);
                o.bary = float3(0.0, 0.0, 1.0) + param;
                stream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[2].worldPos);
                o.bary = float3(0.0, 1.0, 0.0) + param;
                stream.Append(o);
            }
 


            float4 _MainTex_ST;
            fixed4 _Color;
            float _FrameWidth;

            v2g vert (appdata v)
            {
                v2g o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            
            fixed4 frag (g2f i) : SV_Target
            {
                fixed4 col = _Color;

                //Use screen space to make it so the width scales with the screen
                float3 bary_grad = fwidth(i.bary * _FrameWidth); // change over 1 pixel
                float3 bary = i.bary / bary_grad; // scale barycentrics
 
                float edge = smoothstep(0.0, 1.0, min(bary.x, min(bary.y, bary.z)));
                if(edge > 0.5){
                    discard;
                }
 
                return col;
                
            }
            ENDCG
        }
    }
}
