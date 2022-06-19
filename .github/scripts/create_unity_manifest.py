#!/usr/bin/env python3

import os
import sys
import json

packageName = sys.argv[1]
tgzName = sys.argv[2]

os.mkdir("Packages")

manifest = {
    "dependencies": {
        "com.aaulicino.nsubstitute": "https://github.com/AAulicino/Unity3D-NSubstitute.git",
        f"{packageName}": f"file:../{tgzName}",
        "com.unity.test-framework": "1.1.31",
        "com.unity.testtools.codecoverage": "1.1.1",
    },
    "testables": [packageName],
}

print("Generated Manifest:")
print(json.dumps(manifest, indent=4))

with open("Packages/manifest.json", "w") as file:
    json.dump(manifest, file, ensure_ascii=False, indent=4)
