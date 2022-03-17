gitPull:
	git pull --progress -v --no-rebase "origin" && \
	echo ******* REPOSITORIO SIISO ACTUALIZADO  *******
	
publicar:
	cd ./WebApp && \
	dotnet restore --force -s https://api.nuget.org/v3/index.json -s https://nexus.dbusiness.app/repository/nuget-group/ -s https://nuget.devexpress.com/nYVMfL2DHjdhvBeIVCLpdfJqwAjciBDPXo836DK6lFsCbB5gdz/api && \
	echo ******* PROYECTO SIISO RESTAURADO  ******* && \
	dotnet build && \
	echo ******* PROYECTO SIISO COMPILADO  ******* && \
	dotnet publish -r linux-x64 -c Release --self-contained true -p:PublishSingleFile=false -o ../publish && \
	echo ******* PROYECTO SIISO PUBLICADO PARA LINUX X64  *******

setVersion:
	./version.sh

imagecreate:
	docker build -t docker.siiso:prod -f dockerfileNoSDK .

containerdelete: 
	docker rm -f siisoprod

containerrun:
	docker run -p 8040:8040 --name siisoprod -d --network=host --restart=always docker.siiso:prod

siiso: gitPull publicar setVersion imagecreate containerdelete containerrun
