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
ECHO Copying HTTP Requests Plugin files ...

SET HTTPREQUESTS_ROOT=%ROOT_DIR%Plugins\Plugin_HttpRequests\bin\%SOLUTIONCONFIG%\
SET HTTPREQUESTS_DSTDIR=%PLUGIN_DIR%Plugin_HttpRequests\

MKDIR %HTTPREQUESTS_DSTDIR%
ECHO COPY %HTTPREQUESTS_ROOT%Plugin_HttpRequests.dll %HTTPREQUESTS_DSTDIR%
COPY %HTTPREQUESTS_ROOT%Plugin_HttpRequests.dll %HTTPREQUESTS_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
