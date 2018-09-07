# TimelapseMP4


## Generate api client with swagger codegen
Install [swagger-codegen](https://github.com/swagger-api/swagger-codegen)
Execute the following commands:
`cd swagger-generator
java -jar .\swagger-codegen-cli-2.3.1.jar generate -i http://localhost:5020/swagger/v1/swagger.json -l typescript-angular -o ../src/api`