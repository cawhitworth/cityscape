float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CamPos;
float Size;

texture texParticle;

// TODO: add effect parameters here.

sampler2D smpParticle = sampler_state
{
  texture = <texParticle>;
  MinFilter = LINEAR;
  MagFilter = LINEAR;
  MipFilter = LINEAR;
  AddressU = WRAP;
  AddressV = WRAP;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 Tex0 : TEXCOORD0;
	float3 Color : COLOR0;
    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 Tex0 : TEXCOORD0;
	float3 Color : COLOR0;
	float  Fogging : TEXCOORD1;
    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	float3 position = mul(input.Position, World);
	float3 eyeVec = position - CamPos;
	
	float3 up = float3(0,1,0);
	float3 right = cross(eyeVec, up);
	right = normalize(right);
		
	position += (input.Tex0.x - 0.5f) * right * Size;
	position += (0.5f - input.Tex0.y) * up * Size;
	
    float4 viewPosition = mul(float4(position,1), View);
    output.Position = mul(viewPosition, Projection);

	output.Tex0 = input.Tex0;
	output.Color = input.Color;
	output.Fogging = 0.0f;
    // TODO: add your vertex shader code here.

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float4 output = tex2D(smpParticle, input.Tex0) * float4(input.Color,1);
    return output;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.
        CullMode = None;
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_1_1 PixelShaderFunction();
    }
}
