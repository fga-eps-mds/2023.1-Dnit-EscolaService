#!/bin/bash

BUILD_NUMBER="$(ls target/ | sort -r | head -n1)"
TARGET_NUMBER="target/$BUILD_NUMBER"
WORK_DIR="$TARGET_NUMBER/$(ls $TARGET_NUMBER | head -n1)"


echo "Starting build $BUILD_NUMBER";

cd $WORK_DIR
./app
