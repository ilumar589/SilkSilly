using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace SilkPlayground.Examples.MainWindow;

public static class P1
{
    private static IWindow? _window;

    public static void P1Example()
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = "LearnOpenGL with SIlk.NET";

        _window = Window.Create(options);
        
        // assign events
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.FramebufferResize += OnFrameBufferResize;

        //Run the window.
        _window.Run();

        // window.Run() is a BLOCKING method - this means that it will halt execution of any code in the current
        // method until the window has finished running. Therefore, this dispose method will not be called until you
        // close the window.
        _window.Dispose();
    }

    private static void OnLoad()
    {
        var input = _window?.CreateInput();

        if (input?.Keyboards == null) return;
        
        foreach (var inputKeyboard in input.Keyboards)
        {
            inputKeyboard.KeyDown += KeyDown;
        }
    }

    private static void OnUpdate(double delta)
    {
        // here all updates to the program should be done
    }

    private static void OnRender(double delta)
    {
        // here all rendering should be done
    }

    private static void OnFrameBufferResize(Vector2D<int> newSize)
    {
        // update aspect rations, clipping regions, viewports, etc.
    }

    private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
    {
        if (arg2 == Key.Escape)
        {
            _window?.Close();
        }
    }
}