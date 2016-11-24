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
ECHO Copying HTTP accounts Plugin files ...

SET HTTPACCOUNTS_ROOT=%ROOT_DIR%Plugins\Plugin_HttpAccounts\bin\%SOLUTIONCONFIG%\
SET HTTPACCOUNTS_DSTDIR=%PLUGIN_DIR%Plugin_HttpAccounts\

MKDIR %HTTPACCOUNTS_DSTDIR%
ECHO COPY %HTTPACCOUNTS_ROOT%Plugin_HttpAccounts.dll %HTTPACCOUNTS_DSTDIR%
COPY %HTTPACCOUNTS_ROOT%Plugin_HttpAccounts.dll %HTTPACCOUNTS_DSTDIR% >>%OUTPUTFILE%

ECHO COPY %HTTPACCOUNTS_ROOT%app.config %HTTPACCOUNTS_DSTDIR%
COPY %HTTPACCOUNTS_ROOT%app.config %HTTPACCOUNTS_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
