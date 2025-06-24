using System;
using Avalonia.Controls;
using Avalonia.Input;

namespace SourceGit.Views
{
    /// <summary>
    /// <p>
    ///     Variant of the class <see cref="VirtualizingStackPanel"/> that
    ///     prevents overscroll.
    /// </p>
    /// <p>
    ///     Overscroll can occur, if you scroll really fast (for example using a
    ///     Logitech MX Master mouse with MagSpeed Electromagnetic scrolling)
    ///     and then suddenly stop scrolling (by stopping the scroll wheel with
    ///     your finger).  If the scrolling itself doesn't stop immediately, you
    ///     experience overscroll.  It could also be described as lagging.
    /// </p>
    /// <p>
    ///     The reason is, that the scroll events are buffered and pile up on
    ///     fast scrolling.  The solution is to ignore events that would lead to
    ///     overscroll.
    /// </p>
    /// </summary>
    public class VirtualizingStackPanelNoOverscroll : VirtualizingStackPanel
    {
        // This value should be not too great (noticeable overscroll) and
        // not too small (each event starts a new run, so we get overscroll
        // again).
        //
        // A few tests showed 70ms as an acceptable value.
        private const long MaxDelayMilliseconds = 70;

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            // How much later arrived the event here compared to when it was
            // raised?
            var delay = Environment.TickCount64 - (long)e.Timestamp;

            if (delay > MaxDelayMilliseconds)
            {
                // Ignore event to prevent overscroll.
                e.Handled = true;
                return;
            }

            base.OnPointerWheelChanged(e);
        }
    }
}
