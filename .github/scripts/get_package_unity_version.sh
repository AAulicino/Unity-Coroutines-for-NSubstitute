#!/usr/bin/env bash

set -e
set -x

PACKAGE_MIN_VERSION=$(cat $1 | grep -oE '"unity":\s*"([^"]*)' | awk -F '"' '{print $4}')

# get the latest unity patch from major.minor present in package manifest.
echo UNITY_VERSION=$(unity-hub editors --releases | grep $PACKAGE_MIN_VERSION) >>GITHUB_ENV
