using Silk.NET.OpenGL;

namespace SilkPlayground.Examples.Abstractions;

public readonly record struct VertexBufferObjectHandle(uint Handle, BufferTargetARB BufferType);

public readonly record struct BufferObject : IDisposable
{
    private readonly VertexBufferObjectHandle _handle;
    private readonly GL _gl;

    public BufferObject(VertexBufferObjectHandle handle, GL gl)
    {
        _handle = handle;
        _gl = gl ?? throw new ArgumentNullException(nameof(gl));
    }

    public VertexBufferObjectHandle GetVertexObjectHandle()
    {
        return _handle;
    }

    public void Dispose()
    {
        BufferObjectExtensions.Dispose(_gl, in _handle);
    }
}

public static class BufferObjectExtensions
{
    public static unsafe VertexBufferObjectHandle CreateVertexBuffer<TDataType>(GL gl,
        ref readonly Span<TDataType> data, BufferTargetARB bufferType) where TDataType : unmanaged
    {
        var handle = gl.GenBuffer();
        gl.BindBuffer(bufferType, handle);

        fixed (void* ptr = data)
        {
            gl.BufferData(bufferType, (nuint)(data.Length * sizeof(TDataType)), ptr, BufferUsageARB.StaticDraw);
        }

        return new VertexBufferObjectHandle(handle, bufferType);
    }

    public static void Bind(GL gl, ref readonly VertexBufferObjectHandle vbo)
    {
        gl.BindBuffer(vbo.BufferType, vbo.Handle);
    }

    public static void Dispose(GL gl, ref readonly VertexBufferObjectHandle vbo)
    {
        gl.DeleteBuffer(vbo.Handle);
    }
}