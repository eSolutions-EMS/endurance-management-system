cd release
echo clearing release directory...
rm -rf *
echo release directory cleared.
echo =======================================================================================================================================
echo DOTNET PUBLISH
dotnet publish -c Release -o . ../src
echo =======================================================================================================================================
echo copying Views...
mkdir ./Views
mkdir ./Views/Templates
cp -r ../src/Gateways/Desktop/Views/Templates/* ./Views/Templates
echo Views directory copied.
echo =======================================================================================================================================
echo opening explorer...
mkdir logs
touch logs/witness-events.log
touch logs/witness-event-errors.log
cd ..
explorer release
echo publish complete.
