#!/usr/bin/env sh

set -x
set -e

mkdir Assets
mkdir -p Packages/${{ github.event.repository.name }}
mv -v $(git ls-tree --full-tree --name-only HEAD) $PROJECT_ROOT
echo $(python3 $SCRIPTS_DIR/setup_unity_package_manifest.py $PACKAGE_MANIFEST) >>Packages/manifest.json
