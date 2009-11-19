:: Dont forget to add WinRar to path
@echo off
set path="C:\Program Files\WinRAR\";%path%
rar a .\Automator.rar Utilities Plugins User Skins Icons Automator.exe *.dll config.bat
