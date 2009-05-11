float4x4 World;
float4x4 View;
float4x4 Projection;
float3 Light0Position;
float4 Ambient;
float4 Diffuse;
texture texBld;


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
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 Tex0 : TEXCOORD0;
    float4 Diffuse : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.Tex0 = input.Tex0;

    float4 worldNormal = mul(float4(input.Normal,0.0f), World);
    worldNormal = normalize(worldNormal);
    float4 lightDir = float4((Light0Position- worldPosition ),0.0f);
    lightDir = normalize(lightDir);
    float4 diffuse = saturate(dot(lightDir,worldNormal)) * Diffuse;
    
    output.Diffuse = diffuse + Ambient;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 col = tex2D(smpBld, input.Tex0);
    return input.Diffuse * col;
    return float4(1, 0, 0, 1);
}

technique DefaultTechnique
{
    pass Pass1
    {
        // TODO: set renderstates here.
		CullMode = NONE;
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_1_1 PixelShaderFunction();
    }
}
