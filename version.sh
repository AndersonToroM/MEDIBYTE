cd /home/cloudonesoftTest
chmod -R 777 ./siisoPublish
cd ./siisoapp
cp appsettings.dev.json appsettings.json
cd ./Utils
echo "{\"VersionApp\":\"$(date '+%Y%m%d')\",\"ParcheApp\":\"$(date '+%H%M')\"}" > infoApp.json
