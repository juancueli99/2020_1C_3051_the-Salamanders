/**************************************************************************************/
/* Variables comunes */
/**************************************************************************************/

//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

float3 eyePosition;

struct Light
{
    float3 position;
    float3 color;
};

Light lights[2];

float screenWidth, screenHeight;
float timer;

static const int kernelRadius = 5;
static const int kernelSize = 25;
static const float kernel[kernelSize] =
{
    0.003765, 0.015019, 0.023792, 0.015019, 0.003765,
    0.015019, 0.059912, 0.094907, 0.059912, 0.015019,
    0.023792, 0.094907, 0.150342, 0.094907, 0.023792,
    0.015019, 0.059912, 0.094907, 0.059912, 0.015019,
    0.003765, 0.015019, 0.023792, 0.015019, 0.003765,
};

//Textura para DiffuseMap
texture texDiffuseMap;
sampler2D diffuseMap = sampler_state
{
    Texture = (texDiffuseMap);
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

//Textura para full screen quad
texture renderTarget;
sampler2D renderTargetSampler = sampler_state
{
    Texture = (renderTarget);
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

//Input del Vertex Shader
struct VS_INPUT_DEFAULT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinates : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT_DEFAULT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinates : TEXCOORD0;
    float4 PosView : COLOR0;
};

//Vertex Shader
VS_OUTPUT_DEFAULT VSDefault(VS_INPUT_DEFAULT input)
{
    VS_OUTPUT_DEFAULT output;

	// Enviamos la posicion transformada
    output.Position = mul(input.Position, matWorldViewProj);
    
    // Propagar las normales por la matriz normal
    output.Normal = mul(input.Normal, matInverseTransposeWorld);
    
	// Propagar coordenadas de textura
    output.TextureCoordinates = input.TextureCoordinates;
    
    output.PosView = mul(input.Position, matWorldView);

    return output;
}

//Pixel Shader
float4 PSFogEffect(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float startFogDistance = 3500;
    float endFogDistance = 4000;
    float4 ColorFog = float4(0.5, 0.5, 0.5, 1);
    
    float4 fvBaseColor = tex2D(diffuseMap, input.TextureCoordinates);
    
    if (fvBaseColor.a < 0.5)
    {
        discard;
    }
    
    if (input.PosView.z < startFogDistance)
        // Si estoy adentro del startFogDistance
        // muestro el color original de la textura
        return fvBaseColor;
    else if (input.PosView.z > endFogDistance)
    {
        // Si estoy fuera del endFogDistance
        // muestro el color gris
        fvBaseColor = ColorFog;
        return fvBaseColor;
    }
    else
    {
		// combino fog y textura
        float1 total = endFogDistance - startFogDistance;
        float1 resto = input.PosView.z - startFogDistance;
        float1 proporcion = resto / total;
        fvBaseColor = lerp(fvBaseColor, ColorFog, proporcion);
        
        return fvBaseColor;
    }
    
    
    /*
    // combino fog y textura
    float1 total = endFogDistance - startFogDistance;
    float1 resto = input.PosView.z - startFogDistance;
    float1 proporcion = resto / total;
    float4 fvBaseColor2 = lerp(fvBaseColor, ColorFog, proporcion);
    
    return (input.PosView.z <= startFogDistance) ? fvBaseColor : fvBaseColor2;*/

    

}

//Input del Vertex Shader
struct VS_INPUT_POSTPROCESS
{
    float4 Position : POSITION0;
    float2 TextureCoordinates : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT_POSTPROCESS
{
    float4 Position : POSITION0;
    float2 TextureCoordinates : TEXCOORD0;
};

//Vertex Shader
VS_OUTPUT_POSTPROCESS VSPostProcess(VS_INPUT_POSTPROCESS input)
{
    VS_OUTPUT_POSTPROCESS output;

	// Propagamos la posicion, ya que esta en espacio de pantalla
    output.Position = input.Position;

	// Propagar coordenadas de textura
    output.TextureCoordinates = input.TextureCoordinates;

    return output;
}

//Pixel Shader
float4 PSPostProcessMonster(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float4 tex = tex2D(renderTargetSampler, input.TextureCoordinates);
    
    float borde = distance(input.TextureCoordinates, float2(0.5, 0.5));
    
    int pixelX = round(input.TextureCoordinates.x * screenWidth);

    int porcentajeX = 5 * tan(timer * 3);
    int lineasX = int(pixelX) % porcentajeX;
    
    
    int porcentajeX1 = 5 * tan(timer * 5);
    int lineasX1 = int(pixelX) % porcentajeX;
        
    if (lineasX <= 1.0)
    {
        tex = (lineasX <= 1.0) * tex;
        tex = lerp(tex, float4(0, 0, 0, 1), 0.6);
        return lerp(tex, float4(0, 0, 0, 1), borde * 1.25);
    }
    else
    {
        tex = (lineasX1 <= 1.0) * tex;
        tex = lerp(tex, float4(1, 1, 1, 1), 0.6);
        return lerp(tex, float4(0, 0, 0, 1), borde * 1.25);
    }
}

//Pixel Shader
float4 PSPostProcessNightVision(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float4 tex = tex2D(renderTargetSampler, input.TextureCoordinates);
    
    float bordeDerecho = distance(input.TextureCoordinates, float2(0.25, 0.5));
    float bordeIzquierdo = distance(input.TextureCoordinates, float2(0.75, 0.5));
    
    int pixelY = round(input.TextureCoordinates.y * screenHeight);
    
    int lineasY = int(pixelY) % 5;
    
    tex = (lineasY <= 1.0) * tex;
    tex = lerp(tex, float4(0, 0.5, 0, 1), 0.6);
    
    return (input.TextureCoordinates.x < 0.5) ? lerp(tex, float4(0, 0, 0, 1), bordeDerecho * 2) :
    lerp(tex, float4(0, 0, 0, 1), bordeIzquierdo * 2);
}

float4 PSPostProcessDefault(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float4 tex =  tex2D(renderTargetSampler, input.TextureCoordinates);
    return lerp(tex, float4(0, 0, 0, 1), 0.7);
}

float4 PSPostProcessLinterna(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float borde = distance(float2(input.TextureCoordinates.x, input.TextureCoordinates.y / 2), float2(0.5, 0.2));
    
    float4 tex = tex2D(renderTargetSampler, input.TextureCoordinates);
    
    return lerp(tex, float4(0, 0, 0, 1), borde * 2.5);
}


float4 PSPostProcessVela(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float borde = distance(float2(input.TextureCoordinates.x, input.TextureCoordinates.y / 2), float2(0.5, 0.2));
    
    float4 tex = tex2D(renderTargetSampler, input.TextureCoordinates);
    
    tex = lerp(tex, float4(0.6,0.2,0,1), 0.3);
    
    return lerp(tex, float4(0, 0, 0, 1), borde * 2.5);
}


technique FogEffect
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSDefault();
        PixelShader = compile ps_3_0 PSFogEffect();
    }
}

technique PostProcessDefault
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSPostProcess();
        PixelShader = compile ps_3_0 PSPostProcessDefault();
    }
}

technique PostProcessVela
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSPostProcess();
        PixelShader = compile ps_3_0 PSPostProcessVela();
    }
}

technique PostProcessLinterna
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSPostProcess();
        PixelShader = compile ps_3_0 PSPostProcessLinterna();
    }
}

technique PostProcessMonster
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSPostProcess();
        PixelShader = compile ps_3_0 PSPostProcessMonster();
    }
}

technique PostProcessNightVision
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSPostProcess();
        PixelShader = compile ps_3_0 PSPostProcessNightVision();
    }
}

/////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////

struct VS_OUTPUT_TRAIL
{
    float4 Position : POSITION0;
    float3 MeshPosition : TEXCOORD1;
    float3 Normal : NORMAL0;
    float2 TextureCoordinates : TEXCOORD0;
};

//Vertex Shader
VS_OUTPUT_TRAIL VSTrailSectarian(VS_INPUT_DEFAULT input)
{
    VS_OUTPUT_TRAIL output;
    
	// Enviamos la posicion transformada
    output.Position = mul(input.Position, matWorldViewProj);
 
    output.MeshPosition = input.Position;
    
    // Propagar las normales por la matriz normal
    output.Normal = mul(input.Normal, matInverseTransposeWorld);
    
	// Propagar coordenadas de textura
    output.TextureCoordinates = input.TextureCoordinates;
    
    return output;
}

//Pixel Shader
float4 PSTrailSectarian(VS_OUTPUT_TRAIL input) : COLOR0
{
    
    float4 textureColor = tex2D(diffuseMap, input.TextureCoordinates);
   
    bool condicion1 = abs(input.MeshPosition.x) < 20.0;
    bool condicion2 = abs(input.MeshPosition.x) > 4.0;
    bool condicionIntervalo = input.MeshPosition.x > 0.5; //true; //condicion1 && condicion2;
    bool condicionIntervaloY = input.MeshPosition.y > 400; //input.MeshPosition.y > 40.0 && input.MeshPosition.y < 50.0;
    bool condicionIntervaloZ = input.MeshPosition.z > 0.5; //input.MeshPosition.z < 0;
        if (condicionIntervalo && condicionIntervaloY && condicionIntervaloZ)
    {
        return lerp(textureColor, float4(1, 0, 0, 1), abs(sin(timer)));
    }
    else
    {
        return textureColor;
    }
}

technique TrailEffectSECTARIAN
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSTrailSectarian();
        PixelShader = compile ps_3_0 PSTrailSectarian();
    }
}


/////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////

// ---------------------------------------------------------
// Ejemplo toon Shading
// ---------------------------------------------------------

/**************************************************************************************/
/* Variables comunes */
/**************************************************************************************/

//Matrices de transformacion
float4x4 matWorldViewProjAnt; //Matriz World * View * Proj anterior
float4x4 matView; //Matriz View actual
float4x4 matProj; //Matriz Projection actual
float4x4 matViewAnt; //Matriz View anterior



/*float screen_dx = screenWidth; // tamaño de la pantalla en pixels
float screen_dy = screenHeight;*/

//Input del Vertex Shader
struct VS_INPUT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float4 Color : COLOR;
    float2 Texcoord : TEXCOORD0;
};

texture g_RenderTarget;
sampler RenderTarget =
sampler_state
{
    Texture = <g_RenderTarget>;
    ADDRESSU = CLAMP;
    ADDRESSV = CLAMP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

texture texVelocityMap;
sampler2D velocityMap = sampler_state
{
    Texture = (texVelocityMap);
    MinFilter = POINT;
    MagFilter = POINT;
    AddressU = Clamp;
    AddressV = Clamp;
};

texture texVelocityMapAnt;
sampler2D velocityMapAnt = sampler_state
{
    Texture = (texVelocityMapAnt);
    MinFilter = POINT;
    MagFilter = POINT;
    AddressU = Clamp;
    AddressV = Clamp;
};

//Output del Vertex Shader
struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float3 Norm : TEXCOORD1; // Normales
    float4 vPosActual : TEXCOORD2; // Posicion actual
    float4 vPosAnterior : TEXCOORD3; // Posicion anterior
    //float2 Vel : TEXCOORD3; // velocidad por pixel

};

//Vertex Shader
VS_OUTPUT vs_main(VS_INPUT Input)
{
    VS_OUTPUT Output;

	//Proyectar posicion
    Output.Position = mul(Input.Position, matWorldViewProj);
   
	//Las Texcoord quedan igual
    Output.Texcoord = Input.Texcoord;

	// Transformo la normal y la normalizo
    Output.Norm = normalize(mul(Input.Normal, matWorld));

	/*
	// Computo la velocidad del vertice
	// posicion actual
	float4 vPosActual = Output.Position;
	// posicion anterior
	float4 vPosAnterior = mul( Input.Position,matWorld * matViewAnt * matProj);
	vPosActual /= vPosActual.w;
	vPosAnterior /= vPosAnterior.w;
	float2 velocity = vPosActual - vPosAnterior;    
    // lo propago
    Output.Vel = velocity;*/

	// posicion actual
    Output.vPosActual = Output.Position;
	// posicion anterior
    Output.vPosAnterior = mul(Input.Position, matWorld * matViewAnt * matProj);

    return (Output);
   
}

//Pixel Shader
float4 ps_main(float3 Texcoord : TEXCOORD0) : COLOR0
{
    float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
    if (fvBaseColor.a < 0.1)
        discard;
    return fvBaseColor;
}

//Pixel Shader Velocity
float4 ps_velocity(float3 Texcoord : TEXCOORD0, float4 vPosActual : TEXCOORD2, float4 vPosAnterior : TEXCOORD3) : COLOR0
{
	//Obtener el texel de textura
    float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
    
    if (fvBaseColor.a < 0.1)
        discard;

    vPosActual /= vPosActual.w;
    vPosAnterior /= vPosAnterior.w;
    float2 Vel = vPosActual - vPosAnterior;

    return float4(Vel.x, Vel.y, 0.0f, 1.0f);
}

technique DefaultTechnique
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_main();
    }
}

technique VelocityMap
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_velocity();
    }

}

void vs_copy(float4 vPos : POSITION, float2 vTex : TEXCOORD0, out float4 oPos : POSITION, out float2 oScreenPos : TEXCOORD0)
{
    oPos = vPos;
    oScreenPos = vTex;
    oPos.w = 1;
}

float PixelBlurConst = 0.05f;
static const float NumberOfPostProcessSamples = 12.0f;

float4 ps_motion_blur(in float2 Tex : TEXCOORD0) : COLOR0
{
    float4 curFramePixelVelocity = tex2D(velocityMap, Tex);
    float4 lastFramePixelVelocity = tex2D(velocityMapAnt, Tex);

    float2 pixelVelocity;
    float curVelocitySqMag = curFramePixelVelocity.r * curFramePixelVelocity.r +
                             curFramePixelVelocity.g * curFramePixelVelocity.g;
    float lastVelocitySqMag = lastFramePixelVelocity.r * lastFramePixelVelocity.r +
                              lastFramePixelVelocity.g * lastFramePixelVelocity.g;
                                   
    if (lastVelocitySqMag > curVelocitySqMag)
    {
        pixelVelocity.x = lastFramePixelVelocity.r * PixelBlurConst;
        pixelVelocity.y = -lastFramePixelVelocity.g * PixelBlurConst;
    }
    else
    {
        pixelVelocity.x = curFramePixelVelocity.r * PixelBlurConst;
        pixelVelocity.y = -curFramePixelVelocity.g * PixelBlurConst;
    }
	
	
    float3 Blurred = 0;
    for (float i = 0; i < NumberOfPostProcessSamples; i++)
    {
        float2 lookup = pixelVelocity * i / NumberOfPostProcessSamples + Tex;
        float4 Current = tex2D(RenderTarget, lookup);
        Blurred += Current.rgb;
    }

    return float4(Blurred / NumberOfPostProcessSamples, 1.0f);
	//	return tex2D(velocityMap,Tex)  ;
	//	return tex2D(RenderTarget,Tex) ;
}

technique PostProcessMotionBlur
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_copy();
        PixelShader = compile ps_3_0 ps_motion_blur();
    }
}

float4 ps_draw_grid(in float2 Tex : TEXCOORD0, float2 vPos : VPOS) : COLOR0
{
    int x = round(vPos.x / 4);
    int y = round(vPos.y / 4);
    if (x % 5 != 0 || y % 5 != 0)
        discard;
    return float4(1, 1, 1, 1);
}

technique DrawGrid
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_copy();
        PixelShader = compile ps_3_0 ps_draw_grid();
    }
}