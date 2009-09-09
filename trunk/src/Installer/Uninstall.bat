@echo off
title Uninstall Blaze
msiexec /x {428D6F5A-468F-4C48-9E0C-365CB5AC65E9}
:loop
echo Whould you also like to erase you configuration file? Keeping it will allow Blaze to detect your settings next time it is installed.
set Choice=
set /P Choice=Erase the file? (y/n)
if not '%Choice%'=='' set Choice=%Choice:~0,1%
echo.

if /I '%Choice%'=='y' goto erase
if /I '%Choice%'=='Y' goto erase
if /I '%Choice%'=='n' goto donterase
if /I '%Choice%'=='N' goto donterase
echo "%Choice%" is not valid. Please try again.
echo.
goto loop

:erase
if exist User (
	del User\*
	rd User
	)
if exist InstallLib.InstallState del InstallLib.InstallState
goto end

:donterase
goto end

:end
echo All done!
pause