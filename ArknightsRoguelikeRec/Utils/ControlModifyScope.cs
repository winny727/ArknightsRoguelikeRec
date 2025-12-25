using System;
using System.Windows.Forms;

public sealed class ControlModifyScope : IDisposable
{
    private readonly Control _control;

    public ControlModifyScope(Control control)
    {
        _control = control ?? throw new ArgumentNullException(nameof(control));
        _control.SuspendLayout();
    }

    public void Dispose()
    {
        _control.ResumeLayout();
    }
}