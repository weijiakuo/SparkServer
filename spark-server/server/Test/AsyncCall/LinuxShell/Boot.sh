#!/bin/bash

parentPath=$(dirname $(pwd))

cd $parentPath/../../bin/Debug/

./spark-server TestCases TestAsyncCall