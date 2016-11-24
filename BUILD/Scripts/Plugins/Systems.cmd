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
ECHO Copying Systems Plugin files ...

SET SYSTEMS_ROOT=%ROOT_DIR%Plugins\Plugin_Systems\bin\%SOLUTIONCONFIG%\
SET SYSTEMS_DEFAULTDATA_ROOT=%ROOT_DIR%Plugins\Plugin_Systems\DEFAULT_DATA\
SET SYSTEMS_DSTDIR=%PLUGIN_DIR%Plugin_Systems\

MKDIR %SYSTEMS_DSTDIR%
ECHO COPY %SYSTEMS_ROOT%Plugin_Systems.dll %SYSTEMS_DSTDIR%
COPY %SYSTEMS_ROOT%Plugin_Systems.dll %SYSTEMS_DSTDIR% >>%OUTPUTFILE%

ECHO COPY %SYSTEMS_ROOT%app.config %SYSTEMS_DSTDIR%
COPY %SYSTEMS_ROOT%app.config %SYSTEMS_DSTDIR% >>%OUTPUTFILE%

ECHO COPY %SYSTEMS_DEFAULTDATA_ROOT%MacVendors.txt %SYSTEMS_DSTDIR%
COPY %SYSTEMS_DEFAULTDATA_ROOT%MacVendors.txt %SYSTEMS_DSTDIR% >>%OUTPUTFILE%

EXIT /B 0
