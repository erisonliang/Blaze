using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;

namespace SystemCore.SystemAbstraction.WindowManagement.KnownWindows
{
    public class NotepadWindow : VWindow
    {
        #region Constructors
        public NotepadWindow(IntPtr handle, int zorder)
            : base(handle, zorder)
        {

        }
        #endregion

        #region Public Methods
        public override string GetText()
        {
            AutomationElement edit = GetNotepadEdit(node);

            TextPattern pattern = edit.GetCurrentPattern(TextPattern.Pattern) as TextPattern;
            string ret = string.Empty;
            if (pattern != null)
            {
                TextPatternRange[] ranges = pattern.GetVisibleRanges();
                foreach (TextPatternRange range in ranges)
                {
                    ret += range.GetText(-1);
                }
            }
            return ret;
        }

        public override string GetSelectedText()
        {
            AutomationElement edit = GetNotepadEdit(node);
            TextPattern pattern = edit.GetCurrentPattern(TextPattern.Pattern) as TextPattern;
            string ret = string.Empty;
            if (pattern != null)
            {
                TextPatternRange[] ranges = pattern.GetSelection();
                foreach (TextPatternRange range in ranges)
                {
                    ret += range.GetText(-1);
                }
            }
            return ret;
        }

        public override string GetContent()
        {
            return GetText();
        }

        public override string GetSelectedContet()
        {
            return GetSelectedText();
        }
        #endregion

        #region Private Methods        private AutomationElement GetNotepadEdit(AutomationElement node)
        {
            Condition condition = new AndCondition(
                                    new PropertyCondition(AutomationElement.ClassNameProperty, "Edit"),
                                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
            AutomationElement edit = node.FindFirst(TreeScope.Descendants, condition);
            return edit;
        }
        #endregion
    }
}
