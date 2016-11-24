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
ECHO Copying Inject payload Plugin files ...

SET INJECTPAYLOAD_ROOT=%ROOT_DIR%Plugins\Plugin_InjectPayload\bin\%SOLUTIONCONFIG%\
SET INJECTPAYLOAD_DSTDIR=%PLUGIN_DIR%\Plugin_InjectPayload\

MKDIR %INJECTPAYLOAD_DSTDIR%
ECHO COPY %INJECTPAYLOAD_ROOT%Plugin_InjectPayload.dll %INJECTPAYLOAD_DSTDIR%
COPY %INJECTPAYLOAD_ROOT%Plugin_InjectPayload.dll %INJECTPAYLOAD_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
