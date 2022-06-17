#!/usr/bin/env python3

import json
import subprocess
import sys

with open(sys.argv[1]) as file:
    packageManifest = json.load(file)

version = packageManifest["unity"]

releases = subprocess.run(
    # "unity-hub editors --releases",
    "/Applications/Unity\ Hub.app/Contents/MacOS/Unity\ Hub -- --headless editors --releases",
    capture_output=True,
    text=True,
    shell=True,
).stdout

for release in releases.splitlines():
    if version in release:
        print(release.split(" ")[0])
        exit(0)

exit("Version not found")
