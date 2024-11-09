rm -rf ../build
echo 'Cleared previous package build'

cd ../..
find . -name bin -type d -exec rm -rf {} +
find . -name obj -type d -exec rm -rf {} +
find . -name .vs -type d -exec rm -rf {} +
find . -name .vscode -type d -exec rm -rf {} +
echo 'Cleared all build artifacts'

cd -
dotnet build Not.Analyzers/Not.Analyzers.csproj -c Release -o ../build
