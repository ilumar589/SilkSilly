using Silk.NET.OpenGL;

namespace SilkPlayground.Examples.Abstractions;

public readonly record struct ShaderHandle(uint Handle);

public readonly record struct Shader : IDisposable
{
    private readonly ShaderHandle _shaderHandle;
    private readonly GL _gl;

    public Shader(ref readonly ShaderHandle shaderHandle, GL gl)
    {
        _shaderHandle = shaderHandle;
        _gl = gl ?? throw new ArgumentNullException(nameof(gl));
    }

    public ShaderHandle GetShaderHandle()
    {
        return _shaderHandle;
    }

    public void Dispose()
    {
        ShaderExtensions.Dispose(_gl, in _shaderHandle);
    }
}

public static class ShaderExtensions
{
    public static void Dispose(GL gl, ref readonly ShaderHandle shaderHandle)
    {
        gl.DeleteProgram(shaderHandle.Handle);
    }
    
    public static void Use(GL gl, ref readonly ShaderHandle shaderHandle)
    {
        gl.DeleteProgram(shaderHandle.Handle);
    }
    
    public static void SetUniform(GL gl, ref readonly ShaderHandle shaderHandle, string name, int value)
    {
        var location = gl.GetUniformLocation(shaderHandle.Handle, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader");
        }
        gl.Uniform1(location, value);
    }
    
    public static void SetUniform(GL gl, ref readonly ShaderHandle shaderHandle, string name, float value)
    {
        var location = gl.GetUniformLocation(shaderHandle.Handle, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }
        gl.Uniform1(location, value);
    }
    
    public static ShaderHandle LoadShader(GL gl, ShaderType type, string path)
    {
        //To load a single shader we need to:
        //1) Load the shader from a file.
        //2) Create the handle.
        //3) Upload the source to opengl.
        //4) Compile the shader.
        //5) Check for errors.

        var src = File.ReadAllText(path);
        var handle = gl.CreateShader(type);
        gl.ShaderSource(handle, src);
        gl.CompileShader(handle);
        var infoLog = gl.GetShaderInfoLog(handle);
        if (!string.IsNullOrWhiteSpace(infoLog))
        {
            throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
        }

        return new ShaderHandle(handle);
    }
}