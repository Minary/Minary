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
ECHO Copying SslStrip Plugin files ...

SET PLUGIN_SSLSTRIP_ROOT=%ROOT_DIR%Plugins\Plugin_SslStrip\bin\%SOLUTIONCONFIG%\
SET PLUGIN_SSLSTRIP_DSTDIR=%PLUGIN_DIR%\Plugin_SslStrip\

MKDIR %PLUGIN_SSLSTRIP_DSTDIR%
ECHO COPY %PLUGIN_SSLSTRIP_ROOT%Plugin_SslStrip.dll %PLUGIN_SSLSTRIP_DSTDIR%
COPY %PLUGIN_SSLSTRIP_ROOT%Plugin_SslStrip.dll %PLUGIN_SSLSTRIP_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
