//#define HAX

using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gma.UserActivityMonitor
{
    public static partial class HookManager
    {
        /// <summary>
        /// The CallWndProc hook procedure is an application-defined or library-defined callback 
        /// function used with the SetWindowsHookEx function. The HOOKPROC type defines a pointer 
        /// to this callback function. CallWndProc is a placeholder for the application-defined 
        /// or library-defined function name.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/callwndproc.asp
        /// </remarks>
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        //##############################################################################
        #region Mouse hook processing

        /// <summary>
        /// This field is not objectively needed but we need to keep a reference on a delegate which will be 
        /// passed to unmanaged code. To avoid GC to clean it up.
        /// When passing delegates to unmanaged code, they must be kept alive by the managed application 
        /// until it is guaranteed that they will never be called.
        /// </summary>
        private static HookProc s_MouseDelegate;

        /// <summary>
        /// Stores the handle to the mouse hook procedure.
        /// </summary>
        private static int s_MouseHookHandle;

        private static int m_OldX;
        private static int m_OldY;

        /// <summary>
        /// A callback function which will be called every Time a mouse activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
        private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                //Marshall the data from callback.
                MouseLLHookStruct mouseHookStruct = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));

                //detect button clicked
                MouseButtons button = MouseButtons.None;
                short mouseDelta = 0;
                int clickCount = 0;
                bool mouseDown = false;
                bool mouseUp = false;

                switch (wParam)
                {
                    case WM_LBUTTONDOWN:
                        mouseDown = true;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WM_LBUTTONUP:
                        mouseUp = true;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WM_LBUTTONDBLCLK: 
                        button = MouseButtons.Left;
                        clickCount = 2;
                        break;
                    case WM_RBUTTONDOWN:
                        mouseDown = true;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WM_RBUTTONUP:
                        mouseUp = true;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WM_RBUTTONDBLCLK: 
                        button = MouseButtons.Right;
                        clickCount = 2;
                        break;
                    case WM_MBUTTONDOWN:
                        mouseDown = true;
                        button = MouseButtons.Middle;
                        clickCount = 1;
                        break;
                    case WM_MBUTTONUP:
                        mouseUp = true;
                        button = MouseButtons.Middle;
                        clickCount = 1;
                        break;
                    case WM_MBUTTONDBLCLK:
                        button = MouseButtons.Middle;
                        clickCount = 2;
                        break;
                    case WM_XBUTTONDOWN:
                        mouseDown = true;
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
                        if (mouseDelta == 1)
                            button = MouseButtons.XButton1;
                        else
                            button = MouseButtons.XButton2;
                        mouseDelta = 0;
                        clickCount = 1;
                        break;
                    case WM_XBUTTONUP:
                        mouseUp = true;
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
                        if (mouseDelta == 1)
                            button = MouseButtons.XButton1;
                        else
                            button = MouseButtons.XButton2;
                        mouseDelta = 0;
                        clickCount = 1;
                        break;
                    case WM_XBUTTONDBLCLK:
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
                        if (mouseDelta == 1)
                            button = MouseButtons.XButton1;
                        else
                            button = MouseButtons.XButton2;
                        mouseDelta = 0;
                        clickCount = 2;
                        break;
                    case WM_MOUSEWHEEL:
                        //If the message is WM_MOUSEWHEEL, the high-order word of MouseData member is the wheel delta. 
                        //One wheel click is defined as WHEEL_DELTA, which is 120. 
                        //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
                       
                    //TODO: X BUTTONS (I havent them so was unable to test)
                        //If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, 
                        //or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
                        //and the low-order word is reserved. This value can be one or more of the following values. 
                        //Otherwise, MouseData is not used. 
                        break;
                }

                //generate event 
                MouseEventExtArgs e = new MouseEventExtArgs(
                                                   button,
                                                   clickCount,
                                                   mouseHookStruct.Point.X,
                                                   mouseHookStruct.Point.Y,
                                                   mouseDelta);

                //Mouse up
                if (s_MouseUp!=null && mouseUp)
                {
                    s_MouseUp.Invoke(null, e);
                }

                //Mouse down
                if (s_MouseDown != null && mouseDown)
                {
                    s_MouseDown.Invoke(null, e);
                }

                //If someone listens to click and a click is heppened
                if (s_MouseClick != null && clickCount>0)
                {
                    s_MouseClick.Invoke(null, e);
                }

                //If someone listens to click and a click is heppened
                if (s_MouseClickExt != null && clickCount > 0)
                {
                    s_MouseClickExt.Invoke(null, e);
                }

                //If someone listens to double click and a click is heppened
                if (s_MouseDoubleClick != null && clickCount == 2)
                {
                    s_MouseDoubleClick.Invoke(null, e);
                }

                //Wheel was moved
                if (s_MouseWheel!=null && mouseDelta!=0)
                {
                    s_MouseWheel.Invoke(null, e);
                }

                //If someone listens to move and there was a change in coordinates raise move event
                if ((s_MouseMove!=null || s_MouseMoveExt!=null) && (m_OldX != mouseHookStruct.Point.X || m_OldY != mouseHookStruct.Point.Y))
                {
                    m_OldX = mouseHookStruct.Point.X;
                    m_OldY = mouseHookStruct.Point.Y;
                    if (s_MouseMove != null)
                    {
                        s_MouseMove.Invoke(null, e);
                    }

                    if (s_MouseMoveExt != null)
                    {
                        s_MouseMoveExt.Invoke(null, e);
                    }
                }

                if (e.Handled)
                {
                    return -1;
                }
            }

            //call next hook
            return CallNextHookEx(s_MouseHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalMouseEvents()
        {
            // install Mouse hook only if it is not installed and must be installed
            if (s_MouseHookHandle == 0)
            {
                //See comment of this field. To avoid GC to clean it up.
                s_MouseDelegate = MouseHookProc;
                //install hook
                s_MouseHookHandle = SetWindowsHookEx(
                    WH_MOUSE_LL,
                    s_MouseDelegate,
                    Marshal.GetHINSTANCE(
                        Assembly.GetExecutingAssembly().GetModules()[0]),
                    0);
                //If SetWindowsHookEx fails.
                if (s_MouseHookHandle == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup

                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private static void TryUnsubscribeFromGlobalMouseEvents()
        {
            //if no subsribers are registered unsubsribe from hook
            if (s_MouseClick == null &&
                s_MouseDown == null &&
                s_MouseMove == null &&
                s_MouseUp == null &&
                s_MouseClickExt == null &&
                s_MouseMoveExt == null &&
                s_MouseWheel == null)
            {
                ForceUnsunscribeFromGlobalMouseEvents();
            }
        }

        private static void ForceUnsunscribeFromGlobalMouseEvents()
        {
            if (s_MouseHookHandle != 0)
            {
                //uninstall hook
                int result = UnhookWindowsHookEx(s_MouseHookHandle);
                //reset invalid handle
                s_MouseHookHandle = 0;
                //Free up for GC
                s_MouseDelegate = null;
                //if failed and exception must be thrown
                if (result == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }
        
        #endregion

        //##############################################################################
        #region Keyboard hook processing

        /// <summary>
        /// This field is not objectively needed but we need to keep a reference on a delegate which will be 
        /// passed to unmanaged code. To avoid GC to clean it up.
        /// When passing delegates to unmanaged code, they must be kept alive by the managed application 
        /// until it is guaranteed that they will never be called.
        /// </summary>
        private static HookProc s_KeyboardDelegate;

        /// <summary>
        /// Stores the handle to the Keyboard hook procedure.
        /// </summary>
        private static int s_KeyboardHookHandle;

#if HAX
        private static bool _is_shift = false;
        private static bool _is_caps = ((GetKeyState(VK_CAPITAL) & 1) == 1);
#endif

        /// <summary>
        /// A callback function which will be called every Time a keyboard activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
        private static int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            if (!HookManager.Enabled)
            {
                //forward to other application
                return CallNextHookEx(s_KeyboardHookHandle, nCode, wParam, lParam);
            }

            //indicates if any of underlaing events set e.Handled flag
            bool handled = false;

            if (nCode >= 0)
            {
                //read structure KeyboardHookStruct at lParam
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                //raise KeyDown
                if (s_KeyDown != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.VirtualKeyCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    s_KeyDown.Invoke(null, e);
                    handled = e.Handled;
#if HAX
                    if (keyData == Keys.LShiftKey || keyData == Keys.RShiftKey)
                        _is_shift = true;
                    if (keyData == Keys.CapsLock)
                    {
                        if (GetKeyState(VK_CAPITAL) != 0)
                            _is_caps = false;
                        else
                            _is_caps = true;
                    }
#endif
                }

                // raise KeyPress
                if (s_KeyPress != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.VirtualKeyCode;

                    if (keyData == Keys.LMenu || keyData == Keys.RMenu ||
                        keyData == Keys.LControlKey || keyData == Keys.RControlKey ||
                        keyData == Keys.LShiftKey || keyData == Keys.RShiftKey ||
                        keyData == Keys.LWin || keyData == Keys.RWin ||
                        keyData == Keys.CapsLock || keyData == Keys.Space ||
                        keyData == Keys.Up || keyData == Keys.Down ||
                        keyData == Keys.Left || keyData == Keys.Right ||
                        keyData == Keys.Home || keyData == Keys.End ||
                        keyData == Keys.PageUp || keyData == Keys.PageDown)
                    {
                        char key_char = char.MinValue;
                        if (keyData == Keys.Space || keyData == Keys.Up || keyData == Keys.Down ||
                            keyData == Keys.Left || keyData == Keys.Right ||
                            keyData == Keys.Home || keyData == Keys.End ||
                            keyData == Keys.PageUp || keyData == Keys.PageDown)
                        {
                            if (keyData == Keys.Space)
                                key_char = ' ';
                            KeyPressExEventArgs e = new KeyPressExEventArgs(key_char, keyData);
                            s_KeyPress.Invoke(null, e);
                            handled = handled || e.Handled;
                            _it_was_a_dead_key = false;
                        }
                    }
                    else
                    {
#if !HAX
                        bool isDownShift = ((GetKeyState(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                        bool isDownCapslock = (GetKeyState(VK_CAPITAL) != 0 ? true : false);
#else
                        bool isDownShift = _is_shift;
                        bool isDownCapslock = _is_caps;
#endif

                        //System.IO.StreamWriter writer1 = new System.IO.StreamWriter("key_dump.txt", true);
                        //writer1.WriteLine("Key pressed: " + ((Keys)MyKeyboardHookStruct.VirtualKeyCode).ToString() + "; Scan code: " + MyKeyboardHookStruct.ScanCode.ToString() + "; s_KeyPress = " + s_KeyPress.ToString() + "; wParam = " + wParam.ToString());
                        //writer1.WriteLine("IsShift = " + _is_shift.ToString() + "; IsCaps = " + _is_caps.ToString());
                        //writer1.Close();

                        byte[] keyState = new byte[256];
                        GetKeyboardState(keyState);
                        byte[] inBuffer = new byte[2];
                        System.Text.StringBuilder sbString = new System.Text.StringBuilder();
                        IntPtr HKL = GetKeyboardLayout(0);
                        //if (ToAscii(MyKeyboardHookStruct.VirtualKeyCode,
                        //          MyKeyboardHookStruct.ScanCode,
                        //          keyState,
                        //          inBuffer,
                        //          MyKeyboardHookStruct.Flags) == 1)
                        //{
                        //    char key = (char)inBuffer[0];
                        //    if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                        //    KeyPressEventArgs e = new KeyPressEventArgs(key);
                        //    s_KeyPress.Invoke(null, e);
                        //    handled = handled || e.Handled;
                        //}

                        if (!IsDeadKey((uint)MyKeyboardHookStruct.VirtualKeyCode))
                        {
                            if (!_it_was_a_dead_key)
                            {
                                switch (ToUnicodeEx((uint)MyKeyboardHookStruct.VirtualKeyCode,
                                    (uint)MyKeyboardHookStruct.ScanCode,
                                    keyState,
                                    sbString,
                                    5,
                                    (uint)MyKeyboardHookStruct.Flags,
                                    HKL))
                                {
                                    case 1:
                                        {
                                            char key_char = sbString.ToString()[0];
                                            //System.IO.StreamWriter writer2 = new System.IO.StreamWriter("ToUnicode_dump.txt", true);
                                            //writer2.WriteLine("char: " + key_char.ToString());
                                            //writer2.Close();
                                            if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key_char))
                                                key_char = Char.ToUpper(key_char);
                                            else if (Char.IsUpper(key_char))
                                                key_char = Char.ToLower(key_char);
                                            KeyPressExEventArgs e = new KeyPressExEventArgs(key_char, (Keys)MyKeyboardHookStruct.VirtualKeyCode);
                                            s_KeyPress.Invoke(null, e);
                                            handled = handled || e.Handled;
                                            _it_was_a_dead_key = false;
                                        }
                                        break;
                                    case 0:
                                        {
                                            Keys key = (Keys)MyKeyboardHookStruct.VirtualKeyCode;
                                            if (key != Keys.LControlKey && key != Keys.RControlKey &&
                                                key != Keys.LShiftKey && key != Keys.RShiftKey &&
                                                key != Keys.LMenu && key != Keys.RMenu &&
                                                key != Keys.CapsLock)
                                            {
                                                char key_char = char.MinValue;
                                                KeyPressExEventArgs e = new KeyPressExEventArgs(key_char, (Keys)MyKeyboardHookStruct.VirtualKeyCode);
                                                s_KeyPress.Invoke(null, e);
                                                handled = handled || e.Handled;
                                                //System.IO.StreamWriter writer2 = new System.IO.StreamWriter("ToUnicode_dump.txt", true);
                                                //writer2.WriteLine("char: " + key.ToString());
                                                //writer2.Close();
                                                _it_was_a_dead_key = false;
                                            }
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                uint vkey = MapVirtualKey((uint)MyKeyboardHookStruct.VirtualKeyCode, 2);
                                char key_char = char.MinValue;
                                try
                                {
                                    key_char = Convert.ToChar(vkey);
                                }
                                catch
                                {

                                }
                                if (key_char != char.MinValue)
                                {
                                    if (!(isDownCapslock ^ isDownShift) && Char.IsLetter(key_char)) key_char = Char.ToLower(key_char);
                                    KeyPressExEventArgs e = new KeyPressExEventArgs(key_char, (Keys)MyKeyboardHookStruct.VirtualKeyCode);
                                    s_KeyPress.Invoke(null, e);
                                    handled = handled || e.Handled;
                                    _it_was_a_dead_key = false;
                                }
                            }
                        }
                        else
                        {
                            _it_was_a_dead_key = true;
                            _last_dead_key_code = MyKeyboardHookStruct.VirtualKeyCode;
                        }
                    }
                }

                // raise KeyUp
                if (s_KeyUp != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.VirtualKeyCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    s_KeyUp.Invoke(null, e);
                    handled = handled || e.Handled;
#if HAX
                    if (keyData == Keys.LShiftKey || keyData == Keys.RShiftKey)
                        _is_shift = false;
#endif

                    //bool isDownShift = ((GetKeyState(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                    //bool isDownCapslock = (GetKeyState(VK_CAPITAL) != 0 ? true : false);

                    //byte[] keyState = new byte[256];
                    //GetKeyboardState(keyState);
                    //byte[] inBuffer = new byte[2];
                    //System.Text.StringBuilder sbString = new System.Text.StringBuilder();
                    //IntPtr HKL = GetKeyboardLayout(0);

                    //if (!IsDeadKey((uint)MyKeyboardHookStruct.VirtualKeyCode))
                    //{
                    //    if (!_it_was_a_dead_key)
                    //    {
                    //        switch (ToUnicodeEx((uint)MyKeyboardHookStruct.VirtualKeyCode,
                    //            (uint)MyKeyboardHookStruct.ScanCode,
                    //            keyState,
                    //            sbString,
                    //            5,
                    //            (uint)MyKeyboardHookStruct.Flags,
                    //            HKL))
                    //        {
                    //            case 1:
                    //                {
                    //                    char key_char = sbString.ToString()[0];
                    //                    if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key_char))
                    //                        key_char = Char.ToUpper(key_char);
                    //                    else if (Char.IsUpper(key_char))
                    //                        key_char = Char.ToLower(key_char);
                    //                    KeyEventExArgs e = new KeyEventExArgs(keyData, key_char);
                    //                    s_KeyUp.Invoke(null, e);
                    //                    handled = handled || e.Handled;
                    //                    _it_was_a_dead_key = false;
                    //                }
                    //                break;
                    //            case 0:
                    //                {
                    //                    //if (key != Keys.LControlKey && key != Keys.RControlKey &&
                    //                    //    key != Keys.LShiftKey && key != Keys.RShiftKey &&
                    //                    //    key != Keys.LMenu && key != Keys.RMenu &&
                    //                    //    key != Keys.CapsLock)
                    //                    //{
                    //                    //    char key_char = char.MinValue;
                    //                    //    KeyEventExArgs e = new KeyEventExArgs(keyData, key_char);
                    //                    //    s_KeyUp.Invoke(null, e);
                    //                    //    handled = handled || e.Handled;
                    //                    //    _it_was_a_dead_key = false;
                    //                    //}
                    //                    char key_char = char.MinValue;
                    //                    KeyEventExArgs e = new KeyEventExArgs(keyData, key_char);
                    //                    s_KeyUp.Invoke(null, e);
                    //                    handled = handled || e.Handled;
                    //                    _it_was_a_dead_key = false;
                    //                }
                    //                break;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        uint vkey = MapVirtualKey((uint)MyKeyboardHookStruct.VirtualKeyCode, 2);
                    //        char key_char = char.MinValue;
                    //        try
                    //        {
                    //            key_char = Convert.ToChar(vkey);
                    //        }
                    //        catch
                    //        {

                    //        }
                    //        if (key_char != char.MinValue)
                    //        {
                    //            if (!(isDownCapslock ^ isDownShift) && Char.IsLetter(key_char)) key_char = Char.ToLower(key_char);
                    //            KeyEventExArgs e = new KeyEventExArgs(keyData, key_char);
                    //            s_KeyUp.Invoke(null, e);
                    //            handled = handled || e.Handled;
                    //            _it_was_a_dead_key = false;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    //System.IO.StreamWriter writer1 = new System.IO.StreamWriter("key_dump.txt", true);
                    //    //writer1.WriteLine("Key pressed: " + ((Keys)MyKeyboardHookStruct.VirtualKeyCode).ToString() + "; last key: " + ((Keys)_last_dead_key_code).ToString() + "; it was a dead key: " + _it_was_a_dead_key.ToString());
                    //    //writer1.Close();
                    //    //if (_it_was_a_dead_key && _last_dead_key_code == MyKeyboardHookStruct.VirtualKeyCode)
                    //    //{
                    //    //    uint vkey = MapVirtualKey((uint)MyKeyboardHookStruct.VirtualKeyCode, 2);
                    //    //    char key = Convert.ToChar(vkey);
                    //    //    if (!(isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToLower(key);
                    //    //    KeyPressExEventArgs e = new KeyPressExEventArgs(key, (Keys)MyKeyboardHookStruct.VirtualKeyCode);
                    //    //    s_KeyPress.Invoke(null, e);
                    //    //    handled = handled || e.Handled;
                    //    //    System.IO.StreamWriter writer2 = new System.IO.StreamWriter("ToUnicode_dump.txt", true);
                    //    //    writer2.WriteLine("char: " + key.ToString());
                    //    //    writer2.Close();
                    //    //    _it_was_a_dead_key = false;
                    //    //}
                    //    //else
                    //    //{
                    //    _it_was_a_dead_key = true;
                    //    _last_dead_key_code = MyKeyboardHookStruct.VirtualKeyCode;
                    //    //}
                    //}
                }

            }

            //if event handled in application do not handoff to other listeners
            if (handled)
                return -1;

            //forward to other application
            return CallNextHookEx(s_KeyboardHookHandle, nCode, wParam, lParam);
        }

        static bool _it_was_a_dead_key = false;
        static int _last_dead_key_code;

        static bool IsDeadKey(uint key)
        {
            if ((MapVirtualKey(key, 2) & 2147483648) == 2147483648)
            {
                //MessageBox.Show("It's a deadkey: " + ((char)MapVirtualKey(key, 2)).ToString());
                return true;
            }
            else
            {
                //MessageBox.Show("It's not a deadkey: " + ((char)MapVirtualKey(key, 2)).ToString());
                return false;
            }
        }

        private static void ClearKeyboardBuffer(uint vk, uint sc, IntPtr hkl)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(10);
            byte[] keyState = new byte[256];
            int rc = 0;
            do
            {
                rc = ToUnicodeEx(vk, sc, keyState, sb, sb.Capacity, 0, hkl);
            } while (rc < 0);
        }

        private static void ClearKeyboardBuffer(uint vk, uint sc, byte[] keyState, IntPtr hkl)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(10);
            int rc = 0;
            do
            {
                rc = ToUnicodeEx(vk, sc, keyState, sb, sb.Capacity, 0, hkl);
            } while (rc < 0);
        } 

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[]
           lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder pwszBuff,
           int cchBuff, uint wFlags, IntPtr dwhkl);

        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        static extern bool SetKeyboardState(byte[] lpKeyState);

        private static void EnsureSubscribedToGlobalKeyboardEvents()
        {
            // install Keyboard hook only if it is not installed and must be installed
            if (s_KeyboardHookHandle == 0)
            {
                //See comment of this field. To avoid GC to clean it up.
                s_KeyboardDelegate = KeyboardHookProc;
                //install hook
                s_KeyboardHookHandle = SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    s_KeyboardDelegate,
                    Marshal.GetHINSTANCE(
                        Assembly.GetExecutingAssembly().GetModules()[0]),
                    0);
                //If SetWindowsHookEx fails.
                if (s_KeyboardHookHandle == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup

                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private static void TryUnsubscribeFromGlobalKeyboardEvents()
        {
            //if no subsribers are registered unsubsribe from hook
            if (s_KeyDown == null &&
                s_KeyUp == null &&
                s_KeyPress == null)
            {
                ForceUnsunscribeFromGlobalKeyboardEvents();
            }
        }

        private static void ForceUnsunscribeFromGlobalKeyboardEvents()
        {
            if (s_KeyboardHookHandle != 0)
            {
                //uninstall hook
                int result = UnhookWindowsHookEx(s_KeyboardHookHandle);
                //reset invalid handle
                s_KeyboardHookHandle = 0;
                //Free up for GC
                s_KeyboardDelegate = null;
                //if failed and exception must be thrown
                if (result == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        #endregion

    }
}
