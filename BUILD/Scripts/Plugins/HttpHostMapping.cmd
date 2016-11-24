:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Global variables
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


CALL %* 
EXIT /B


:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Global functions
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

:CopyPluginFiles

ECHO[
ECHO Copying HTTP host mapping Plugin files ...

SET HTTPHOSTMAPPING_ROOT=%ROOT_DIR%Plugins\Plugin_HttpHostMapping\bin\%SOLUTIONCONFIG%\
SET HTTPHOSTMAPPING_DSTDIR=%PLUGIN_DIR%Plugin_HttpHostMapping\

MKDIR %HTTPHOSTMAPPING_DSTDIR%
ECHO COPY %HTTPHOSTMAPPING_ROOT%Plugin_HttpHostMapping.dll %HTTPHOSTMAPPING_DSTDIR%
COPY %HTTPHOSTMAPPING_ROOT%Plugin_HttpHostMapping.dll %HTTPHOSTMAPPING_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
