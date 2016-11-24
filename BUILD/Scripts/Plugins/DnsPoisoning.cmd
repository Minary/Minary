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
ECHO Copying DNS Poisoning Plugin files ...

SET DNSPOISONING_ROOT=%ROOT_DIR%Plugins\Plugin_DnsPoisoning\bin\%SOLUTIONCONFIG%\
SET DNSPOISONING_DSTDIR=%PLUGIN_DIR%Plugin_DnsPoisoning\

MKDIR %DNSPOISONING_DSTDIR%
ECHO COPY %DNSPOISONING_ROOT%Plugin_DnsPoisoning.dll %DNSPOISONING_DSTDIR%
COPY %DNSPOISONING_ROOT%Plugin_DnsPoisoning.dll %DNSPOISONING_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
