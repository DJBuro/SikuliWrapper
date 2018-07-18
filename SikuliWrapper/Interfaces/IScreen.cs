namespace SikuliWrapper.Interfaces
{
    using System;
    using SikuliWrapper.Models;

    public interface IScreen : IDisposable
    {
        void Exists(IImage pattern, double timeoutInSeconds = 1);
        void Click(IImage pattern);
        void Click(IImage pattern, Point offset);
        void DoubleClick(IImage pattern);
        void DoubleClick(IImage pattern, Point offset);
        void Wait(IImage pattern, double timeoutInSeconds = 2);
        void WaitVanish(IImage pattern, double timeoutInSeconds = 3);
        void Type(IImage image, string text);
        void Hover(IImage pattern);
        void Hover(IImage pattern, Point offset);
        void RightClick(IImage pattern);
        void RightClick(IImage pattern, Point offset);
        void DragDrop(IImage fromPattern, IImage toPattern);
    }
}
