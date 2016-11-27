:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Global variables
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

CALL %* 
EXIT /B


:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Global functions
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

:CopyAttackServiceFiles

ECHO[
ECHO Copying ArpScan ...

SET ROOT_DIR=%1
SET ATTACKSERVICES_DIR=%2
SET ARPSCAN_DSTDIR=%ATTACKSERVICES_DIR%ArpScan\
SET ARPSCAN_ROOT=%ROOT_DIR%Tools\ArpScan\bin\%SOLUTIONCONFIG%\

MKDIR %ARPSCAN_DSTDIR%
ECHO COPY %ARPSCAN_ROOT%ArpScan.exe %ARPSCAN_DSTDIR% 
COPY %ARPSCAN_ROOT%ArpScan.exe %ARPSCAN_DSTDIR% 

ECHO COPY %ARPSCAN_ROOT%*.dll %ARPSCAN_DSTDIR% 
COPY %ARPSCAN_ROOT%*.dll %ARPSCAN_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
