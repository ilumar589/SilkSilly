using Silk.NET.OpenGL;
using StbImageSharp;

namespace SilkPlayground.Examples.Abstractions;

public readonly record struct TextureHandle(uint Handle);

public static class TextureExtensions
{
    public static unsafe TextureHandle UploadImageAsATexture(GL gl, string path)
    {
        var handle = gl.GenTexture();
        var textureHandle = new TextureHandle(handle);

        Bind(gl, in textureHandle);

        // Load image in memory
        var image = ImageResult.FromMemory(File.ReadAllBytes(path), ColorComponents.RedGreenBlueAlpha);

        fixed (byte* ptr = image.Data)
        {
            // create our texture and upload the image data
            gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)image.Width, (uint)image.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
        }
        
        // set parameters

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
        
    }
}