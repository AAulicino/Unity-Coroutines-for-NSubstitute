#!/usr/bin/env sh

set -x
set -e

mkdir Assets
mkdir Packages
echo $(python3 $SCRIPTS_DIR/setup_unity_package_manifest.py $PACKAGE_MANIFEST) >>Packages/manifest.json
