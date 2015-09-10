#!/bin/bash

shellPath=$0
projectPath=$1

java -jar "${shellPath%/*}"/checkFile.jar "$projectPath"



