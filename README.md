# TimelapseMP4


## Generate api client with swagger codegen
Install [swagger-codegen](https://github.com/swagger-api/swagger-codegen)
Execute the following commands:
`cd swagger-generator
java -jar .\swagger-codegen-cli-2.3.1.jar generate -i http://localhost:5020/swagger/v1/swagger.json -l typescript-angular -o ../src/api`

##
sudo mkdir /timelapse
sudo mount -t cifs -o username=timelapse,password=password //192.168.1.14/projects/VWP\ Timelapse /timelapse/

dotnet publish -r linux-arm

---
Pi ffmpeg installation
http://www.jeffreythompson.org/blog/2014/11/13/installing-ffmpeg-for-raspberry-pi/

---
ffmpeg command 
http://lukemiller.org/index.php/2014/10/ffmpeg-time-lapse-notes/

--

autorest --input-file=http://vwp.timdows.com/timelapse/swagger/v1/swagger.json --csharp --output-folder=Services --namespace=TimelapseMP4.Creator.Services
