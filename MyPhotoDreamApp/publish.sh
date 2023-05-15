#!/bin/sh
dotnet publish -c Release 
#ssh root@46.17.42.116 
#rsync -R --delete -z -v bin/Release/net7.0/publish/ /home/www/photodreamapp
#sudo systemctl restart kestrel-MyPhotoDreamApp.service


