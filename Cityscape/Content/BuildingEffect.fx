float4x4 World;
float4x4 View;
float4x4 Projection;
float3 Light0Position;
float4 Ambient;
float4 Diffuse;
texture texBld;
float LightDistance;

sampler2D smpBld = sampler_state
{
  texture = <texBld>;
  MinFilter = LINEAR;
  MagFilter = LINEAR;
  MipFilter = LINEAR;
  AddressU = WRAP;
  AddressV = WRAP;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL;
    float2 Tex0 : TEXCOORD0;
    float3 Mod : TEXCOORD1;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 Tex0 : TEXCOORD0;
    float4 Diffuse : COLOR0;
    float3 Mod : TEXCOORD1;
    float  Fogging : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.Tex0 = input.Tex0;
    output.Mod = input.Mod;
    
    float4 worldNormal = mul(float4(input.Normal,0.0f), World);
    worldNormal = normalize(worldNormal);
    float4 lightDir = float4((Light0Position- worldPosition ),0.0f);
    lightDir = normalize(lightDir);
    float4 diffuse = saturate(dot(lightDir,worldNormal)) * Diffuse;
    float atten = 1 / (LightDistance * distance(Light0Position, input.Position));
    float dist = length(viewPosition);
    
    output.Fogging = 1 / (0.1f * dist);
    output.Diffuse = (diffuse) + Ambient;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 col = tex2D(smpBld, input.Tex0) * float4(input.Mod, 0.0f);
    
    float4 fogColour = float4(0.0f, 0.0f, 0.1f, 0.0f);
    return lerp(fogColour, input.Diffuse * col, input.Fogging);
}

float4 PixelShaderFunction_NoFog(VertexShaderOutput input) : COLOR0
{
    float4 col = tex2D(smpBld, input.Tex0) * float4(input.Mod, 0.0f);
    
    return input.Diffuse * col;
}

technique DefaultTechnique
{
    pass Pass1
    {
        // TODO: set renderstates here.
		CullMode = ccw;
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_1_1 PixelShaderFunction();
    }
}

technique NoFogging
{
    pass Pass1
    {
        // TODO: set renderstates here.
		CullMode = NONE;
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_1_1 PixelShaderFunction_NoFog();
    }
}
