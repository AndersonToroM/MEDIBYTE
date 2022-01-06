cd /home/netcore/publish
chmod -R 777 ./siisoPublish
cd ./siisoPublish
cp appsettings.prod.json appsettings.json
cd ./Utils
echo "{\"VersionApp\":\"$(date '+%Y%m%d')\",\"ParcheApp\":\"$(date '+%H%M')\"}" > infoApp.json