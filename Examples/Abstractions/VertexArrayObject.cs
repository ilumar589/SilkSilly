using Silk.NET.OpenGL;

namespace SilkPlayground.Examples.Abstractions;

public readonly record struct VertexArrayObjectHandle(uint Handle);

public readonly record struct VertexArrayObject : IDisposable
{
    private readonly VertexArrayObjectHandle _handle;
    private readonly GL _gl;

    public VertexArrayObject(VertexArrayObjectHandle handle, GL gl)
    {
        _handle = handle;
        _gl = gl ?? throw new ArgumentNullException(nameof(gl));
    }

    public VertexArrayObjectHandle GetVertexArrayHandle()
    {
        return _handle;
    }

    public void Dispose()
    {
    }
}

public static class VertexArrayObjectExtensions
{
    public static VertexArrayObjectHandle CreateVertexArrayObjectHandle(GL gl, ref readonly BufferObject vbo,
        ref readonly BufferObject ebo)
    {
        var vaoHandle = gl.GenVertexArray();
        var vboHandle = vbo.GetVertexObjectHandle();
        var eboHandle = ebo.GetVertexObjectHandle();

        gl.BindVertexArray(vaoHandle);
        BufferObjectExtensions.Bind(gl, in vboHandle);
        BufferObjectExtensions.Bind(gl, in eboHandle);

        return new VertexArrayObjectHandle(vaoHandle);
    }

    public static unsafe void VertexAttributePointer<TVertexType>(GL gl, uint index, int count,
        VertexAttribPointerType type,
        uint vertexSize, int offSet) where TVertexType : unmanaged
    {
        gl.VertexAttribPointer(index, count, type, false, vertexSize * (uint)sizeof(TVertexType),
            (void*)(offSet * sizeof(TVertexType)));
        gl.EnableVertexAttribArray(index);
    }

    private static void Dispose(GL gl, VertexArrayObjectHandle vao)
    {
        gl.DeleteVertexArray(vao.Handle);
    }
}