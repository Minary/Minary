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
ECHO Copying IP Accounting Plugin files ...

SET IPACCOUNTING_ROOT=%ROOT_DIR%Plugins\Plugin_IpAccounting\bin\%SOLUTIONCONFIG%\
SET IPACCOUNTING_DEFAULTDATA_ROOT=%ROOT_DIR%Plugins\Plugin_IpAccounting\DEFAULT_DATA\
SET IPACCOUNTING_DSTDIR=%PLUGIN_DIR%\Plugin_IpAccounting\
SET APPLICATION_IPACCOUNTING_ROOT=%ROOT_DIR%Tools\IpAccounting\%SOLUTIONCONFIG%\

MKDIR %IPACCOUNTING_DSTDIR%

ECHO COPY %IPACCOUNTING_ROOT%Plugin_IpAccounting.dll %IPACCOUNTING_DSTDIR%
COPY %IPACCOUNTING_ROOT%Plugin_IpAccounting.dll %IPACCOUNTING_DSTDIR% >>%OUTPUTFILE%

ECHO COPY %IPACCOUNTING_DEFAULTDATA_ROOT%Service_Definitions.txt %IPACCOUNTING_DSTDIR%
COPY %IPACCOUNTING_DEFAULTDATA_ROOT%Service_Definitions.txt %IPACCOUNTING_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
