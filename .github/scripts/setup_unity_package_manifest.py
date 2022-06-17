#!/usr/bin/env python3

import os
import sys
import json

manifestPath = sys.argv[1]

with open(manifestPath) as file:
    packageManifest = json.load(file)

manifest = {
    "dependencies": {
        "com.aaulicino.nsubstitute": "https://github.com/AAulicino/Unity3D-NSubstitute.git",
        f"{packageManifest['name']}": f"file:/{os.path.dirname(manifestPath)}",
        "com.unity.test-framework": "1.1.31",
        "com.unity.testtools.codecoverage": "1.1.1"
    },
    "testables": [packageManifest['name']]
}

print (json.dumps(manifest, indent=4))