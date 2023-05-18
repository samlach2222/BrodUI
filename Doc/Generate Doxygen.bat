@ECHO OFF
cd "%~dp0"
"C:\Program Files\doxygen\bin\doxygen" Doxyfile
echo.

TIMEOUT 8
