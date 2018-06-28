namespace SikuliWrapper.Interfaces
{
    using System;
    using SikuliWrapper.Models;

    public interface IScreen : IDisposable
    {
        bool Exists(IImage pattern, double timeoutInSeconds = 1);
        bool Click(IImage pattern);
        bool Click(IImage pattern, Point offset);
        bool DoubleClick(IImage pattern);
        bool DoubleClick(IImage pattern, Point offset);
        bool Wait(IImage pattern, double timeoutInSeconds = 2);
        bool WaitVanish(IImage pattern, double timeoutInSeconds = 3);
        bool Type(IImage image, string text);
        bool Hover(IImage pattern);
        bool Hover(IImage pattern, Point offset);
        bool RightClick(IImage pattern);
        bool RightClick(IImage pattern, Point offset);
        bool DragDrop(IImage fromPattern, IImage toPattern);
    }
}
