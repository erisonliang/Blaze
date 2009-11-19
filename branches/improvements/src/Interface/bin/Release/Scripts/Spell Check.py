import sys
import clr
import spellcheck
import System
##import atexit
##clr.AddReference("System");
clr.AddReference("ContextLib")
from ContextLib import *

len = len(sys.argv)
args = ''

if len > 0:
	for index, arg in enumerate(sys.argv):
		args += arg
		if index < len-1:
			args += ' '

# if no arguments were found, lets use user's context selected content
if args == '':
	data = _user_context.GetSelectedContent()
	if data.Text != '':
		args = data.Text
	data.Dispose()

if args != '':
	if not spellcheck.check_word(args):
		spellcheck.correct_word(args)
	else:
		System.Windows.Forms.MessageBox.Show("Your spelling is correct!", "Spell check", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
else:
	System.Windows.Forms.MessageBox.Show("You must specify something for spell checking!", "Spell check", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

## This should work, however, MS Word won't start again
#atexit._run_exitfuncs()