#!/usr/bin/env sh

set -e
set -x

PACKAGE_MIN_VERSION=$(cat $PACKAGE_MANIFEST | grep -oE '"unity":\s*"([^"]*)' | awk -F '"' '{print $4}')

echo "Unity Version: $PACKAGE_MIN_UNITY_VERSION"
echo PACKAGE_MIN_UNITY_VERSION=$(unity-hub editors --releases | grep $PACKAGE_MIN_VERSION)
