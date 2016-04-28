/*
 * https://social.msdn.microsoft.com/Forums/windows/en-US/aaed00ce-4bc9-424e-8c05-c30213171c2c/flickerfree-painting?forum=winforms#b70d2026-e895-430f-a6f2-c8bdff40ac84
 */
namespace PsiComponents
{
    public sealed class NativeMethods
    {
        // ...
        public static void SuspendDrawing(Control c)
        {
            if (c == null)
                throw new ArgumentNullException("c");
            NativeMethods.SendMessage(c.Handle, (Int32)NativeMethods.WM_Message.WM_SETREDRAW, (Int32)0, (Int32)0);
        }
         
        public static void ResumeDrawing(Control c)
        {
            if (c == null)
                throw new ArgumentNullException("c");
            NativeMethods.SendMessage(c.Handle, (Int32)NativeMethods.WM_Message.WM_SETREDRAW, (Int32)1, (Int32)0);
            c.Refresh();
        }
         
        [DllImport("User32")]
        private static extern IntPtr SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);
         
        // ...
        private NativeMethods()
        {
        }
    }
}
