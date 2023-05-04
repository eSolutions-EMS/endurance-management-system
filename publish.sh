while getopts "lv:" option; do
  case $option in
    l) remote=false;;
    v) version=$OPTARG;;
    \?) echo "Error: Invalid option"
      exit;;
   esac
done

branch="dev/main"
if ! git checkout $branch
then 
  exit 1
fi

if [ $remote ]; then
  if ! git pull
  then
   exit 1
  fi
fi
 
echo "bumping version"
settings_path=src/Gateways/Desktop/settings.json
cat $settings_path | sed -r "s/([0-9]+\.[0-9]+\.[0-9]+)/$version/" > temp && mv temp $settings_path
git add .
git commit -m "Set Judge version to $version"
  
if [ $remote ]; then
  echo "pushing to remote '$branch'"
  if ! git push
  then
    exit 1
  fi
fi

echo "clearing release directory..."
output_dir="endurance-judge-$version"
rm -rf "$output_dir"
mkdir "$output_dir"
cd "$output_dir"

echo "release directory cleared."
echo "======================================================================================================================================="
echo "DOTNET PUBLISH"
if ! dotnet publish -c Release -o . ../src/EnduranceJudge.sln
then
  exit 1
fi

echo "======================================================================================================================================="
echo "opening explorer..."
mkdir logs
touch logs/witness-events.log
touch logs/witness-event-errors.log
cd ..
explorer "$output_dir"
echo "publish complete."
