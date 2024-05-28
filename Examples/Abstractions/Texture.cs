using Silk.NET.OpenGL;
using StbImageSharp;

namespace SilkPlayground.Examples.Abstractions;

public readonly record struct TextureHandle(uint Handle);

public readonly record struct Texture : IDisposable
{
    private readonly TextureHandle _textureHandle;
    private readonly GL _gl;

    public Texture(TextureHandle textureHandle, GL gl)
    {
        _textureHandle = textureHandle;
        _gl = gl;
    }

    public TextureHandle GetTextureHandle()
    {
        return _textureHandle;
    }

    public void Dispose()
    {
        TextureExtensions.Dispose(_gl, in _textureHandle);
    }
}

public static class TextureExtensions
{
    public static void Dispose(GL gl, ref readonly TextureHandle textureHandle)
    {
        gl.DeleteTexture(textureHandle.Handle);
    }
    
    public static unsafe TextureHandle UploadImageAsATexture(GL gl, string path)
    {
        var handle = gl.GenTexture();
        var textureHandle = new TextureHandle(handle);

        Bind(gl, in textureHandle);

        var image = ImageResult.FromMemory(File.ReadAllBytes(path), ColorComponents.RedGreenBlueAlpha);

        fixed (byte* ptr = image.Data)
        {
            gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)image.Width, (uint)image.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
        }
        
        SetParameters(gl);
        
        return textureHandle;
    }

    private static void Bind(GL gl, ref readonly TextureHandle textureHandle,
        TextureUnit textureUnit = TextureUnit.Texture0)
    {
        //When we bind a texture we can choose which texture slot we can bind it to
        gl.ActiveTexture(textureUnit);
        gl.BindTexture(TextureTarget.Texture2D, textureHandle.Handle);
    }

    private static void SetParameters(GL gl)
    {
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) GLEnum.ClampToEdge);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) GLEnum.ClampToEdge);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) GLEnum.LinearMipmapLinear);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);
        
        gl.GenerateMipmap(TextureTarget.Texture2D);
    }
}