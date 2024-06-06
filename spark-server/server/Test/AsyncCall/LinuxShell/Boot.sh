#!/bin/bash

# boot gate server

parentPath=$(dirname $(pwd))

cd $parentPath/../../bin/Debug/

dotnet run spark-server.exe TestCases TestAsyncCall