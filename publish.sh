while getopts "v:" option; do
  case $option in
    v) branch_version=$OPTARG;;
    \?) echo "Error: Invalid option"
      exit;;
   esac
done

dev_branch=$(git branch --show-current)
echo "push to dev remote '$dev_branch'"
git push
if [ $? -gt 0 ]; then
  exit 1
fi

rel_branch="rel/$branch_version"
echo "sync with release branch '$rel_branch'"
git checkout "$rel_branch"
if [ $? -gt 0 ]; then
  exit 1
fi

git rebase "$dev_branch"
if [ $? -gt 0 ]; then
  exit 1
fi

echo "push to rel remote '$rel_branch'"
output_dir="endurance-judge-$branch_version"

echo "clearing release directory..."
rm -rf "$output_dir"
mkdir "$output_dir"
cd "$output_dir"

echo "release directory cleared."
echo "======================================================================================================================================="
echo "DOTNET PUBLISH"
dotnet publish -c Release -o . ../src/EnduranceJudge.sln
if [ $? -gt 0 ]; then
  exit 1
fi

echo "======================================================================================================================================="
echo "copying Views directory..."
mkdir ./Views
mkdir ./Views/Templates
cp -r ../src/Gateways/Desktop/Views/Templates/* ./Views/Templates

echo "Views directory copied."
echo "======================================================================================================================================="
echo "opening explorer..."
mkdir logs
touch logs/witness-events.log
touch logs/witness-event-errors.log
cd ..
explorer "$output_dir"
echo "publish complete."
