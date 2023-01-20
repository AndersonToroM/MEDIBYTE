cd ./publish
cp appsettings.prod.json appsettings.json
cd ./Utils
echo "{\"VersionApp\":\"$(date '+%Y%m%d')\",\"ParcheApp\":\"$(date '+%H%M')\"}" > infoApp.json
