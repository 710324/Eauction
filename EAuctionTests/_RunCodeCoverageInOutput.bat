dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura

%USERPROFILE%\.nuget\packages\reportgenerator\5.1.9\tools\net5.0\reportgenerator.exe "-reports:%cd%\coverage.cobertura.xml" "-targetdir:%cd%\html" -reporttypes:HTML
\
