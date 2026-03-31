---
name: run-maintenance
description: "Run standard maintenance chores: pull latest changes and ensure submodules are up to date."
---

# Run Maintenance

## Steps

1. Pull latest changes (including submodules):
   ```bash
   git pull --recurse-submodules
   ```

2. Ensure all submodules are initialised and up to date:
   ```bash
   git submodule update --init --recursive
   ```

Run both steps from the repository root before starting any release flow.
