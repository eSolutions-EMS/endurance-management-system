rm -rf ../build
echo 'Cleared previous package build'

dotnet build Not.Analyzers/Not.Analyzers.csproj -o ../build
