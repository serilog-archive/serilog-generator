#!/bin/sh -xe

# Test 

cd test/Serilog.Generator.Tests

dotnet test 

cd ..
cd ..

cd src/serilog-generator

dotnet publish -c Release -o out

cd ..
cd ..


# Example test script
# cd src/serilog-generator/out
# dotnet serilog-generator.dll --using=\"Serilog.Sinks.File\" --write-to:File.path=\"test.txt\"
