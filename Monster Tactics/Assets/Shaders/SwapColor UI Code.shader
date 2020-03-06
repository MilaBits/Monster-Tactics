Shader "Unlit Master"
{
    Properties
    {
        [NoScaleOffset]_MainTex("Main Sprite", 2D) = "white" {}
        [NoScaleOffset]Texture2D_68DC455A("Input Palette", 2D) = "white" {}
        [NoScaleOffset]Texture2D_E6F2163E("Output Palette", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent+0"
        }
        
        Pass
        {
            Name "Pass"
            Tags 
            { 
                // LightMode: <None>
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite Off
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma shader_feature _ _SAMPLE_GI
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            #define SHADERPASS_UNLIT
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            TEXTURE2D(Texture2D_68DC455A); SAMPLER(samplerTexture2D_68DC455A); float4 Texture2D_68DC455A_TexelSize;
            TEXTURE2D(Texture2D_E6F2163E); SAMPLER(samplerTexture2D_E6F2163E); float4 Texture2D_E6F2163E_TexelSize;
            SAMPLER(_SampleTexture2D_99E0D24D_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_ED41B5F1_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_4D7DEA49_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_2BD9BDBB_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_7C2A0C92_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_576FFED2_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_4066AB28_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_4F1215DC_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_1D37EC0D_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            
            void Unity_ReplaceColor_float(float3 In, float3 From, float3 To, float Range, out float3 Out, float Fuzziness)
            {
                float Distance = distance(From, In);
                Out = lerp(To, In, saturate((Distance - Range) / max(Fuzziness, 1e-5f)));
            }
        
            // Graph Vertex
            // GraphVertex: <None>
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
            };
            
            struct SurfaceDescription
            {
                float3 Color;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _SampleTexture2D_99E0D24D_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                float _SampleTexture2D_99E0D24D_R_4 = _SampleTexture2D_99E0D24D_RGBA_0.r;
                float _SampleTexture2D_99E0D24D_G_5 = _SampleTexture2D_99E0D24D_RGBA_0.g;
                float _SampleTexture2D_99E0D24D_B_6 = _SampleTexture2D_99E0D24D_RGBA_0.b;
                float _SampleTexture2D_99E0D24D_A_7 = _SampleTexture2D_99E0D24D_RGBA_0.a;
                float2 _Vector2_53071EBD_Out_0 = float2(0, 0);
                float4 _SampleTexture2D_ED41B5F1_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_68DC455A, samplerTexture2D_68DC455A, _Vector2_53071EBD_Out_0);
                float _SampleTexture2D_ED41B5F1_R_4 = _SampleTexture2D_ED41B5F1_RGBA_0.r;
                float _SampleTexture2D_ED41B5F1_G_5 = _SampleTexture2D_ED41B5F1_RGBA_0.g;
                float _SampleTexture2D_ED41B5F1_B_6 = _SampleTexture2D_ED41B5F1_RGBA_0.b;
                float _SampleTexture2D_ED41B5F1_A_7 = _SampleTexture2D_ED41B5F1_RGBA_0.a;
                float2 _Vector2_94563D84_Out_0 = float2(0, 0);
                float4 _SampleTexture2D_4D7DEA49_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_E6F2163E, samplerTexture2D_E6F2163E, _Vector2_94563D84_Out_0);
                float _SampleTexture2D_4D7DEA49_R_4 = _SampleTexture2D_4D7DEA49_RGBA_0.r;
                float _SampleTexture2D_4D7DEA49_G_5 = _SampleTexture2D_4D7DEA49_RGBA_0.g;
                float _SampleTexture2D_4D7DEA49_B_6 = _SampleTexture2D_4D7DEA49_RGBA_0.b;
                float _SampleTexture2D_4D7DEA49_A_7 = _SampleTexture2D_4D7DEA49_RGBA_0.a;
                float3 _ReplaceColor_20DE0EF9_Out_4;
                Unity_ReplaceColor_float((_SampleTexture2D_99E0D24D_RGBA_0.xyz), (_SampleTexture2D_ED41B5F1_RGBA_0.xyz), (_SampleTexture2D_4D7DEA49_RGBA_0.xyz), 0, _ReplaceColor_20DE0EF9_Out_4, 0);
                float2 _Vector2_CA8BA287_Out_0 = float2(0.25, 0);
                float4 _SampleTexture2D_2BD9BDBB_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_68DC455A, samplerTexture2D_68DC455A, _Vector2_CA8BA287_Out_0);
                float _SampleTexture2D_2BD9BDBB_R_4 = _SampleTexture2D_2BD9BDBB_RGBA_0.r;
                float _SampleTexture2D_2BD9BDBB_G_5 = _SampleTexture2D_2BD9BDBB_RGBA_0.g;
                float _SampleTexture2D_2BD9BDBB_B_6 = _SampleTexture2D_2BD9BDBB_RGBA_0.b;
                float _SampleTexture2D_2BD9BDBB_A_7 = _SampleTexture2D_2BD9BDBB_RGBA_0.a;
                float2 _Vector2_DE2CADC1_Out_0 = float2(0.25, 0);
                float4 _SampleTexture2D_7C2A0C92_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_E6F2163E, samplerTexture2D_E6F2163E, _Vector2_DE2CADC1_Out_0);
                float _SampleTexture2D_7C2A0C92_R_4 = _SampleTexture2D_7C2A0C92_RGBA_0.r;
                float _SampleTexture2D_7C2A0C92_G_5 = _SampleTexture2D_7C2A0C92_RGBA_0.g;
                float _SampleTexture2D_7C2A0C92_B_6 = _SampleTexture2D_7C2A0C92_RGBA_0.b;
                float _SampleTexture2D_7C2A0C92_A_7 = _SampleTexture2D_7C2A0C92_RGBA_0.a;
                float3 _ReplaceColor_9390AE18_Out_4;
                Unity_ReplaceColor_float(_ReplaceColor_20DE0EF9_Out_4, (_SampleTexture2D_2BD9BDBB_RGBA_0.xyz), (_SampleTexture2D_7C2A0C92_RGBA_0.xyz), 0, _ReplaceColor_9390AE18_Out_4, 0);
                float2 _Vector2_565BDCE7_Out_0 = float2(0.5, 0);
                float4 _SampleTexture2D_576FFED2_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_68DC455A, samplerTexture2D_68DC455A, _Vector2_565BDCE7_Out_0);
                float _SampleTexture2D_576FFED2_R_4 = _SampleTexture2D_576FFED2_RGBA_0.r;
                float _SampleTexture2D_576FFED2_G_5 = _SampleTexture2D_576FFED2_RGBA_0.g;
                float _SampleTexture2D_576FFED2_B_6 = _SampleTexture2D_576FFED2_RGBA_0.b;
                float _SampleTexture2D_576FFED2_A_7 = _SampleTexture2D_576FFED2_RGBA_0.a;
                float2 _Vector2_9A1DBDA8_Out_0 = float2(0.5, 0);
                float4 _SampleTexture2D_4066AB28_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_E6F2163E, samplerTexture2D_E6F2163E, _Vector2_9A1DBDA8_Out_0);
                float _SampleTexture2D_4066AB28_R_4 = _SampleTexture2D_4066AB28_RGBA_0.r;
                float _SampleTexture2D_4066AB28_G_5 = _SampleTexture2D_4066AB28_RGBA_0.g;
                float _SampleTexture2D_4066AB28_B_6 = _SampleTexture2D_4066AB28_RGBA_0.b;
                float _SampleTexture2D_4066AB28_A_7 = _SampleTexture2D_4066AB28_RGBA_0.a;
                float3 _ReplaceColor_72D46A74_Out_4;
                Unity_ReplaceColor_float(_ReplaceColor_9390AE18_Out_4, (_SampleTexture2D_576FFED2_RGBA_0.xyz), (_SampleTexture2D_4066AB28_RGBA_0.xyz), 0, _ReplaceColor_72D46A74_Out_4, 0);
                float2 _Vector2_B762E1D2_Out_0 = float2(0.75, 0);
                float4 _SampleTexture2D_4F1215DC_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_68DC455A, samplerTexture2D_68DC455A, _Vector2_B762E1D2_Out_0);
                float _SampleTexture2D_4F1215DC_R_4 = _SampleTexture2D_4F1215DC_RGBA_0.r;
                float _SampleTexture2D_4F1215DC_G_5 = _SampleTexture2D_4F1215DC_RGBA_0.g;
                float _SampleTexture2D_4F1215DC_B_6 = _SampleTexture2D_4F1215DC_RGBA_0.b;
                float _SampleTexture2D_4F1215DC_A_7 = _SampleTexture2D_4F1215DC_RGBA_0.a;
                float2 _Vector2_D8519BC3_Out_0 = float2(0.75, 0);
                float4 _SampleTexture2D_1D37EC0D_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_E6F2163E, samplerTexture2D_E6F2163E, _Vector2_D8519BC3_Out_0);
                float _SampleTexture2D_1D37EC0D_R_4 = _SampleTexture2D_1D37EC0D_RGBA_0.r;
                float _SampleTexture2D_1D37EC0D_G_5 = _SampleTexture2D_1D37EC0D_RGBA_0.g;
                float _SampleTexture2D_1D37EC0D_B_6 = _SampleTexture2D_1D37EC0D_RGBA_0.b;
                float _SampleTexture2D_1D37EC0D_A_7 = _SampleTexture2D_1D37EC0D_RGBA_0.a;
                float3 _ReplaceColor_32C22A6B_Out_4;
                Unity_ReplaceColor_float(_ReplaceColor_72D46A74_Out_4, (_SampleTexture2D_4F1215DC_RGBA_0.xyz), (_SampleTexture2D_1D37EC0D_RGBA_0.xyz), 0, _ReplaceColor_32C22A6B_Out_4, 0);
                surface.Color = _ReplaceColor_32C22A6B_Out_4;
                surface.Alpha = _SampleTexture2D_99E0D24D_A_7;
                surface.AlphaClipThreshold = 0.5;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_Position;
                float4 texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_Position;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                float4 interp00 : TEXCOORD0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
                output.uv0 =                         input.texCoord0;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags 
            { 
                "LightMode" = "ShadowCaster"
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            #define SHADERPASS_SHADOWCASTER
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            TEXTURE2D(Texture2D_68DC455A); SAMPLER(samplerTexture2D_68DC455A); float4 Texture2D_68DC455A_TexelSize;
            TEXTURE2D(Texture2D_E6F2163E); SAMPLER(samplerTexture2D_E6F2163E); float4 Texture2D_E6F2163E_TexelSize;
            SAMPLER(_SampleTexture2D_99E0D24D_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            // GraphFunctions: <None>
        
            // Graph Vertex
            // GraphVertex: <None>
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _SampleTexture2D_99E0D24D_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                float _SampleTexture2D_99E0D24D_R_4 = _SampleTexture2D_99E0D24D_RGBA_0.r;
                float _SampleTexture2D_99E0D24D_G_5 = _SampleTexture2D_99E0D24D_RGBA_0.g;
                float _SampleTexture2D_99E0D24D_B_6 = _SampleTexture2D_99E0D24D_RGBA_0.b;
                float _SampleTexture2D_99E0D24D_A_7 = _SampleTexture2D_99E0D24D_RGBA_0.a;
                surface.Alpha = _SampleTexture2D_99E0D24D_A_7;
                surface.AlphaClipThreshold = 0.5;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_Position;
                float4 texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_Position;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                float4 interp00 : TEXCOORD0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
                output.uv0 =                         input.texCoord0;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthOnly"
            Tags 
            { 
                "LightMode" = "DepthOnly"
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
            ColorMask 0
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            #define SHADERPASS_DEPTHONLY
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            TEXTURE2D(Texture2D_68DC455A); SAMPLER(samplerTexture2D_68DC455A); float4 Texture2D_68DC455A_TexelSize;
            TEXTURE2D(Texture2D_E6F2163E); SAMPLER(samplerTexture2D_E6F2163E); float4 Texture2D_E6F2163E_TexelSize;
            SAMPLER(_SampleTexture2D_99E0D24D_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            // GraphFunctions: <None>
        
            // Graph Vertex
            // GraphVertex: <None>
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _SampleTexture2D_99E0D24D_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                float _SampleTexture2D_99E0D24D_R_4 = _SampleTexture2D_99E0D24D_RGBA_0.r;
                float _SampleTexture2D_99E0D24D_G_5 = _SampleTexture2D_99E0D24D_RGBA_0.g;
                float _SampleTexture2D_99E0D24D_B_6 = _SampleTexture2D_99E0D24D_RGBA_0.b;
                float _SampleTexture2D_99E0D24D_A_7 = _SampleTexture2D_99E0D24D_RGBA_0.a;
                surface.Alpha = _SampleTexture2D_99E0D24D_A_7;
                surface.AlphaClipThreshold = 0.5;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_Position;
                float4 texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_Position;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                float4 interp00 : TEXCOORD0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
                output.uv0 =                         input.texCoord0;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
            ENDHLSL
        }
        
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}
