if (!(Test-Path "nuget\nuget.exe")) {
	New-Item -ItemType Directory -Force -Path nuget
	Invoke-WebRequest -Uri https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile nuget\nuget.exe
}

.\nuget\nuget.exe install OpenCover -Version 4.6.519 -OutputDirectory packages
.\nuget\nuget.exe install ReportGenerator -Version 4.0.4 -OutputDirectory packages

dotnet build DexTranslate.sln
New-Item -ItemType Directory -Force -Path coverage
packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:"c:\Program Files\dotnet\dotnet.exe" -targetargs:"test" -filter:"+[DexTranslate.*]* -[*Fixtures]*" -oldStyle -register:user -output:coverage\opencover.xml
packages\ReportGenerator.4.0.4\tools\net47\ReportGenerator.exe -reports:coverage\OpenCover.xml -targetdir:coverage\report