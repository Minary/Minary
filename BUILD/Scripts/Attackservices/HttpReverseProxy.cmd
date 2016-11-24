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
ECHO Copying HttpReverseProxy(lib) ...

SET ROOT_DIR=%1
SET ATTACKSERVICES_DIR=%2
SET HTTPREVERSEPROXY_DSTDIR=%ATTACKSERVICES_DIR%HttpReverseProxy\
SET HTTPREVERSEPROXY_ROOT=%ROOT_DIR%Tools\HttpReverseProxy\HttpReverseProxy\bin\%SOLUTIONCONFIG%\
SET PLUGINS_DSTDIR=%HTTPREVERSEPROXY_DSTDIR%plugins\

SET SSLSTRIP_ROOT=%ROOT_DIR%Tools\HttpReverseProxy\SSLStrip\bin\%SOLUTIONCONFIG%\
SET SSLSTRIP_DSTDIR=%HTTPREVERSEPROXY_DSTDIR%plugins\SslStrip

SET CLIENTREQUESTSNIFFER_ROOT=%ROOT_DIR%Tools\HttpReverseProxy\ClientRequestSniffer\bin\%SOLUTIONCONFIG%\
SET CLIENTREQUESTSNIFFER_DSTDIR=%HTTPREVERSEPROXY_DSTDIR%plugins\ClientRequestSniffer

SET WEAKEN_ROOT=%ROOT_DIR%Tools\HttpReverseProxy\Weaken\bin\%SOLUTIONCONFIG%\
SET WEAKEN_DSTDIR=%HTTPREVERSEPROXY_DSTDIR%plugins\Weaken

SET INJECT_ROOT=%ROOT_DIR%Tools\HttpReverseProxy\Inject\bin\%SOLUTIONCONFIG%\
SET INJECT_DSTDIR=%HTTPREVERSEPROXY_DSTDIR%plugins\inject

SET HOSTMAPPING_ROOT=%ROOT_DIR%Tools\HttpReverseProxy\HostMapping\bin\%SOLUTIONCONFIG%\
SET HOSTMAPPING_DSTDIR=%HTTPREVERSEPROXY_DSTDIR%plugins\HostMapping


MKDIR %HTTPREVERSEPROXY_DSTDIR%
MKDIR %PLUGINS_DSTDIR%
MKDIR %SSLSTRIP_DSTDIR%
MKDIR %CLIENTREQUESTSNIFFER_DSTDIR%
MKDIR %WEAKEN_DSTDIR%
MKDIR %INJECT_DSTDIR%
MKDIR %HOSTMAPPING_DSTDIR%

ECHO COPY %HTTPREVERSEPROXY_ROOT%HttpReverseProxy.exe %HTTPREVERSEPROXY_DSTDIR%
COPY %HTTPREVERSEPROXY_ROOT%HttpReverseProxy.exe %HTTPREVERSEPROXY_DSTDIR% >>%OUTPUTFILE%

ECHO COPY %HTTPREVERSEPROXY_ROOT%HttpReverseProxyLib.dll %HTTPREVERSEPROXY_DSTDIR% 
COPY %HTTPREVERSEPROXY_ROOT%HttpReverseProxyLib.dll %HTTPREVERSEPROXY_DSTDIR% >>%OUTPUTFILE%

ECHO COPY %HTTPREVERSEPROXY_ROOT%NConsoler.dll %HTTPREVERSEPROXY_DSTDIR%
COPY %HTTPREVERSEPROXY_ROOT%NConsoler.dll %HTTPREVERSEPROXY_DSTDIR% >>%OUTPUTFILE%

ECHO COPY %SSLSTRIP_ROOT%SSLStrip.dll %SSLSTRIP_DSTDIR%\ 
COPY %SSLSTRIP_ROOT%SSLStrip.dll %SSLSTRIP_DSTDIR%\ >>%OUTPUTFILE%

ECHO COPY %CLIENTREQUESTSNIFFER_ROOT%ClientRequestSniffer.dll %CLIENTREQUESTSNIFFER_DSTDIR%
COPY %CLIENTREQUESTSNIFFER_ROOT%ClientRequestSniffer.dll %CLIENTREQUESTSNIFFER_DSTDIR% >>%OUTPUTFILE%

ECHO COPY %WEAKEN_ROOT%Weaken.dll %WEAKEN_DSTDIR%
COPY %WEAKEN_ROOT%Weaken.dll %WEAKEN_DSTDIR% >>%OUTPUTFILE%

ECHO COPY %INJECT_ROOT%Inject.dll %INJECT_DSTDIR%
COPY %INJECT_ROOT%Inject.dll %INJECT_DSTDIR% >>%OUTPUTFILE%

ECHO COPY %HOSTMAPPING_ROOT%Inject.dll %HOSTMAPPING_DSTDIR% 
COPY %HOSTMAPPING_ROOT%HostMapping.dll %HOSTMAPPING_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
