echo clearing release directory...
rm -rf ./release/*
echo release directory cleared.
echo =======================================================================================================================================
echo DOTNET PUBLISH
dotnet publish -c Release -o ./release src
echo =======================================================================================================================================
echo copying Views...
mkdir ./release/Views
mkdir ./release/Views/Templates
cp -r src/Gateways/Desktop/Views/Templates/* ./release/Views/Templates
echo Views directory copied.
echo =======================================================================================================================================
echo opening explorer...
explorer release
echo publish complete.
