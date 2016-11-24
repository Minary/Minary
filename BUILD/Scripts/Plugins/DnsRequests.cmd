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
ECHO Copying DNS Requests Plugin files ...

SET DNSREQUESTS_ROOT=%ROOT_DIR%Plugins\Plugin_DnsRequests\bin\%SOLUTIONCONFIG%\
SET DNSREQUESTS_DSTDIR=%PLUGIN_DIR%Plugin_DnsRequests\

MKDIR %DNSREQUESTS_DSTDIR%
ECHO COPY %DNSREQUESTS_ROOT%Plugin_DnsRequests.dll %DNSREQUESTS_DSTDIR%
COPY %DNSREQUESTS_ROOT%Plugin_DnsRequests.dll %DNSREQUESTS_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
