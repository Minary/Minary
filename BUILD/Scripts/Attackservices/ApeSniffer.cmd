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
ECHO Copying ApeSniffer ...

SET ROOT_DIR=%1
SET ATTACKSERVICES_DIR=%2
SET APESNIFFER_DSTDIR=%ATTACKSERVICES_DIR%\ApeSniffer\
SET APESNIFFER_ROOT=%ROOT_DIR%Tools\ApeSniffer\%SOLUTIONCONFIG%\

MKDIR %APESNIFFER_DSTDIR%
ECHO COPY %APESNIFFER_ROOT%bin\ApeSniffer.exe %APESNIFFER_DSTDIR%
COPY %APESNIFFER_ROOT%bin\ApeSniffer.exe %APESNIFFER_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
