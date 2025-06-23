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
        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            if (CheckOverscroll(e.Timestamp) == CheckResult.Overscroll)
            {
                // Ignore event to prevent overscroll.
                Console.WriteLine($"### scroll event ignored ({e.Timestamp})");
                e.Handled = true;
                return;
            }

            base.OnPointerWheelChanged(e);
        }

        private enum CheckResult
        {
            NoOverscroll,
            Overscroll,
        }

        private CheckResult CheckOverscroll(ulong timestamp)
        {
            // This value should be not too great (noticeable overscroll) and
            // not too small (each event starts a new run, so we get overscroll
            // again).
            //
            // A few tests showed 70ms as an acceptable value.
            const ulong maxSkewMilliseconds = 70;

            // We don't know the definition of `timestamp`.  But it clearly has
            // a millisecond resolution.  So we synchronize ourselves using a
            // unix time.

            var last = _lastTimestamp;
            _lastTimestamp = timestamp;

            var unixTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var diff = unixTime - timestamp;

            if (timestamp > last + maxSkewMilliseconds || timestamp < last)
            {
                // The distance between two event timestamps is too great (or
                // there was an overflow). So stop the run and start a new one.
                _referenceDiff = diff;

                // This is the first event in a run, so no overscroll.
                return CheckResult.NoOverscroll;
            }
            else if (diff > _referenceDiff + maxSkewMilliseconds)
            {
                // The scroll events have overrun the wall clock (unix time).
                // This is the overscroll we don't want.  So ignore the event.
                return CheckResult.Overscroll;
            }
            else
            {
                // We are in a run of scroll events, and they are nice in order
                // with plausible timestamps.  So no overscroll.
                return CheckResult.NoOverscroll;
            }
        }

        private ulong _lastTimestamp;
        private ulong _referenceDiff;
    }
}
